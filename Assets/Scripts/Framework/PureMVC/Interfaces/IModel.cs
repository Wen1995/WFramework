using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;

namespace PureMVC.Interfaces
{
	public interface IModel
	{
		void RegisterProxy(IProxy proxy);
		IProxy RemoveProxy(string proxyName);
		IProxy RetriveProxy(string proxyName);
		bool HasProxy(string proxyName);
	}
}

