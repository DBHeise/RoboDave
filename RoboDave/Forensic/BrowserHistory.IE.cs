
namespace RoboDave.Forensic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Runtime.InteropServices;
    using System.Reflection;

    internal class BrowserHistory_ie
    {
        [StructLayout(LayoutKind.Sequential)]
        public class INTERNET_CACHE_ENTRY_INFOW
        {
            public uint dwStructSize;
            public string lpszSourceUrlName;
            public string lpszLocalFileName;
            public uint CacheEntryType;
            public uint dwUseCount;
            public uint dwHitRate;
            public uint dwSizeLow;
            public uint dwSizeHigh;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;
            public IntPtr lpHeaderInfo;
            public uint dwHeaderInfoSize;
            public string lpszFileExtension;
            public uint dwReserved; //union uint dwExemptDelta;
        }

        [DllImport("wininet.dll")]
        public static extern IntPtr FindFirstUrlCacheEntryEx(

          string lpszUrlSearchPattern,
          uint dwFlags,
          uint dwFilter,
          Int64 GroupId,
          IntPtr lpFirstCacheEntryInfo,
          ref uint lpdwFirstCacheEntryInfoBufferSize,
          Pointer lpGroupAttributes,
          Pointer pcbGroupAttributes,
          Pointer lpReserved
        );

        [DllImport("wininet.dll")]
        public static extern bool FindCloseUrlCache(IntPtr hEnumHandle);

        [DllImport("wininet.dll")]
        public static extern bool FindNextUrlCacheEntryEx(
            IntPtr hEnumHandle,
            IntPtr lpFirstCacheEntryInfo,
            ref uint lpdwFirstCacheEntryInfoBufferSize,
            Pointer lpGroupAttributes,
            Pointer pcbGroupAttributes,
            Pointer lpReserved);

        public uint NORMAL_CACHE_ENTRY = 0x00000001;

        internal DateTime FromFileTime(System.Runtime.InteropServices.ComTypes.FILETIME ft)
        {
        //    int high = ft.dwHighDateTime;
        //    int low = ft.dwLowDateTime;
        //    if (high < 0) { high = high + (1 << 32); }
        //    if (low < 0) { low = low + (1 << 32); }
            long v = (((long)ft.dwHighDateTime) << 32) | ((uint)ft.dwLowDateTime);
            return DateTime.FromFileTimeUtc(v);
        }

       internal BrowserHistory[] GetHistory_WinINet()
        {
            List<BrowserHistory> history = new List<BrowserHistory>();
            IntPtr vHandle;
            INTERNET_CACHE_ENTRY_INFOW vInternetCacheEntryInfo = new INTERNET_CACHE_ENTRY_INFOW();
            uint vFirstCacheEntryInfoBufferSize = 0;
            FindFirstUrlCacheEntryEx(null, 0, NORMAL_CACHE_ENTRY, 0, (IntPtr)null,
                ref vFirstCacheEntryInfoBufferSize, null, null, null);
            IntPtr vBuffer = Marshal.AllocHGlobal((int)vFirstCacheEntryInfoBufferSize);
            vHandle = FindFirstUrlCacheEntryEx(null, 0, NORMAL_CACHE_ENTRY, 0,
               vBuffer, ref vFirstCacheEntryInfoBufferSize,
               null, null, null);
            while (vHandle != null)
            {
                Marshal.PtrToStructure(vBuffer, vInternetCacheEntryInfo);
                var bh = new BrowserHistory()
                {
                    User = "Current",
                    Browser = "WinINet",
                    Url = vInternetCacheEntryInfo.lpszSourceUrlName,
                    Timestamp = FromFileTime(vInternetCacheEntryInfo.LastAccessTime)
                };
                history.Add(bh);
                Marshal.FreeCoTaskMem(vBuffer);

                FindNextUrlCacheEntryEx(vHandle, (IntPtr)null, ref vFirstCacheEntryInfoBufferSize,
                  null, null, null);
                vBuffer = Marshal.AllocHGlobal((int)vFirstCacheEntryInfoBufferSize);
                if (!FindNextUrlCacheEntryEx(vHandle, vBuffer,
                   ref vFirstCacheEntryInfoBufferSize, null, null, null)) break;
            }
            Marshal.FreeCoTaskMem(vBuffer);
            return history.ToArray();
        }


        internal BrowserHistory[] GetHistory_IE() 
        {
            List<BrowserHistory> history = new List<BrowserHistory>();
            IUrlHistoryStg2 vUrlHistoryStg2 = (IUrlHistoryStg2)new UrlHistory();
            IEnumSTATURL vEnumSTATURL = vUrlHistoryStg2.EnumUrls();
            STATURL vSTATURL;
            uint vFectched;
            while (vEnumSTATURL.Next(1, out vSTATURL, out vFectched) == 0)
            {
                var bh = new BrowserHistory()
                {
                    User = "Current",
                    Browser = "IE",
                    Url = vSTATURL.pwcsUrl,
                    Timestamp = FromFileTime(vSTATURL.ftLastVisited)
                };
                history.Add(bh);                
            }
            //vUrlHistoryStg2.ClearHistory();//Clear the history
            return history.ToArray();
        }


        struct STATURL
        {
            public static uint SIZEOF_STATURL =
                (uint)Marshal.SizeOf(typeof(STATURL));

            public uint cbSize;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwcsUrl;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwcsTitle;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastVisited,
                ftLastUpdated,
                ftExpires;
            public uint dwFlags;
        }

        [ComImport, Guid("3C374A42-BAE4-11CF-BF7D-00AA006946EE"),
            InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IEnumSTATURL
        {
            [PreserveSig]
            uint Next(uint celt, out STATURL rgelt, out uint pceltFetched);
            void Skip(uint celt);
            void Reset();
            void Clone(out IEnumSTATURL ppenum);
            void SetFilter(
                [MarshalAs(UnmanagedType.LPWStr)] string poszFilter,
                uint dwFlags);
        }

        [ComImport, Guid("AFA0DC11-C313-11d0-831A-00C04FD5AE38"),
            InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IUrlHistoryStg2
        {
            #region IUrlHistoryStg methods
            void AddUrl(
                [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
                [MarshalAs(UnmanagedType.LPWStr)] string pocsTitle,
                uint dwFlags);

            void DeleteUrl(
                [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
                uint dwFlags);

            void QueryUrl(
                [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
                uint dwFlags,
                ref STATURL lpSTATURL);

            void BindToObject(
                [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
                ref Guid riid,
                [MarshalAs(UnmanagedType.IUnknown)] out object ppvOut);

            IEnumSTATURL EnumUrls();
            #endregion

            void AddUrlAndNotify(
                [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
                [MarshalAs(UnmanagedType.LPWStr)] string pocsTitle,
                uint dwFlags,
                [MarshalAs(UnmanagedType.Bool)] bool fWriteHistory,
                [MarshalAs(UnmanagedType.IUnknown)] object /*IOleCommandTarget*/
                poctNotify,
                [MarshalAs(UnmanagedType.IUnknown)] object punkISFolder);

            void ClearHistory();
        }

        [ComImport, Guid("3C374A40-BAE4-11CF-BF7D-00AA006946EE")]
        class UrlHistory /* : IUrlHistoryStg[2] */ { }
    }
}

