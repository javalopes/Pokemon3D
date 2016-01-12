using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.DataModel.Serialization
{
    interface DataSerializer<T>
    {
        T FromString(string data);

        string ToString(DataModel<T> dataModel);
    }
}
