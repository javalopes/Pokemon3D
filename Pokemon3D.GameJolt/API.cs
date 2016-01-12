using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Pokemon3D.GameJolt
{
    /// <summary>
    /// Handles requests to the Game Jolt Game API.
    /// </summary>
    public partial class API
    {
        /// <summary>
        /// The Id of the game.
        /// </summary>
        public string GameId { get; private set; }

        /// <summary>
        /// The private key of the game. This is used to create the signature of the API requests.
        /// </summary>
        public string GameKey { get; private set; }

        /// <summary>
        /// Creates a new instance of the Game Jolt API interface.
        /// </summary>
        /// <param name="gameId">The id of the game.</param>
        /// <param name="gameKey">The private key of the game.</param>
        public API(string gameId, string gameKey)
        {
            GameId = gameId;
            GameKey = gameKey;
        }

        /// <summary>
        /// Executes one or more Game Jolt API calls.
        /// </summary>
        /// <param name="calls">The list of calls.</param>
        /// <param name="format">The response format of the calls.</param>
        /// <param name="responseHandler">The response handler method that accepts the response from the Game Jolt server as a <see cref="string"/>.</param>
        /// <param name="parallelProcessing">If the Game Jolt API should process the calls simultaniously.</param>
        /// <param name="stopOnError">If the Game Jolt API should stop processing, if an error occurred during one call.</param>
        public void ExecuteCalls(APICall[] calls, ResponseFormat format, Action<string> responseHandler, bool parallelProcessing = true, bool stopOnError = false)
        {
            string formatStr = ResponseFormatToString(format);

            // build url from parameters:
            string url = string.Format(FORMAT_CALL_URL, HOST, VERSION, GameId, formatStr);
            if (parallelProcessing)
                url += "&parallel=true";
            if (stopOnError)
                url += "&stop_on_error=true";

            // add signature to url:
            var urlSignature = CreateSignature(url);
            url += "&signature=" + urlSignature;

            // the request will be a POST request, and all actual api requests will be stored in the post data:
            StringBuilder postDataBuilder = new StringBuilder("data=");

            foreach (APICall call in calls)
            {
                string callUrl = call.CreateUrl(this);
                string callSignature = CreateSignature(callUrl);
                callUrl += "&signature=" + callSignature;

                postDataBuilder.Append("&requests[]=" + UrlEncoder.Encode(callUrl));
            }

            string postData = postDataBuilder.ToString();

            try
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    // create post requests:
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.AllowWriteStreamBuffering = true;
                    request.Method = "POST";
                    request.ContentLength = postData.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ServicePoint.Expect100Continue = false;

                    StreamWriter writer = null;
                    StreamReader reader = null;
                    string responseStr = string.Empty; // stores the result.

                    try
                    {
                        // write post data to stream:
                        writer = new StreamWriter(request.GetRequestStream());
                        writer.Write(postData);
                        writer.Close();

                        // get request response, read from stream:
                        reader = new StreamReader(request.GetResponse().GetResponseStream());
                        responseStr = reader.ReadToEnd();
                    }
                    catch { } // suppress exceptions
                    finally
                    {
                        if (writer != null)
                            writer.Close();
                        if (reader != null)
                            reader.Close();
                    }

                    // call handle:
                    responseHandler(responseStr);
                });
            }
            catch { } // suppress exceptions
        }

        /// <summary>
        /// Creates a signature for an API call url.
        /// </summary>
        private string CreateSignature(string url)
        {
            MD5 md5 = MD5.Create();

            // the string used as hash is the url (without "signature" parameter) and the game's key.
            string hashString = url + GameKey;

            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(hashString));

            StringBuilder byteBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                byteBuilder.Append(bytes[i].ToString("x2"));

            return byteBuilder.ToString();
        }
    }
}
