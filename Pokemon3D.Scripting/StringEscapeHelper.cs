using System.Linq;
using System.Text;

namespace Pokemon3D.Scripting
{
    /// <summary>
    /// A string escaper searching an expression from left to right.
    /// </summary>
    internal class LeftToRightStringEscapeHelper : StringEscapeHelper
    {
        private bool _isEscaped;

        /// <summary>
        /// Creates a new instance of the <see cref="LeftToRightStringEscapeHelper"/> class, setting it to an index within the expression and ignoring every char in front of that index.
        /// </summary>
        internal LeftToRightStringEscapeHelper(string expression, int startIndex, bool ignoreStart) : base(expression)
        {
            if (ignoreStart)
            {
                Index = startIndex;
            }
            else
            {
                if (HasStringsValue)
                {
                    while (startIndex > Index)
                    {
                        CheckNext();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LeftToRightStringEscapeHelper"/> class, setting searching up to a specific index in the expression.
        /// </summary>
        internal LeftToRightStringEscapeHelper(string expression, int startIndex) : this(expression, startIndex, false) { }

        internal override void CheckStartAt(int startIndex)
        {
            if (HasStringsValue)
            {
                if (startIndex < Index)
                {
                    Index = 0;
                    CheckStartAt(startIndex);
                }
                else
                {
                    while (startIndex > Index)
                    {
                        CheckNext();
                    }
                    CheckNext();
                }
            }
        }

        protected sealed override void CheckNext()
        {
            var t = Expression[Index];

            if (EndOfString)
            {
                EndOfString = false;
                IsStringValue = false;
            }

            if (t == StringDelimiterSingle || t == StringDelimiterDouble)
            {
                if (!IsStringValue)
                {
                    IsStringValue = true;
                    StartChar = t;
                }
                else
                {
                    if (!_isEscaped && t == StartChar)
                    {
                        EndOfString = true;
                    }
                    else
                    {
                        _isEscaped = false;
                    }
                }
            }
            else if (t == '\\')
            {
                _isEscaped = !_isEscaped;
            }
            else
            {
                if (_isEscaped)
                    _isEscaped = false;
            }

            Index++;
        }
    }

    /// <summary>
    /// A string escaper searching an expression from right to left.
    /// </summary>
    internal class RightToLeftStringEscapeHelper : StringEscapeHelper
    {

        /// <summary>
        /// Creates a new instance of the <see cref="RightToLeftStringEscapeHelper"/> class, setting it to an index within the expression and ignoring every char before that index.
        /// </summary>
        internal RightToLeftStringEscapeHelper(string expression, int startIndex, bool ignoreStart) : base(expression)
        {
            if (ignoreStart)
            {
                Index = startIndex;
            }
            else
            {
                if (HasStringsValue)
                {
                    while (startIndex < Index)
                    {
                        CheckNext();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RightToLeftStringEscapeHelper"/> class, setting searching up to a specific index in the expression.
        /// </summary>
        internal RightToLeftStringEscapeHelper(string expression, int startIndex) : this(expression, startIndex, false) { }

        internal override void CheckStartAt(int startIndex)
        {
            if (HasStringsValue)
            {
                if (startIndex > Index)
                {
                    Index = Expression.Length - 1;
                    CheckStartAt(startIndex);
                }
                else
                {
                    while (startIndex < Index)
                    {
                        CheckNext();
                    }
                    CheckNext();
                }
            }
        }

        protected override void CheckNext()
        {
            var t = Expression[Index];

            if (EndOfString)
            {
                EndOfString = false;
                IsStringValue = false;
            }

            if (t == StringDelimiterSingle || t == StringDelimiterDouble)
            {
                if (IsStringValue && t == StartChar)
                {
                    var cIndex = Index - 1;
                    var isEscaped = false;

                    while (cIndex >= 0 && Expression[cIndex] == '\\')
                    {
                        isEscaped = !isEscaped;
                        cIndex--;
                    }

                    if (!isEscaped)
                    {
                        EndOfString = true;
                    }
                }
                else if (!IsStringValue)
                {
                    IsStringValue = true;
                    StartChar = t;
                }
            }

            Index--;
        }
    }

    /// <summary>
    /// A base class for classes to implement searches for string delimiters and wrappers.
    /// </summary>
    internal abstract class StringEscapeHelper
    {
        protected internal const char StringDelimiterSingle = '\'';
        protected internal const char StringDelimiterDouble = '\"';

        protected bool IsStringValue;
        protected string Expression;
        protected int Index;
        protected char StartChar;
        protected bool EndOfString;
        protected bool HasStringsValue;

        protected internal StringEscapeHelper(string expression)
        {
            Expression = expression;
            HasStringsValue = HasStrings(expression);
        }

        /// <summary>
        /// Returns if the <see cref="StringEscapeHelper"/> is currently positioned within a string.
        /// </summary>
        internal bool IsString => IsStringValue;

        /// <summary>
        /// Jumps to the given index, ignoring all chars in between the current index and the new one.
        /// </summary>
        internal void JumpTo(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Checks for string delimiters at the given index.
        /// </summary>
        internal abstract void CheckStartAt(int startIndex);
        protected abstract void CheckNext();

        protected static bool HasStrings(string expression)
        {
            return expression.Contains(StringDelimiterSingle) || expression.Contains(StringDelimiterDouble);
        }

        /// <summary>
        /// Returns if an expression contains a value, disregarding content within string literals.
        /// </summary>
        internal static bool ContainsWithoutStrings(string expression, string value)
        {
            if (!expression.Contains(value))
            {
                return false;
            }
            else
            {
                if (!HasStrings(expression))
                {
                    return true;
                }
                else
                {
                    var index = 0;
                    var escaped = false;
                    var isString = false;
                    var startChar = StringDelimiterSingle;

                    var sb = new StringBuilder();

                    while (index < expression.Length)
                    {
                        var t = expression[index];

                        if (t == StringDelimiterSingle || t == StringDelimiterDouble)
                        {
                            if (!isString)
                            {
                                isString = true;
                                startChar = t;

                                // Append the starting delimiter:
                                sb.Append(t);
                            }
                            else
                            {
                                if (!escaped && t == startChar)
                                {
                                    isString = false;
                                }
                                else
                                {
                                    escaped = false;
                                }
                            }
                        }
                        else if (t == '\\')
                        {
                            escaped = !escaped;
                        }
                        else
                        {
                            if (escaped)
                                escaped = false;
                        }

                        if (!isString)
                            sb.Append(t);

                        index++;
                    }

                    return sb.ToString().Contains(value);
                }
            }
        }
    }
}
