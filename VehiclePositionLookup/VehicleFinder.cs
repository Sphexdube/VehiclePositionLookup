using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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

            Parallel.ForEach(coords, coord =>
            {
                vehiclePositionList.Add(GetNearest(vehiclePositions, coord.Latitude, coord.Longitude));
            });

            stopwatch.Stop();

            Console.WriteLine($"Data file read execution time : {(object)elapsedMilliseconds} ms");
            Console.WriteLine($"Closest position calculation execution time : {(object)stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Total execution time : {(object)(elapsedMilliseconds + stopwatch.ElapsedMilliseconds)} ms");
            Console.WriteLine();
        }

        internal static VehiclePosition GetNearest(List<VehiclePosition> vehiclePositions, float latitude, float longitude)
        {
            double nearestDistance = 0;
            VehiclePosition nearest = null;

            Parallel.ForEach(vehiclePositions, vehiclePosition => 
            {
                double num = UtilityMethods.DistanceBetween(latitude, longitude, vehiclePosition.Latitude, vehiclePosition.Longitude);
                if (num < nearestDistance)
                {
                    nearest = vehiclePosition;
                    nearestDistance = num;
                }
            });

            return nearest;
        }
    }
}

