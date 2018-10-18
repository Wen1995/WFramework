using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFramework.Common
{
	/// <summary>
	/// Base class of singleton, means to extract the singleton part of framework
	/// </summary>
	/// <typeparam name="T">Singleton class type 单例类</typeparam>
	/// <typeparam name="I">Interface of singleton class 单例类的接口</typeparam>
	public class BaseSingleton<T, I> where T : BaseSingleton<T, I>
									 where I : class
	{
		private static I mInstance = null;
		
		public static I Instance
		{
			get
			{
				if(mInstance == null)
					mInstance = System.Activator.CreateInstance(typeof(T), true) as I;
				return mInstance;
        	}
		}
	}
}

