using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Common;
using PureMVC.Interfaces;
using PureMVC.Pattern;

namespace PureMVC.Core
{
    public class Model : BaseSingleton<Model>
    {
		protected readonly IDictionary<string, IProxy> proxyMap;

		public Model()
		{
			proxyMap = new Dictionary<string, IProxy>();
		}

        public bool HasProxy(string proxyName)
        {
			return proxyMap.ContainsKey(proxyName);
        }

        public void RegisterProxy(IProxy proxy)
        {
			if(HasProxy(proxy.ProxyName))
			{
				Debug.LogWarning(string.Format("proxyName[{0}] already registered", proxy.ProxyName));
				return;
			}
			proxyMap.Add(proxy.ProxyName, proxy);
			Debug.Log(string.Format("Register proxy[{0}]", proxy.ProxyName));
			proxy.OnRegister();
        }

        public IProxy RemoveProxy(string proxyName)
        {
			if(!HasProxy(proxyName))
			{
				Debug.LogWarning(string.Format("proxyName[{0}] not exist", proxyName));
				return null;
			}
			IProxy proxy = proxyMap[proxyName];
			proxyMap.Remove(proxyName);
			proxy.OnRemove();
			Debug.Log(string.Format("Remove proxy[{0}]", proxy.ProxyName));
			return proxy;
        }

        public IProxy RetriveProxy(string proxyName)
        {
			if(!HasProxy(proxyName))
			{
				Debug.LogWarning(string.Format("proxyName[{0}] not exist", proxyName));
				return null;
			}
			return proxyMap[proxyName];
        }
    }
}
