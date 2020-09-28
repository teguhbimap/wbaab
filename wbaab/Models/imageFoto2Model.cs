using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wbaab.Models
{
    public class imageFoto2Model
    {
        public int imageFoto2SeqCd { get; set; }
        public string imageFoto2Path { get; set; }
        public string imageFoto2Title { get; set; }
        public string imageFoto2Note { get; set; }
        public bool isEdit { get; set; }
    }
}