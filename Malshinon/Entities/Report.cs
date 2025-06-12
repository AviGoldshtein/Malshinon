using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Entities
{
    internal class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\n" +
                $"ReporterId: {ReporterId}\n" +
                $"TargetId: {TargetId}\n" +
                $"Timestamp: {Timestamp}\n" +
                $"Text: {Text}\n";
        }
        public static void PrintListReports(List<Report> reports)
        {
            foreach (Report report in reports)
            {
                Console.WriteLine(report);
            }
        }
    }
}
