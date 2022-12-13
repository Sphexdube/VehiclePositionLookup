using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            var listOfFilterdVehiclePositions = GetListOfFilterdCoordinates(vehiclePositions, coords);

            stopwatch.Stop();

            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            Parallel.ForEach(coords, coord =>
            {
                vehiclePositionList.Add(GetNearest(listOfFilterdVehiclePositions, coord.Latitude, coord.Longitude));
            });

            stopwatch.Stop();

            Console.WriteLine($"Data file read execution time : {(object)elapsedMilliseconds} ms");
            Console.WriteLine($"Closest position calculation execution time : {(object)stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Total execution time : {(object)(elapsedMilliseconds + stopwatch.ElapsedMilliseconds)} ms");

            if (vehiclePositionList.Count > 0)
            {
                DisplayVehiclePositions(vehiclePositionList);
            }
            
            Console.WriteLine();
        }

        private static void DisplayVehiclePositions(List<VehiclePosition> vehiclePositionList)
        {
            Console.WriteLine($"The nearest vehicle position of the 10 coordinates provided: \n");

            foreach (var vehicle in vehiclePositionList)
            {
                Console.WriteLine($"ID: {vehicle.ID} Registration: {vehicle.Registration} Latitude: {vehicle.Latitude} Longitude: {vehicle.Longitude} RecordedTimeUTC: { vehicle.RecordedTimeUTC}");
            }
        }

        private static List<VehiclePosition> GetListOfFilterdCoordinates(List<VehiclePosition> vehiclePositions, Coordinate[] coords)
        {
            Coordinate minCoordinate, maxCoordinate;

            GetMinMaxCoordinates(coords, out minCoordinate, out maxCoordinate);

            return (from p in vehiclePositions
                    where (p.Latitude >= minCoordinate.Latitude && p.Latitude <= maxCoordinate.Latitude) &&
                           (p.Longitude >= minCoordinate.Longitude && p.Longitude <= maxCoordinate.Longitude)
                    select p).ToList();
        }

        private static void GetMinMaxCoordinates(Coordinate[] coords, out Coordinate minCoordinate, out Coordinate maxCoordinate)
        {
            minCoordinate = new Coordinate()
            {
                Latitude = coords.Min(p => p.Latitude),
                Longitude = coords.Min(p => p.Longitude)
            };
            maxCoordinate = new Coordinate()
            {
                Latitude = coords.Max(p => p.Latitude),
                Longitude = coords.Max(p => p.Longitude)
            };
        }

        internal static VehiclePosition GetNearest(List<VehiclePosition> vehiclePositions, float latitude, float longitude)
        {
            double nearestDistance = double.MaxValue;
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

