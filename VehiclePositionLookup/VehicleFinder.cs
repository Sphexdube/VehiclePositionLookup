using System;
using System.Collections.Generic;
using System.Diagnostics;
using VehiclePositionLookup.Models;
using VehiclePositionLookup.Utility;

namespace VehiclePositionLookup
{
    internal class VehicleFinder
    {
        internal static void FindClosest(Coordinate[] coords)
        {
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<VehiclePosition> vehiclePositions = DataFileParser.ReadDataFile();

            stopwatch.Stop();

            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            foreach (Coordinate coord in coords)
            {
                vehiclePositionList.Add(GetNearest(vehiclePositions, coord.Latitude, coord.Longitude, out double _));
            }

            stopwatch.Stop();

            DisplayExecutionTimes(stopwatch, elapsedMilliseconds);
        }

        internal static VehiclePosition GetNearest(List<VehiclePosition> vehiclePositions, float latitude, float longitude, out double nearestDistance)
        {
            VehiclePosition nearest = (VehiclePosition)null;
            nearestDistance = double.MaxValue;
            foreach (VehiclePosition vehiclePosition in vehiclePositions)
            {
                double num = UtilityMethods.DistanceBetween(latitude, longitude, vehiclePosition.Latitude, vehiclePosition.Longitude);
                if (num < nearestDistance)
                {
                    nearest = vehiclePosition;
                    nearestDistance = num;
                }
            }
            return nearest;
        }

        private static void DisplayExecutionTimes(Stopwatch stopwatch, long elapsedMilliseconds)
        {
            Console.WriteLine($"Data file read execution time : {(object)elapsedMilliseconds} ms");
            Console.WriteLine($"Closest position calculation execution time : {(object)stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Total execution time : {(object)(elapsedMilliseconds + stopwatch.ElapsedMilliseconds)} ms");
            Console.WriteLine();
        }
    }
}

