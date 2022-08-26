/*
 * MoesifAPI.PCL
 *

 */
using System;
using Moesif.Api.Controllers;
using Moesif.Api.Http.Client;

namespace Moesif.Api
{
    public partial class MoesifApiClient
    {

        /// <summary>
        /// Singleton access to Api controller
        /// </summary>
        public ApiController Api
        {
            get
            {
                return Moesif.Api.Controllers.ApiController.Instance;
            }
        }

        /// <summary>
        /// Singleton access to Health controller
        /// </summary>
        public HealthController Health
        {
            get
            {
                return Moesif.Api.Controllers.HealthController.Instance;
            }
        }

        /// <summary>
        /// The shared http client to use for all API calls
        /// </summary>
        public IHttpClient SharedHttpClient
        {
            get
            {
                return BaseController.ClientInstance;
            }
            set
            {
                BaseController.ClientInstance = value;
            }        
        }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        private MoesifApiClient() { }

        /// <summary>
        /// Client initialization constructor
        /// </summary>
        public MoesifApiClient(
            string applicationId,
            string userAgentString = null,
            bool debug = false,
            string baseUri = "https://api.moesif.net"
        ) {
            Configuration.ApplicationId = applicationId;
            Configuration.UserAgentString = userAgentString;
            Configuration.Debug = debug;
            Configuration.BaseUri = baseUri;
        }
    }
}