using System;
using System.IO;

namespace ParkingApp
{
    public class Ticket
    {
        public long TicketNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public int IssueTime { get; set; }
        public string MeterId { get; set; }
        public string MarkedTime { get; set; }
        public string RPStatePlate { get; set; }
        public DateTime PlateExpiryDate { get; set; }
        public string VIN { get; set; }
        public string Make { get; set; }
        public string BodyStyle { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public string Route { get; set; }
        public int Agency { get; set; }
        public string ViolationCode { get; set; }
        public string ViolationDescription { get; set; }
        public int FineAmount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AgencyDescription { get; set; }
        public string ColorDescription { get; set; }
        public string BodyStyleDescription { get; set; }
        public string IssueDateStr { get; set; }
        public string PlateExpiryDateStr { get; set; }
    }
}
