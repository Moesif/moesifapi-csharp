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
        [JsonProperty("time")]
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
        [JsonProperty("uri")]
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
        [JsonProperty("verb")]
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
        [JsonProperty("headers")]
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
        [JsonProperty("api_version")]
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
        /// Request body
        /// </summary>
        [JsonProperty("body")]
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
        [JsonProperty("transfer_encoding")]
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