namespace Pokemon3D.Common.ScriptPipeline
{
    /// <summary>
    /// Easier access to optional and/or weakly typed parameters.
    /// </summary>
    public sealed class ParameterHelper
    {
        private object[] _stack;
        private int _index;

        public ParameterHelper(object[] parameters)
        {
            _stack = parameters;
        }
        
        /// <summary>
        /// Removes the top most item from the parameter stack and returns it.
        /// </summary>
        public T Pop<T>(T defaultValue = default(T))
        {
            if (IsEmpty())
                return defaultValue;

            T result;

            if (_stack[_index] != null)
                result = (T)_stack[_index];
            else
                result = defaultValue;

            _index++;
            return result;
        }

        /// <summary>
        /// Skips over items on the stack.
        /// </summary>
        public void Skip(int steps = 1)
        {
            _index += steps;
            if (_index > _stack.Length)
                _index = _stack.Length;
        }

        /// <summary>
        /// Returns if the stack is empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _index == _stack.Length;
        }

        /// <summary>
        /// Returns the amount of items left on the stack.
        /// </summary>
        /// <returns></returns>
        public int GetStackSize()
        {
            return _stack.Length - _index;
        }
    }
}
