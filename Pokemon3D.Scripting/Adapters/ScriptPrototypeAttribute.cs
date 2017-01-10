﻿using System;

namespace Pokemon3D.Scripting.Adapters
{
    /// <summary>
    /// An attribute to add to types to change their name in their script representations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ScriptPrototypeAttribute : ScriptMemberAttribute
    { }
}
