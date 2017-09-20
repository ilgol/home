using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Update.Models
{
    public class DigiBitData
    {
        public string username { get; set; }
        public double confirmed_rewards { get; set; }
        public string round_estimate { get; set; }
        public string total_hashrate { get; set; }
        public double payout_history { get; set; }
        public string round_shares { get; set; }
        public Dictionary<string, Worker> Workers { get; set; }
    }
}
