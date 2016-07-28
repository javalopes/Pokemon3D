namespace Pokemon3D.Scripting.Types
{
    internal static class ObjectComparer
    {
        /// <summary>
        /// Compares two objects for equality, respecting their typings.
        /// </summary>
        /// <remarks>Used by the === and !== equality operators.</remarks>
        public static bool StrictEquals(ScriptProcessor processor, SObject left, SObject right)
        {
            left = SObject.Unbox(left);
            right = SObject.Unbox(right);

            if (left.TypeOf() != right.TypeOf())
            {
                return false;
            }
            // If they are undefined or null, return true:
            else if (left.TypeOf() == SObject.LITERAL_UNDEFINED || left.TypeOf() == SObject.LITERAL_NULL)
            {
                return true;
            }
            // Both are numbers:
            else if (left.TypeOf() == SObject.LITERAL_TYPE_NUMBER)
            {
                var numLeft = ((SNumber)left).Value;
                var numRight = ((SNumber)right).Value;

                return numLeft == numRight;
            }
            // Both are string:
            else if (left.TypeOf() == SObject.LITERAL_TYPE_STRING)
            {
                var strLeft = ((SString)left).Value;
                var strRight = ((SString)right).Value;

                return strLeft == strRight;
            }
            // Both are bool:
            else if (left.TypeOf() == SObject.LITERAL_TYPE_BOOL)
            {
                var boolLeft = ((SBool)left).Value;
                var boolRight = ((SBool)right).Value;

                return boolLeft == boolRight;
            }
            else
            {
                return ReferenceEquals(left, right);
            }
        }

        /// <summary>
        /// Compares two objects for equality, converting types if needed.
        /// </summary>
        /// <remarks>Used by the == and != equality operators.</remarks>
        public static bool LooseEquals(ScriptProcessor processor, SObject left, SObject right)
        {
            left = SObject.Unbox(left);
            right = SObject.Unbox(right);

            // both types are the same:
            if (left.TypeOf() == right.TypeOf())
            {
                // If they are undefined or null, return true:
                if (left.TypeOf() == SObject.LITERAL_UNDEFINED || left.TypeOf() == SObject.LITERAL_NULL)
                {
                    return true;
                }
                // Both are numbers:
                else if (left.TypeOf() == SObject.LITERAL_TYPE_NUMBER)
                {
                    var numLeft = ((SNumber)left).Value;
                    var numRight = ((SNumber)right).Value;

                    return numLeft == numRight;
                }
                // Both are string:
                else if (left.TypeOf() == SObject.LITERAL_TYPE_STRING)
                {
                    var strLeft = ((SString)left).Value;
                    var strRight = ((SString)right).Value;

                    return strLeft == strRight;
                }
                // Both are bool:
                else if (left.TypeOf() == SObject.LITERAL_TYPE_BOOL)
                {
                    var boolLeft = ((SBool)left).Value;
                    var boolRight = ((SBool)right).Value;

                    return boolLeft == boolRight;
                }
                else
                {
                    return ReferenceEquals(left, right);
                }
            }
            // null & undefined
            else if (left.TypeOf() == SObject.LITERAL_NULL && right.TypeOf() == SObject.LITERAL_UNDEFINED ||
                left.TypeOf() == SObject.LITERAL_UNDEFINED && right.TypeOf() == SObject.LITERAL_NULL)
            {
                return true;
            }
            // When one is a number and another is a string, convert the string to a number and compare:
            else if (left.TypeOf() == SObject.LITERAL_TYPE_STRING && right.TypeOf() == SObject.LITERAL_TYPE_NUMBER)
            {
                var numLeft = left.ToNumber(processor).Value;
                var numRight = ((SNumber)right).Value;

                return numLeft == numRight;
            }
            else if (left.TypeOf() == SObject.LITERAL_TYPE_NUMBER && right.TypeOf() == SObject.LITERAL_TYPE_STRING)
            {
                var numRight = right.ToNumber(processor).Value;
                var numLeft = ((SNumber)left).Value;

                return numLeft == numRight;
            }
            else if (left.TypeOf() == SObject.LITERAL_TYPE_BOOL)
            {
                return LooseEquals(processor, left.ToNumber(processor), right);
            }
            else if (right.TypeOf() == SObject.LITERAL_TYPE_BOOL)
            {
                return LooseEquals(processor, left, right.ToNumber(processor));
            }
            else
            {
                return false;
            }
        }
    }
}
