using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wbaab.Models
{
    public class thresholdModel
    {
        public int thresholdSeqCd { get; set; }
        public string thresholdName { get; set; }
        public int thresholdTime { get; set; }
        public bool isEdit { get; set; }
    }
}