﻿namespace Pokemon3D.Scripting.Types
{
    /// <summary>
    /// A proxy "this" object for the global script context.
    /// </summary>
    internal class GlobalContextObject : SObject
    {
        // The context this object was created for.
        private readonly ScriptContext _context;

        public GlobalContextObject(ScriptContext context)
        {
            _context = context;
        }

        internal override bool HasMember(ScriptProcessor processor, string memberName)
        {
            return _context.IsVariable(memberName);
        }

        internal override SObject GetMember(ScriptProcessor processor, SObject accessor, bool isIndexer)
        {
            string memberName;
            if (accessor is SString)
                memberName = ((SString)accessor).Value;
            else
                memberName = accessor.ToString(processor).Value;

            if (_context.IsVariable(memberName))
            {
                return _context.GetVariable(memberName);
            }
            else
            {
                return processor.ErrorHandler.ThrowError(ErrorType.ReferenceError, ErrorHandler.MessageReferenceNotDefined, memberName );
            }
        }

        internal override void SetMember(ScriptProcessor processor, SObject accessor, bool isIndexer, SObject value)
        {
            string memberName;
            if (accessor is SString)
                memberName = ((SString)accessor).Value;
            else
                memberName = accessor.ToString(processor).Value;

            if (_context.IsVariable(memberName))
            {
                _context.GetVariable(memberName).Data = value;
            }
            else
            {
                processor.ErrorHandler.ThrowError(ErrorType.ReferenceError, ErrorHandler.MessageReferenceNotDefined, memberName );
            }
        }

        internal override SObject ExecuteMethod(ScriptProcessor processor, string methodName, SObject caller, SObject This, SObject[] parameters)
        {
            // The statement that entered this method looks like this: this.varName(), where varName is a variable containing an SFunction object.

            if (_context.IsVariable(methodName))
            {
                var methodVariable = _context.GetVariable(methodName);
                var method = methodVariable.Data;

                if (method is SFunction)
                {
                    return ((SFunction)method).Call(processor, caller, This, parameters);
                }
                else
                {
                    return processor.ErrorHandler.ThrowError(ErrorType.TypeError, ErrorHandler.MessageTypeNotAFunction,  methodName );
                }
            }
            return processor.ErrorHandler.ThrowError(ErrorType.ReferenceError, ErrorHandler.MessageReferenceNotDefined,  methodName );
        }

        internal override double SizeOf()
        {
            return 1;
        }
    }
}
