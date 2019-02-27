using RoboDave.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Generators
{

    public class Person
    {
        public String GivenName { get; set; }
        public String MiddleName { get; set; }
        public String SurName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        //Gender, Race, Birthday
        //Height, Weight, Hair Color, Blood Type
        //Address, Geo Coordinates
        //Educational Background, Martial Status
        //SSN, Passport, Drivers License
        //Employment, Salary, Occupation, Company, Industry
        //Credit Card - CCN, CVV2, Expires
        //Image, Tagline, website/blog
        //D&D Stats        
        //Favorites: Movie, TV, Book, Artist, Song
    }

    [Cmdlet(VerbsCommon.New, "RandomPerson", SupportsShouldProcess = true)]
    [OutputType(typeof(Person))]
    public class RandomPersonCmdlet : Cmdlet
    {
        private static String CombineNameForEmail(String firstname, String lastname)
        {
            String ans;
            var i = Rando.RandomInt(0, 10);
            switch (i)
            {
                case 0:
                    ans = firstname + lastname;
                    break;
                case 1:
                    ans = firstname + "_" + lastname;
                    break;
                case 2:
                    ans = firstname + lastname[0];
                    break;
                case 3:
                    ans = firstname[0] + lastname;
                    break;
                case 4:
                    ans = lastname + firstname[0] + firstname[1];
                    break;
                default:
                    ans = firstname + "." + lastname;
                    break;
            }
            return ans.Replace("'", "");
        }
        private static String GenerateEmail(Person p)
        {
            String ans;

            var user = CombineNameForEmail(p.GivenName, p.SurName);
            var n = Rando.RandomInt(1, 5);
            var commonTLD = new String[] { "com", "com", "com", "com", "com", "com", "com", "com", "com", "com", "com", "com", "net", "org", "edu", "org", "edu" };

            switch (n)
            {
                case 1:
                    ans = user + "@" + MadLibHelper.madlib.Generate("[adjective][noun]") + "." + StringGenerator.GetString(StringType.TLD);
                    break;
                default:
                    ans = user + "@" + MadLibHelper.madlib.Generate("[adjective][noun]") + "." + commonTLD.GetRandomItem();
                    break;
            }

            return ans.ToLowerInvariant();

        }

        protected override void ProcessRecord()
        {
            var p = new Person();
            p.GivenName = MadLibHelper.madlib.Generate("[name]");
            p.SurName = MadLibHelper.madlib.Generate("[lastname]");
            p.Email = GenerateEmail(p);

            if (!Rando.RandomBoolean(3))
            {
                p.MiddleName = StringGenerator.GetString(StringType.UpperCase, 1);
            }

            p.PhoneNumber = "(" + StringGenerator.GetString(StringType.Digits, 3) + ") " + StringGenerator.GetString(StringType.Digits, 3) + "-" + StringGenerator.GetString(StringType.Digits, 4);

            WriteObject(p);
        }
    }
}
