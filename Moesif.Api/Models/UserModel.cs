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
    public class UserModel: INotifyPropertyChanged
    {
        // These fields hold the values for the public properties.
        private DateTime? modifiedTime;
        private string sessionToken;
        private string ipAddress;
        private string userId;
        private string companyId;
        private string userAgentString;
        private object metadata;
        private CampaignModel campaign;

        /// <summary>
        /// Time when request was made
        /// </summary>
        [JsonProperty("modified_time")]
        public DateTime ModifiedTime
        {
            get
            {
                return this.modifiedTime??DateTime.UtcNow;
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
        [JsonProperty("session_token")]
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
        [JsonProperty("ip_address")]
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
        [JsonProperty("user_id")]
        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
                onPropertyChanged("UserId");
            }
        }

        /// <summary>
        /// End user's company_id string from your app
        /// </summary>
        [JsonProperty("company_id")]
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
        [JsonProperty("user_agent_string")]
        public string UserAgentString
        {
            get
            {
                return this.userAgentString;
            }
            set
            {
                this.userAgentString = value;
                onPropertyChanged("UserAgentString");
            }
        }

        /// <summary>
        /// Metadata from your app, see documentation
        /// </summary>
        [JsonProperty("metadata")]
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
        [JsonProperty("campaign")]
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
