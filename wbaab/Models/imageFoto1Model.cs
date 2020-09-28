using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wbaab.Models
{
    public class imageFoto1Model
    {
        public int imageFoto1SeqCd { get; set; }
        public string imageFoto1Path { get; set; }
        public string imageFoto1Title { get; set; }
        public string imageFoto1Note { get; set; }
        public bool isEdit { get; set; }
    }
}