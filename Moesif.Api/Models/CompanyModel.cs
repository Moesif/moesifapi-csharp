﻿/*
 * MoesifAPI.PCL
 *

 */
using System;
// using System.IO;
// using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using Moesif.Api;

#if NET6_0_OR_GREATER
    using System.Text.Json.Serialization;
#else
     using Newtonsoft.Json;
#endif

namespace Moesif.Api.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class CompanyModel : INotifyPropertyChanged
    {
        // These fields hold the values for the public properties.
        private DateTime? modifiedTime;
        private string sessionToken;
        private string ipAddress;
        private string companyId;
        private string companyDomain;
        private object metadata;
        private CampaignModel campaign;

        /// <summary>
        /// Time when request was made
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("modified_time")]
#else
        [JsonProperty("modified_time")]
#endif
        public DateTime ModifiedTime
        {
            get
            {
                return this.modifiedTime ?? DateTime.UtcNow;
            }
            set
            {
                this.modifiedTime = value;
                onPropertyChanged("ModifiedTime");
            }
        }

        /// <summary>
        /// End user's auth/session token
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("session_token")]
#else
        [JsonProperty("session_token")]
#endif
        public string SessionToken
        {
            get
            {
                return this.sessionToken;
            }
            set
            {
                this.sessionToken = value;
                onPropertyChanged("SessionToken");
            }
        }

        /// <summary>
        /// End user's ip address
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("ip_address")]
#else
        [JsonProperty("ip_address")]
#endif
        public string IpAddress
        {
            get
            {
                return this.ipAddress;
            }
            set
            {
                this.ipAddress = value;
                onPropertyChanged("IpAddress");
            }
        }

        /// <summary>
        /// End user's user_id string from your app
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("company_id")]
#else
        [JsonProperty("company_id")]
#endif
        public string CompanyId
        {
            get
            {
                return this.companyId;
            }
            set
            {
                this.companyId = value;
                onPropertyChanged("CompanyId");
            }
        }

        /// <summary>
        /// End user's user_agent_string string from your app
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("company_domain")]
#else
        [JsonProperty("company_domain")]
#endif
        public string CompanyDomain
        {
            get
            {
                return this.companyDomain;
            }
            set
            {
                this.companyDomain = value;
                onPropertyChanged("CompanyDomain");
            }
        }

        /// <summary>
        /// Metadata from your app, see documentation
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("metadata")]
#else
        [JsonProperty("metadata")]
#endif
        public object Metadata
        {
            get
            {
                return this.metadata;
            }
            set
            {
                this.metadata = value;
                onPropertyChanged("Metadata");
            }
        }

        /// <summary>
        /// Campaign object
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("campaign")]
#else
        [JsonProperty("campaign")]
#endif
        public CampaignModel Campaign 
        { 
            get 
            {
                return this.campaign; 
            } 
            set 
            {
                this.campaign = value;
                onPropertyChanged("Campaign");
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
