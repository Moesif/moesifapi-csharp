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
    public class EventModel : INotifyPropertyChanged 
    {
        // These fields hold the values for the public properties.
        private EventRequestModel request;
        private EventResponseModel response;
        private ContextModel context;
        private string sessionToken;
        private string tags;
        private string userId;
        private string companyId;
        private object metadata;
        private string direction;
        private int weight;
        private string blockedBy;

        /// <summary>
        /// API request object
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("request")]
#else
        [JsonProperty("request")]
#endif
        public EventRequestModel Request 
        { 
            get 
            {
                return this.request; 
            } 
            set 
            {
                this.request = value;
                onPropertyChanged("Request");
            }
        }

        /// <summary>
        /// API response Object
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("response")]
#else
        [JsonProperty("response")]
#endif
        public EventResponseModel Response 
        { 
            get 
            {
                return this.response; 
            } 
            set 
            {
                this.response = value;
                onPropertyChanged("Response");
            }
        }

        /// <summary>
        /// context.Request.User from Apim policy
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("context")]
#else
        [JsonProperty("context")]
#endif
        public ContextModel Context 
        { 
            get 
            {
                return this.context; 
            } 
            set 
            {
                this.context = value;
                onPropertyChanged("Context");
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
        /// comma separated list of tags, see documentation
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("tags")]
#else
        [JsonProperty("tags")]
#endif
        public string Tags 
        { 
            get 
            {
                return this.tags; 
            } 
            set 
            {
                this.tags = value;
                onPropertyChanged("Tags");
            }
        }

        /// <summary>
        /// End user's user_id string from your app
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("user_id")]
#else
        [JsonProperty("user_id")]
#endif
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
        /// company_id string
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
        /// direction string
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("direction")]
#else
        [JsonProperty("direction")]
#endif
        public string Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
                onPropertyChanged("Direction");
            }
        }

        /// <summary>
        /// Weight of an API call
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("weight")]
#else
        [JsonProperty("weight")]
#endif
        public int Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                this.weight = value;
                onPropertyChanged("Weight");
            }
        }

#if NET6_0_OR_GREATER
        [JsonPropertyName("blocked_by")]
#else
        [JsonProperty("blocked_by")]
#endif
        public string BlockedBy
        {
            get
            {
                return this.blockedBy;
            }
            set
            {
                this.blockedBy = value;
                onPropertyChanged("BlockedBy");
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