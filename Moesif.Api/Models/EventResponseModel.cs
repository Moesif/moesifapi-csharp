/*
 * MoesifAPI.PCL
 *

 */
using System;
// using System.IO;
using System.Collections.Generic;
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
        [JsonPropertyName("time")]
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
        [JsonPropertyName("status")]
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
        [JsonPropertyName("headers")]
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
        [JsonPropertyName("body")]
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
        [JsonPropertyName("ip_address")]
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
        [JsonPropertyName("transfer_encoding")]
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