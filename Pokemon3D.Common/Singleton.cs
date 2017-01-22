﻿using System;
using System.Reflection;
// ReSharper disable StaticMemberInGenericType

namespace Pokemon3D.Common
{
    /// <summary>
    /// A base class for a thread-safe singleton pattern implementation.
    /// The type that uses this class must not have a public constructor.
    /// </summary>
    public abstract class Singleton<T> where T : class
    {
        private static volatile T _instance;
        private static readonly object LockObject = new object();

        /// <summary>
        /// The instance of this singleton class.
        /// </summary>
        public static T Instance
        {
            get
            {
                EnsureInstanceExists();
                return _instance;
            }
        }

        private static void EnsureInstanceExists()
        {
            lock (LockObject)
            {
                if (_instance != null) return;
                var singletonType = typeof(T);

                //Ensure there are no public constructors...
                if (singletonType.GetConstructors(BindingFlags.Public).Length > 0)
                {
                    throw new InvalidOperationException(
                        $"'{singletonType.Name}' can be instanciated multiple time because of public constructor.");
                }

                _instance = (T)Activator.CreateInstance(singletonType, true);
            }
        }
    }
}
