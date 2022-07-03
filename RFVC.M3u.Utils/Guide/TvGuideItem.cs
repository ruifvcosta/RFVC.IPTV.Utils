using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFVC.IPTV.Guide
{
    public class TvGuideItem
    {

        #region Properties

        public string Channel { get; }
        public string Title { get; }
        public string Desc { get; }
        public DateTime Start { get; }
        public DateTime Stop { get; }


        #endregion

        public TvGuideItem(string channel, string title, string descrition, DateTime start, DateTime finish)
        {
            Channel = channel;
            Title = title;
            Desc = descrition;
            Start = start;
            Stop = finish;
        }

    
    }
}

