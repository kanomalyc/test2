using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ParkingApp
{
    public class TicketService
    {

        public void ReadAndUpdateCSV(string filePath, string outputFilePath)
        {
            Ticket[] tickets = ReadCSV(filePath);
            tickets = UpdateTickets(tickets);
            WriteCSV(outputFilePath,tickets);

            // Total of fines issued per year per make of vehicle
            Console.WriteLine("Total of fines issued per year per make of vehicle:");
            foreach (var make in tickets.GroupBy(t => t.Make))
            {
                Console.WriteLine($"Make: {make.Key}");
                foreach (var year in make.GroupBy(t => t.IssueDate.Year))
                {
                    Console.WriteLine($"\tYear: {year.Key}, Total Fines: ${year.Sum(t => t.FineAmount)}");
                }
            }
            // Average and Standard Deviation of the fine amount per year per agency
            Console.WriteLine("\nAverage and Standard Deviation of the fine amount per year per agency:");
            foreach (var agency in tickets.GroupBy(t => t.Agency))
            {
                Console.WriteLine($"Agency: {agency.Key}");
                foreach (var year in agency.GroupBy(t => t.IssueDate.Year))
                {
                    var avg = year.Average(t => t.FineAmount);
                    var stdDev = Math.Sqrt(year.Average(t => Math.Pow(t.FineAmount - avg, 2)));
                    Console.WriteLine($"\tYear: {year.Key}, Average Fine: ${avg:0.##}, Standard Deviation: ${stdDev:0.##}");
                }
            }
        }

        private Ticket[] ReadCSV(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var header = lines[0].Split(',');
            var ticketList = new List<Ticket>();

            for (int i = 1; i < lines.Length; i++)
            {
                var fields = lines[i].Split(',');
                var ticket = new Ticket();

                if (fields.Length > 0 && !string.IsNullOrEmpty(fields[0]))
                {
                    ticket.TicketNumber = long.Parse(fields[0]);
                }
                else
                {
                    ticket.TicketNumber = 0;
                }
                ticket.IssueDateStr = fields[1];
                if (fields.Length > 2 && !string.IsNullOrEmpty(fields[2]))
                {
                    ticket.IssueTime = int.Parse(fields[2]);
                }
                else
                {
                    ticket.IssueTime = 0;
                }
                ticket.MeterId = fields[3];
                ticket.MarkedTime = fields[4];
                ticket.RPStatePlate = fields[5];
                ticket.PlateExpiryDateStr = fields[6];
                ticket.VIN = fields[7];
                ticket.Make = fields[8];
                ticket.BodyStyle = fields[9];
                ticket.Color = fields[10];
                ticket.Location = fields[11];
                ticket.Route = fields[12];
                try
                {
                    ticket.Agency = int.Parse(fields[13]);

                }
                catch (FormatException ex)
                {
                    Console.WriteLine("An error occurred while parsing the agency: " + ex.Message);
                }
                ticket.ViolationCode = fields[14];
                ticket.ViolationDescription = fields[15];
                try
                {
                    ticket.FineAmount = int.Parse(fields[16]);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("An error occurred while parsing the fine amount: " + ex.Message);
                }
                ticket.Latitude = double.Parse(fields[17]);
                ticket.Longitude = double.Parse(fields[18]);
                ticket.AgencyDescription = fields[19];
                ticket.ColorDescription = fields[20];
                ticket.BodyStyleDescription = fields[21];

                ticketList.Add(ticket);
            }

            return ticketList.ToArray();
        }

        private Ticket[] UpdateTickets(Ticket[] tickets)
        {
            foreach (var ticket in tickets)
            {
                // Combine the Issue Date and Issue Time into a single Issue Date Time value
                ticket.IssueDate = DateTime.Parse(ticket.IssueDateStr);

                // Convert the plate expiry value into a date value with the date as the last day of the month
                ticket.PlateExpiryDate = new DateTime(int.Parse(ticket.PlateExpiryDateStr.Substring(0, 4)),
                                                      int.Parse(ticket.PlateExpiryDateStr.Substring(4, 2)), 1)
                                                      .AddMonths(1).AddDays(-1);


                // Convert invalid latitude and longitude values into none values
                if (ticket.Latitude == 99999)
                {
                    ticket.Latitude = double.NaN;
                }

                if (ticket.Longitude == 99999)
                {
                    ticket.Longitude = double.NaN;
                }
            }

            return tickets;
        }
        private void WriteCSV(string outputFilePath, Ticket[] tickets)
        {
            var header = "TicketNumber,IssueDate,IssueTime,MeterId,MarkedTime,RPStatePlate,PlateExpiryDate,VIN,Make,BodyStyle,Color,Location,Route,Agency,ViolationCode,ViolationDescription,FineAmount,Latitude,Longitude,AgencyDescription,ColorDescription,BodyStyleDescription";
            var ticketLines = new List<string>();
            foreach (var ticket in tickets)
            {
                var ticketLine = $"{ticket.TicketNumber},{ticket.IssueDate.ToString("yyyy-MM-dd")},{ticket.IssueTime},{ticket.MeterId},{ticket.MarkedTime}," +
                                 $"{ticket.RPStatePlate},{ticket.PlateExpiryDate.ToString("yyyy-MM-dd")},{ticket.VIN},{ticket.Make},{ticket.BodyStyle},{ticket.Color}," +
                                 $"{ticket.Location},{ticket.Route},{ticket.Agency},{ticket.ViolationCode},{ticket.ViolationDescription},{ticket.FineAmount}," +
                                 $"{ticket.Latitude},{ticket.Longitude},{ticket.AgencyDescription},{ticket.ColorDescription},{ticket.BodyStyleDescription}";
                ticketLines.Add(ticketLine);
            }

            File.WriteAllLines(outputFilePath, new[] { header }.Concat(ticketLines));
        }
    }   
}