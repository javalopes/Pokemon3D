using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json;
using Pokemon3D.GameModes;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// A <see cref="DataRequest{T}"/> to request a <see cref="DataModel{T}"/> from a file.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="DataModel{T}"/>.</typeparam>
    class DataModelRequest<T> : DataRequest where T : DataModel<T>
    {
        /// <summary>
        /// The final model created from the result data.
        /// </summary>
        public T ResultModel { get; private set; }

        public DataModelRequest(GameMode gameMode, string dataPath) : base(gameMode, dataPath)
        {
            Finished += RequestFinished;
        }

        private void RequestFinished(object sender, EventArgs e)
        {
            ResultModel = DataModel<T>.FromString(ResultData);
        }
    }
}
