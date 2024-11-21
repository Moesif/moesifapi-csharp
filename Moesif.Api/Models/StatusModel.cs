/*
 * MoesifAPI.PCL
 *

 */
using System;
// using System.IO;
// using System.Collections.Generic;
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
    public class StatusModel : INotifyPropertyChanged 
    {
        // These fields hold the values for the public properties.
        private bool status;
        private string region;

        /// <summary>
        /// Status of Call
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("status")]
#else
        [JsonProperty("status")]
#endif
        public bool Status 
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
        /// Location
        /// </summary>
#if NET6_0_OR_GREATER
        [JsonPropertyName("region")]
#else
        [JsonProperty("region")]
#endif
        public string Region 
        { 
            get 
            {
                return this.region; 
            } 
            set 
            {
                this.region = value;
                onPropertyChanged("Region");
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