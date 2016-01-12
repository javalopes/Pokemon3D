using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameJolt
{
    /// <summary>
    /// A call to a Game Jolt API endpoint.
    /// </summary>
    public class APICall
    {
        /// <summary>
        /// The API endpoint this call connects to.
        /// </summary>
        internal string EndpointUrl { get; private set; }

        /// <summary>
        /// The URL parameters used by this call.
        /// </summary>
        internal Dictionary<string, string> Parameters { get; }

        internal APICall(string endpointUrl) : this(endpointUrl, new Dictionary<string, string>()) { }

        internal APICall(string endpointUrl, Dictionary<string, string> parameters)
        {
            EndpointUrl = endpointUrl;
            Parameters = parameters;
        }

        private const string FORMAT_URL = "/{0}/?game_id={1}";
        private const string FORMAT_PARAMETER = "&{0}={1}";

        internal string GetUrl(API api)
        {
            EndpointUrl = EndpointUrl.Trim('/');

            StringBuilder urlBuilder = new StringBuilder(string.Format(FORMAT_URL, EndpointUrl, UrlEncoder.Encode(api.GameId)));
            
            for (int i = 0; i < Parameters.Count; i++)
                urlBuilder.Append(string.Format(FORMAT_PARAMETER, Parameters.Keys.ElementAt(i), UrlEncoder.Encode(Parameters.Values.ElementAt(i))));

            return urlBuilder.ToString();
        }
    }
}
