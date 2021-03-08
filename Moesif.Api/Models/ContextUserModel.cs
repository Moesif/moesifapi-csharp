using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Moesif.Api.Models
{
    public class ContextModel : INotifyPropertyChanged
    {
        private ContextUserModel user;

        [JsonProperty("user")]
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

    public class ContextUserModel : INotifyPropertyChanged 
    {
        private String id;
        private String email;
        private String firstName;
        private String lastName;

        [JsonProperty("Id")]
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

        [JsonProperty("Email")]
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

        [JsonProperty("FirstName")]
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
        [JsonProperty("LastName")]
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
            ContextUserModel m = null;
            if (!string.IsNullOrWhiteSpace(jsonStr))
                m = JsonConvert.DeserializeObject<ContextUserModel>(jsonStr.Trim());
            return m;
        }
    }
}