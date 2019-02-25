

namespace RoboDave.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Text;

    [Cmdlet(VerbsCommon.New, "RandomCoordinate", SupportsShouldProcess = true)]
    [OutputType(typeof(RoboDave.Geo.Coordinate))]
    public class RandomCoordinateCmdlet : PSCmdlet
    {
        private Random rnd = new Random();

        protected override void BeginProcessing()
        {
            var lat = (rnd.NextDouble() * 180) - 90; //-90 to 90
            var lon = (rnd.NextDouble() * 360) - 180; //-180 to 180
            var ans = new RoboDave.Geo.Coordinate(lat, lon);
            WriteObject(ans);
        }
    }
}
