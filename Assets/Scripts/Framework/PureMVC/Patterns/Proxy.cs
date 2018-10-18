using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Pattern;

namespace PureMVC.Pattern
{
    public class Proxy : Notifier, IProxy
    {
		protected string mProxyName;
		protected object mData;

        public Proxy() : this("Proxy", null)
        {}
		public Proxy(string proxyName) : this(proxyName, null)
		{
			mProxyName = proxyName;
		}

		public Proxy(string proxyName, object data)
		{
			mProxyName = proxyName;
			mData = data;
		}

        public string ProxyName
        {
            get
            {
                return mProxyName;
            }
        }

        public object Data 
		{ 
			get{return mData;}
			set{mData = value;}
		}

        public virtual void OnRegister()
        {}

        public virtual void OnRemove()
        {}
    }
}
