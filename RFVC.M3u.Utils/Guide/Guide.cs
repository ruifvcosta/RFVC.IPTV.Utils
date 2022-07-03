using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFVC.IPTV.Guide
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Guide
    {

        private tvChannel[] channelField;

        private tvProgramme[] programmeField;

        private string generatorinfonameField;

        private string generatorinfourlField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("channel")]
        public tvChannel[] channel
        {
            get
            {
                return this.channelField;
            }
            set
            {
                this.channelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("programme")]
        public tvProgramme[] programme
        {
            get
            {
                return this.programmeField;
            }
            set
            {
                this.programmeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("generator-info-name")]
        public string generatorinfoname
        {
            get
            {
                return this.generatorinfonameField;
            }
            set
            {
                this.generatorinfonameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("generator-info-url")]
        public string generatorinfourl
        {
            get
            {
                return this.generatorinfourlField;
            }
            set
            {
                this.generatorinfourlField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class tvChannel
    {

        private string displaynameField;

        private tvChannelIcon iconField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("display-name")]
        public string displayname
        {
            get
            {
                return this.displaynameField;
            }
            set
            {
                this.displaynameField = value;
            }
        }

        /// <remarks/>
        public tvChannelIcon icon
        {
            get
            {
                return this.iconField;
            }
            set
            {
                this.iconField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class tvChannelIcon
    {

        private string srcField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string src
        {
            get
            {
                return this.srcField;
            }
            set
            {
                this.srcField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class tvProgramme
    {

        private string titleField;

        private string descField;

        private string startField;

        private string stopField;

        private string channelField;

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string desc
        {
            get
            {
                return this.descField;
            }
            set
            {
                this.descField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string start
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string stop
        {
            get
            {
                return this.stopField;
            }
            set
            {
                this.stopField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string channel
        {
            get
            {
                return this.channelField;
            }
            set
            {
                this.channelField = value;
            }
        }

    }
}
