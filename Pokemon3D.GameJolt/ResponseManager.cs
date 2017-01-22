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
        private readonly ResponseModel _dataModel;
        private readonly bool _parsingSuccessful;

        public ResponseManager(string responseData)
        {
            try
            {
                _parsingSuccessful = true;
                _dataModel = DataModel<ResponseModel>.FromString(responseData);
            }
            catch
            {
                _parsingSuccessful = false;
            }
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
