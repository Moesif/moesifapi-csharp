using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

#if NET6_0_OR_GREATER
    using System.Text.Json.Serialization;
#else
    using Newtonsoft.Json;
#endif

namespace Moesif.Api.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class ContextModel : INotifyPropertyChanged
    {
        private ContextUserModel user;

#if NET6_0_OR_GREATER
        [JsonPropertyName("user")]
#else
        [JsonProperty("user")]
#endif
        public ContextUserModel User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
                onPropertyChanged("User");
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

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class ContextUserModel : INotifyPropertyChanged 
    {
        private String id;
        private String email;
        private String firstName;
        private String lastName;

#if NET6_0_OR_GREATER
        [JsonPropertyName("Id")]
#else
        [JsonProperty("Id")]
#endif
        public String Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                onPropertyChanged("Id");
            }
        }

#if NET6_0_OR_GREATER
        [JsonPropertyName("Email")]
#else
        [JsonProperty("Email")]
#endif
        public String Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
                onPropertyChanged("Email");
            }
        }

#if NET6_0_OR_GREATER
        [JsonPropertyName("FirstName")]
#else
        [JsonProperty("FirstName")]
#endif
        public String FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = value;
                onPropertyChanged("FirstName");
            }
        }

#if NET6_0_OR_GREATER
        [JsonPropertyName("LastName")]
#else
        [JsonProperty("LastName")]
#endif
        public String LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = value;
                onPropertyChanged("LastName");
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

        public static ContextUserModel deserialize(String jsonStr)
        {
//            ContextUserModel m = null;
//            if (!string.IsNullOrWhiteSpace(jsonStr))
//                m = JsonConvert.DeserializeObject<ContextUserModel>(jsonStr.Trim());
//            return m;
            return ApiHelper.JsonDeserialize<ContextUserModel>(jsonStr);
        }
    }
}