/*
 * MoesifAPI.PCL
 *

 */
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using Moesif.Api;

#if NET6_0_OR_GREATER
    using System.Text.Json.Serialization;
#else
     using System.Linq;
     using System.Text;
     using System.Threading.Tasks;
     using Newtonsoft.Json;
     using Newtonsoft.Json.Converters;
//     using System.Text.Json;
#endif

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
#if NET6_0_OR_GREATER
        [JsonPropertyName("utm_source")]
#else
        [JsonProperty("utm_source")]
#endif
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
#if NET6_0_OR_GREATER
        [JsonPropertyName("utm_medium")]
#else
        [JsonProperty("utm_medium")]
#endif
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
#if NET6_0_OR_GREATER
        [JsonPropertyName("utm_campaign")]
#else
        [JsonProperty("utm_campaign")]
#endif
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
#if NET6_0_OR_GREATER
        [JsonPropertyName("utm_term")]
#else
        [JsonProperty("utm_term")]
#endif
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
#if NET6_0_OR_GREATER
        [JsonPropertyName("utm_content")]
#else
        [JsonProperty("utm_content")]
#endif
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
#if NET6_0_OR_GREATER
        [JsonPropertyName("referrer")]
#else
        [JsonProperty("referrer")]
#endif
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
#if NET6_0_OR_GREATER
        [JsonPropertyName("referring_domain")]
#else
        [JsonProperty("referring_domain")]
#endif
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
#if NET6_0_OR_GREATER
        [JsonPropertyName("gclid")]
#else
        [JsonProperty("gclid")]
#endif
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
