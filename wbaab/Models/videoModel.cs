using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wbaab.Models
{
    public class videoModel
    {
        public int videoSeqCd { get; set; }
        public string videoPath { get; set; }
        public string videoTitle { get; set; }
        public string videoNote { get; set; }
        public bool isEdit { get; set; }
    }
}