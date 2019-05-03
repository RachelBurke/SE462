using System.Collections.Generic;
using System.Linq;

namespace FerryLegacy
{
    public class BookingManager
    {
        // Private Attributes
        private List<Booking> _bookings;

        // Determines if a user can book a particular journey based off of what they need (# passengers, # tickets, weight)
        private bool CanBook(int journeyId, int passengers, int vehicles, int weight)
        {
            foreach (var journey in ManagementSystem.GetAllJourneys())
            {
                if (journey.Id == journeyId)
                {
                    var bookings = _bookings.Where(x => x.Journey.Id == journeyId);
                    var seatsLeft = journey.Ferry.Passengers - bookings.Sum(x => x.Passengers);
                    var vehiclesLeft = journey.Ferry.Vehicles - bookings.Sum(x => x.Vehicles);
                    var weightLeft = journey.Ferry.Weight - bookings.Sum(x => x.Weight);
                    return seatsLeft >= passengers && vehiclesLeft >= vehicles && weightLeft >= weight;
                }
            }
            return false;
        }

        // Bookings Constructor - initialize booking list
        public BookingManager()
        {
            _bookings = new List<Booking>();
        }

        // Returns a list of bookings for a particular journey
        public List<Booking> GetJourneyBookings(int id)
        {
            return _bookings.Where(x => x.Journey.Id == id).ToList();
        }

        // Returns a list of all the bookings
        public List<Booking> GetAllBookings()
        {
            return _bookings;
        }

        // Books a journey if possible
        public bool Book(int journeyId, int passengers, int vehicles, int weight)
        {
            if(CanBook(journeyId, passengers, vehicles, weight))
            {
                _bookings.Add(new Booking
                {
                    Journey = ManagementSystem.GetJourney(journeyId),
                    Passengers = passengers,
                    Vehicles = vehicles,
                    Weight = weight
                });
                return true;
            }
            return false;
        }
    }
}