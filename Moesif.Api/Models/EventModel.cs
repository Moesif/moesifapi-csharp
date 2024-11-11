/*
 * MoesifAPI.PCL
 *

 */
using System;
// using System.IO;
// using System.Collections.Generic;
using System.ComponentModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Converters;
// using System.Text.Json;
using System.Text.Json.Serialization;
// using Moesif.Api;

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
        [JsonPropertyName("request")]
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
        [JsonPropertyName("response")]
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
        [JsonPropertyName("context")]
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
        [JsonPropertyName("session_token")]
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
        [JsonPropertyName("tags")]
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
        [JsonPropertyName("user_id")]
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
        [JsonPropertyName("company_id")]
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
        [JsonPropertyName("metadata")]
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
        [JsonPropertyName("direction")]
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
        [JsonPropertyName("weight")]
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

        [JsonPropertyName("blocked_by")]
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