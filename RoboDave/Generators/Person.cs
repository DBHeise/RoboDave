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
        public String MiddleInitial { get; set; }
        public String SurName { get; set; }
        public String Email { get; set; }
        //Gender, Race, Birthday
        //Address, Geo Coordinates
        //Height, Weight, Hair Color, Blood Type
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
            var i = Rando.RandomInt(0, 5);
            switch (i)
            {
                case 0:
                    return firstname + lastname;
                case 1:
                    return firstname + "_" + lastname;
                case 2:
                    return firstname + lastname[0];
                case 3:
                    return firstname[0] + lastname;
                default:
                    return firstname + "." + lastname;                    
            }            
        }


        protected override void ProcessRecord()
        {
            var p = new Person();
            p.GivenName = MadLibHelper.madlib.Generate("[name]");
            p.SurName = MadLibHelper.madlib.Generate("[lastname]");
            p.Email = (CombineNameForEmail(p.GivenName, p.SurName) + "@" + MadLibHelper.madlib.Generate("[adjective][noun]") + "." + StringGenerator.GetString(StringType.TLD)).ToLowerInvariant();
            
            if (Rando.RandomBoolean(3))
            {
                p.MiddleInitial = StringGenerator.GetString(StringType.UpperCase, 1);
            }

            WriteObject(p);
        }
    }
}
