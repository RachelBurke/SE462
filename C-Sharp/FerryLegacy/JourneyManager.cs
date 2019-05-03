using System;
using System.Collections.Generic;
using System.Linq;

namespace FerryLegacy
{
    class JourneyManager
    {
        //Private Attributes
        private static List<Journey> _journeys;

        // Creates a list of journeys to travel on
        private static void CreateJourneys()
        {
            var timetable = ManagementSystem.GetFullTimeTable().OrderBy(x => x.Time).ThenBy(x => x.OriginId);
            var ports = ManagementSystem.GetAllPorts();

            foreach (var entry in timetable)
            {
                Port fromPort = ports.Where(x => x.Id == entry.OriginId).Single();
                Port toPort = ports.Where(x => x.Id == entry.DestinationId).Single();
                Ferry ferry = ManagementSystem.GetNextAvailableFerry(fromPort, toPort, entry.Time);
                Journey journey = new Journey
                {
                    Id = entry.Id,
                    Origin = fromPort,
                    Destination = toPort,
                    Departure = entry.Time,
                    Travel = entry.JourneyTime,
                    Arrival = entry.Time + entry.JourneyTime,
                    Ferry = ferry,
                    Seats = ferry.Passengers,
                    Vehicles = ferry.Vehicles,
                    Weight = ferry.Weight
                };

                _journeys.Add(journey);
                ManagementSystem.SetFerryJourney(ferry, journey);
                ManagementSystem.MoveFerry(entry.OriginId, entry.DestinationId, ferry);
            }
        }

        // Constructor to initialize the journey list
        public JourneyManager()
        {
            _journeys = new List<Journey>();
            CreateJourneys();
        }

        // Returns a specific journey
        public Journey GetJourney(int journeyId)
        {
            return _journeys.Single(x => x.Id == journeyId);
        }

        // Returns a list of all the journeys
        public List<Journey> GetAllJourneys()
        {
            return _journeys;
        }

        // Returns a list of available journeys
        public List<Journey> GetAvailableJourneys(int fromPort, int toPort, TimeSpan time)
        {
            List<Journey> available = new List<Journey>();
            foreach (var journey in _journeys)
            {
                if (toPort == journey.Destination.Id && fromPort == journey.Origin.Id)
                {
                    if (journey.Departure >= time)
                    {
                        List<Booking> bookings = ManagementSystem.GetBookings(journey.Id);
                        var seatsLeft = journey.Ferry.Passengers - bookings.Sum(x => x.Passengers);
                        var vehiclesLeft = journey.Ferry.Vehicles - bookings.Sum(x => x.Vehicles);
                        var weightLeft = journey.Ferry.Weight - bookings.Sum(x => x.Weight);
                        if (seatsLeft > 0)
                        {
                            journey.Seats = seatsLeft;
                            journey.Vehicles = vehiclesLeft;
                            journey.Weight = weightLeft;
                            available.Add(journey);
                        }
                    }
                }
            }
            return available;
        }
    }
}
