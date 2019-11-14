/*
 * MoesifAPI.PCL
 *

 */
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Moesif.Api;

namespace Moesif.Api.Models
{
    public class CampaignModel : INotifyPropertyChanged
    {
        // These fields hold the values for the public properties.
        private string utmSource;
        private string utmMedium;
        private string utmCampaign;
        private string utmTerm;
        private string utmContent;
        private string referrer;
        private string referringDomain;
        private string gclid;

        /// <summary>
        /// the utm source
        /// </summary>
        [JsonProperty("utm_source")]
        public string UtmSource
        {
            get
            {
                return this.utmSource;
            }
            set
            {
                this.utmSource = value;
                onPropertyChanged("UtmSource");
            }
        }

        /// <summary>
        /// the utm medium
        /// </summary>
        [JsonProperty("utm_medium")]
        public string UtmMedium
        {
            get
            {
                return this.utmMedium;
            }
            set
            {
                this.utmMedium = value;
                onPropertyChanged("UtmMedium");
            }
        }

        /// <summary>
        /// the utm campaign
        /// </summary>
        [JsonProperty("utm_campaign")]
        public string UtmCampaign
        {
            get
            {
                return this.utmCampaign;
            }
            set
            {
                this.utmCampaign = value;
                onPropertyChanged("UtmCampaign");
            }
        }

        /// <summary>
        /// the utm term
        /// </summary>
        [JsonProperty("utm_term")]
        public string UtmTerm
        {
            get
            {
                return this.utmTerm;
            }
            set
            {
                this.utmTerm = value;
                onPropertyChanged("UtmTerm");
            }
        }

        /// <summary>
        /// the utm Content
        /// </summary>
        [JsonProperty("utm_content")]
        public string UtmContent
        {
            get
            {
                return this.utmContent;
            }
            set
            {
                this.utmContent = value;
                onPropertyChanged("UtmContent");
            }
        }

        /// <summary>
        /// the referrer
        /// </summary>
        [JsonProperty("referrer")]
        public string Referrer
        {
            get
            {
                return this.referrer;
            }
            set
            {
                this.referrer = value;
                onPropertyChanged("Referrer");
            }
        }

        /// <summary>
        /// the referring domain
        /// </summary>
        [JsonProperty("referring_domain")]
        public string ReferringDomain
        {
            get
            {
                return this.referringDomain;
            }
            set
            {
                this.referringDomain = value;
                onPropertyChanged("ReferringDomain");
            }
        }

        /// <summary>
        /// the gclid
        /// </summary>
        [JsonProperty("gclid")]
        public string Gclid
        {
            get
            {
                return this.gclid;
            }
            set
            {
                this.gclid = value;
                onPropertyChanged("Gclid");
            }
        }

        /// <summary>
        /// Property changed event for observer pattern
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises event when a property is changed
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        protected void onPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
