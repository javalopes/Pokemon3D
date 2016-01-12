using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ResponseManager(string responseData)
        {
            _dataModel = DataModel<ResponseModel>.FromString(responseData);
        }
        
        public bool AllCallsSuccessful
        {
            get { return _dataModel.Response.Responses.All(r => r != null && r.Success); }
        }

        public CallResponseModel[] Responses
        {
            get { return _dataModel.Response.Responses; }
        }

        public bool BatchSuccessful
        {
            get { return _dataModel.Response.Success; }
        }
    }
}
