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
    public class EventRequestModel : INotifyPropertyChanged 
    {
        // These fields hold the values for the public properties.
        private DateTime time;
        private string uri;
        private string verb;
        private Dictionary<string, string> headers;
        private string apiVersion;
        private string ipAddress;
        private object body;
        private string transferEncoding;

        /// <summary>
        /// Time when request was made
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("time")]
#else
        [JsonProperty("time")]
#endif
        public DateTime Time 
        { 
            get 
            {
                return this.time; 
            } 
            set 
            {
                this.time = value;
                onPropertyChanged("Time");
            }
        }

        /// <summary>
        /// full uri of request such as https://www.example.com/my_path?param=1
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("uri")]
#else
        [JsonProperty("uri")]
#endif
        public string Uri 
        { 
            get 
            {
                return this.uri; 
            } 
            set 
            {
                this.uri = value;
                onPropertyChanged("Uri");
            }
        }

        /// <summary>
        /// verb of the API request such as GET or POST
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("verb")]
#else
        [JsonProperty("verb")]
#endif
        public string Verb 
        { 
            get 
            {
                return this.verb; 
            } 
            set 
            {
                this.verb = value;
                onPropertyChanged("Verb");
            }
        }

        /// <summary>
        /// Key/Value map of request headers
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("headers")]
#else
        [JsonProperty("headers")]
#endif
        public Dictionary<string, string> Headers 
        { 
            get 
            {
                return this.headers; 
            } 
            set 
            {
                this.headers = value;
                onPropertyChanged("Headers");
            }
        }

        /// <summary>
        /// Optionally tag the call with your API or App version
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("api_version")]
#else
        [JsonProperty("api_version")]
#endif
        public string ApiVersion 
        { 
            get 
            {
                return this.apiVersion; 
            } 
            set 
            {
                this.apiVersion = value;
                onPropertyChanged("ApiVersion");
            }
        }

        /// <summary>
        /// IP Address of the client if known.
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
        /// Request body
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("body")]
#else
        [JsonProperty("body")]
#endif
        public object Body 
        { 
            get 
            {
                return this.body; 
            } 
            set 
            {
                this.body = value;
                onPropertyChanged("Body");
            }
        }

        /// <summary>
        /// Transfer Encoding of the body such as "base64", null value implies "json" transfer encoding.
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("transfer_encoding")]
#else
        [JsonProperty("transfer_encoding")]
#endif
        public string TransferEncoding
        {
            get
            {
                return this.transferEncoding;
            }
            set
            {
                this.transferEncoding = value;
                onPropertyChanged("TransferEncoding");
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