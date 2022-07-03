using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFVC.IPTV.M3u
{

    public class M3uFileItem
    {
        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? LogoLocation { get; set; }

        public string? GuideID { get; set; }

        public string? GuideName { get; set; }
        
        public string? Group { get;  set; }

        public int Type { get; set; }
    }

}
