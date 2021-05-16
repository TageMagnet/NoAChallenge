using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoaChallenge.Models
{
    public class ColorModel
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public Color[] data { get; set; }
    }

    public class Color
    {
        public int id { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public string color { get; set; }
        public string pantone_value { get; set; }
    }

}
