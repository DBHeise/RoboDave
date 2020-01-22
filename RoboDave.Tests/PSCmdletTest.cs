using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Tests
{
    public class PSCmdletTest
    {
        protected List<String> testFiles;
        protected Runspace runspace;
        protected PowerShell ps;

        [TestInitialize]
        public void TestInit()
        {
            testFiles = new List<String>();

            var initState = InitialSessionState.CreateDefault();
            initState.LanguageMode = PSLanguageMode.FullLanguage;
            initState.ThrowOnRunspaceOpenError = true;
            initState.ThreadOptions = PSThreadOptions.UseCurrentThread;

            var folder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            initState.ImportPSModule(new string[] { System.IO.Path.Combine(folder, "RoboDave.dll") });

            runspace = RunspaceFactory.CreateRunspace(initState);
            runspace.Open();

            ps = PowerShell.Create();
            ps.Runspace = runspace;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ps.Dispose();
            foreach (String file in testFiles)
            {
                System.IO.File.Delete(file);
            }
        }


        protected List<T> DoInvoke<T>()
        {
            List<T> ans = new List<T>();
            var results = ps.Invoke();
            Assert.IsFalse(ps.HadErrors);
            foreach (PSObject result in results)
            {
                Assert.IsTrue(result.BaseObject is T);
                ans.Add((T)result.BaseObject);
            }
            return ans;
        }
    }
}
