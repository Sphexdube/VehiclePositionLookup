using GeoCoordinatePortable;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace VehiclePositionLookup.Utility
{
    public static class UtilityMethods
    {
        internal const string subDirectory = "files";

        internal static DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, 0);

        internal static string GetLocalFilePath(string fileName) => GetLocalFilePath(subDirectory, fileName);

        internal static double DistanceBetween(float latitude1, float longitude1, float latitude2, float longitude2)
        {
            return new GeoCoordinate((double)latitude1, (double)longitude1).GetDistanceTo(new GeoCoordinate((double)latitude2, (double)longitude2));
        }

        internal static string GetLocalFilePath(string subDirectory, string fileName) => Path.Combine(GetLocalPath(subDirectory), fileName);

        internal static byte[] ToNullTerminatedString(string registration)
        {
            byte[] bytes = Encoding.Default.GetBytes(registration);
            byte[] terminatedString = new byte[bytes.Length + 1];
            bytes.CopyTo((Array)terminatedString, 0);
            return terminatedString;
        }

        internal static string GetLocalPath(string subDirectory)
        {
            string path1 = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            if (subDirectory != string.Empty)
                path1 = Path.Combine(path1, subDirectory);
            return path1;
        }

        internal static string SelectRandom(Random rnd, string[] values)
        {
            int index = rnd.Next(values.Length - 1);
            return values[index];
        }

        internal static ulong ToCTime(DateTime time) => Convert.ToUInt64((time - Epoch).TotalSeconds);

        internal static DateTime FromCTime(ulong cTime) => Epoch.AddSeconds((double)cTime);
    }
}

