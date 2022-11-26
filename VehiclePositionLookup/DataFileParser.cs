using System;
using System.Collections.Generic;
using System.IO;
using VehiclePositionLookup.Utility;

namespace VehiclePositionLookup
{
    public class DataFileParser
    {
        internal static List<VehiclePosition> ReadDataFile()
        {
            byte[] data = ReadFileData();
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            int offset = 0;
            while (offset < data.Length)
                vehiclePositionList.Add(ReadVehiclePosition(data, ref offset));
            return vehiclePositionList;
        }

        internal static byte[] ReadFileData()
        {
            string localFilePath = UtilityMethods.GetLocalFilePath("VehiclePositions.dat");
            if (File.Exists(localFilePath))
                return File.ReadAllBytes(localFilePath);
            Console.WriteLine("Data file not found.");
            return (byte[])null;
        }

        private static VehiclePosition ReadVehiclePosition(byte[] data, ref int offset) => VehiclePosition.FromBytes(data, ref offset);
    }
}