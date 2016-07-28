using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Types;
using Pokemon3D.Scripting.Types.Prototypes;

namespace Pokemon3D.Scripting.Adapters
{
    /// <summary>
    /// An exception thrown by the scripting runtime environment.
    /// </summary>
    public class ScriptRuntimeException : Exception
    {
        internal SError ErrorObject { get; private set; }

        /// <summary>
        /// The type of error that occurred.
        /// </summary>
        public string Type => GetStringMember(ErrorObject, ErrorPrototype.MEMBER_NAME_TYPE);

        /// <summary>
        /// The line in the source of the script the error occurred on.
        /// </summary>
        public int Line => (int)GetNumberMember(ErrorObject, ErrorPrototype.MEMBER_NAME_LINE);

        internal ScriptRuntimeException(SError errorObject)
            : base(GetStringMember(errorObject, ErrorPrototype.MEMBER_NAME_MESSAGE))
        {
            ErrorObject = errorObject;
        }

        private static string GetStringMember(SProtoObject obj, string member)
        {
            var memberObj = SObject.Unbox(obj.Members[member]);

            if (memberObj is SString)
                return ((SString)memberObj).Value;
            else
                return "";
        }

        private static double GetNumberMember(SProtoObject obj, string member)
        {
            var memberObj = SObject.Unbox(obj.Members[member]);

            if (memberObj is SNumber)
                return ((SNumber)memberObj).Value;
            else
                return -1;
        }
    }
}
