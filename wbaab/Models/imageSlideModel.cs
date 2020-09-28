using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wbaab.Models
{
    public class imageSlideModel
    {
        public int imageSlideSeqCd { get; set; }
        public string imageSlidePath { get; set; }
        public string imageSlideNote1 { get; set; }
        public string imageSlideNote2 { get; set; }
        public bool isEdit { get; set; }
    }
}