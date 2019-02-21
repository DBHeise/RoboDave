namespace RoboDave.Geo
{
    using System;

    [Serializable]
    public class Coordinate : IComparable
    {
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public Coordinate(Double latitude, Double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Latitude, this.Longitude);
        }

        public int CompareTo(object obj)
        {
            return this.GetHashCode() - obj.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Coordinate other = (Coordinate)obj;

            return (this.Latitude == other.Latitude &&
                this.Longitude == other.Longitude);
        }

        public static bool operator ==(Coordinate item1, Coordinate item2)
        {
            return (item1.Latitude == item2.Latitude && item1.Longitude == item2.Longitude);
        }

        public static bool operator !=(Coordinate item1, Coordinate item2)
        {
            return (item1.Latitude != item2.Latitude || item1.Longitude != item2.Longitude);
        }

        public override int GetHashCode()
        {
            return this.Latitude.GetHashCode() ^ this.Longitude.GetHashCode();
        }

        #region Static Helper Methods
        /// <summary>
        /// Finds a new coordinate based on an initial coordinate, a distance (feet), and a bearing (degrees from North)
        /// </summary>
        /// <param name="start">Initial Coordinate</param>
        /// <param name="distance">Distance (in feet)</param>
        /// <param name="bearing">Bearing (in degrees from North) </param>
        /// <returns>new coordinate</returns>
        public static Coordinate FindPoint(Coordinate start, Double distance, Double bearing)
        {
            distance = distance / R;
            bearing = DegreesToRadians(bearing);

            Double lat1 = DegreesToRadians(start.Latitude);
            Double lon1 = DegreesToRadians(start.Longitude);

            Double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(distance) + Math.Cos(lat1) * Math.Sin(distance) * Math.Cos(bearing));
            Double lon2 = lon1 + Math.Atan2(Math.Sin(bearing) * Math.Sin(distance) * Math.Cos(lat1), Math.Cos(distance) - Math.Sin(lat1) * Math.Sin(lat2));
            lon2 = (lon2 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;

            return new Coordinate(RadiansToDegrees(lat2), RadiansToDegrees(lon2));
        }

        /// <summary>
        /// Finds the bearing (in degrees from North) from two points
        /// </summary>
        /// <param name="one">One Point</param>
        /// <param name="two">Another Point</param>
        /// <returns></returns>
        public static Double FindBearing(Coordinate one, Coordinate two)
        {
            Double dLon = DegreesToRadians(two.Latitude - one.Longitude);
            Double lat1 = DegreesToRadians(one.Latitude);
            Double lat2 = DegreesToRadians(two.Latitude);

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) -
                    Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            var brng = Math.Atan2(y, x);

            return (RadiansToDegrees(Math.Atan2(y, x)) + 360) % 360;
        }

        /// <summary>
        /// Finds the mid point between two points
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static Coordinate FindMidPoint(Coordinate one, Coordinate two)
        {
            Double lat1 = DegreesToRadians(one.Latitude);
            Double lon1 = DegreesToRadians(one.Longitude);
            Double lat2 = DegreesToRadians(two.Latitude);
            Double dLon = DegreesToRadians(two.Longitude - one.Longitude);

            Double Bx = Math.Cos(lat2) * Math.Cos(dLon);
            Double By = Math.Cos(lat2) * Math.Sin(dLon);

            Double lat3 = Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2),
                              Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By));
            Double lon3 = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);
            lon3 = (lon3 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;

            return new Coordinate(RadiansToDegrees(lat3), RadiansToDegrees(lon3));
        }

        /// <summary>
        /// Returns the distance (in feet) between two coordinates using the haversine formula
        /// </summary>
        /// <param name="one">First coordinate point</param>
        /// <param name="two">Second coordinate point</param>
        /// <returns>distance in feet</returns>
        public static Double FindDistance(Coordinate one, Coordinate two)
        {
            Double lat1 = DegreesToRadians(one.Latitude);
            Double lon1 = DegreesToRadians(one.Longitude);
            Double lat2 = DegreesToRadians(two.Latitude);
            Double lon2 = DegreesToRadians(two.Longitude);
            Double dLat = lat2 - lat1;
            Double dLon = lon2 - lon1;

            Double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            Double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            Double d = R * c;
            return d;
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="angle">angle in degrees</param>
        /// <returns></returns>
        public static Double DegreesToRadians(Double angle)
        {
            return angle * Math.PI / 180.0;
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="angle">angle in radians</param>
        /// <returns></returns>
        public static Double RadiansToDegrees(Double angle)
        {
            return angle * 180.0 / Math.PI;
        }

        /// <summary>
        /// This is the standard mean radius of the earth in feet
        /// </summary>
        private static Double R = 20902259.7804591; // feet
        #endregion
    }
}
