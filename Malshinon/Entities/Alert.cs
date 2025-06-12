using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Entities
{
    internal class Alert
    {
        public int Id { get; set; }
        public int TargetId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\n" +
                $"TargetId: {TargetId}\n" +
                $"CreatedAt: {CreatedAt}\n" +
                $"Reason: {Reason}\n";
        }
        public static void PrintListAlerts(List<Alert> alerts)
        {
            foreach(Alert alert in alerts)
            {
                Console.WriteLine(alert);
            }
        }
    }
}
