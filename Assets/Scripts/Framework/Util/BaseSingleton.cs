using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Common
{
	/// <summary>
	/// Base class of singleton, means to extract the singleton part of framework
	/// </summary>
	/// <typeparam name="T">Singleton class type 单例类</typeparam>
	public class BaseSingleton<T>
        where T : class
	{
        static T mInstance;
        static object mLock = new object();
		public static T Instance
		{
			get
			{
                lock (mLock)
                {
                    if (mInstance == null)
                        mInstance = System.Activator.CreateInstance(typeof(T), true) as T;
                    return mInstance;
                }
        	}
		}
	}
}

