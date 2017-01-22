using System.Linq;
using Pokemon3D.DataModel.GameJolt;
using Pokemon3D.DataModel;

namespace Pokemon3D.GameJolt
{
    /// <summary>
    /// Manages the response data from a Game Jolt API call.
    /// </summary>
    public sealed class ResponseManager
    {
        private ResponseModel _dataModel;
        private bool _parsingSuccessful;

        public ResponseManager(string responseData)
        {
            try
            {
                _dataModel = DataModel<ResponseModel>.FromString(responseData);
                _parsingSuccessful = true;
            }
            catch { }
        }
        
        public bool AllCallsSuccessful
            => _parsingSuccessful && _dataModel.Response.Responses.All(r => r != null && r.Success);
        
        public CallResponseModel[] Responses
        {
            get 
            {
                if (_parsingSuccessful)
                    return _dataModel.Response.Responses;
                    
                return new CallResponseModel[0];
            }
        }

        public bool BatchSuccessful
            => _parsingSuccessful && _dataModel.Response.Success;
    }
}
