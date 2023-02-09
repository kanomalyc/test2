using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingApp
{
    public class TicketTester
    {
        public static void Main(string[] args)
        {
            string filePath = @"parking-citations-sm.csv";
            string outputFilePath = @"parking-citations-sm-updated.csv";

            if (args.Length >= 1)
                filePath = args[0];
            if (args.Length >= 2)
                outputFilePath = args[1];


            var TicketService = new TicketService();
            TicketService.ReadAndUpdateCSV(filePath, outputFilePath);
        }
    }
}