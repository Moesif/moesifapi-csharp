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
    public class EventResponseModel : INotifyPropertyChanged 
    {
        // These fields hold the values for the public properties.
        private DateTime time;
        private int status;
        private Dictionary<string, string> headers;
        private object body;
        private string ipAddress;
        private string transferEncoding;

        /// <summary>
        /// Time when response received
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
        /// HTTP Status code such as 200
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("status")]
#else
        [JsonProperty("status")]
#endif
        public int Status 
        { 
            get 
            {
                return this.status; 
            } 
            set 
            {
                this.status = value;
                onPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Key/Value map of response headers
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
        /// Response body
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
        /// IP Address from the response, such as the server IP Address
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