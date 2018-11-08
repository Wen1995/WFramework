using PureMVC.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Common;

namespace PureMVC.Pattern
{
    /// <summary>
    /// Manage all observers
    /// </summary>
    public class Observers : BaseSingleton<Observers>
    {
        protected IDictionary<string, IList<IObserver>> observerMap;
        //avoid synchronic problems
        //protected readonly static object syncStaticFlag = new object();

        public Observers()
        {
            observerMap = new Dictionary<string, IList<IObserver>>();
        }

        public void NotifyObserver(INotification notification)
        {
            string name = notification.Name;
            if(!observerMap.ContainsKey(name)) return;
            var iList = observerMap[name];
            for(int i=0;i<iList.Count;i++)
                iList[i].NotifyObserver(notification);
        }

        public void RegisterObserver(string notificationName, IObserver observer)
        {
            if(!observerMap.ContainsKey(notificationName))
                observerMap[notificationName] = new List<IObserver>();
            var iList = observerMap[notificationName];
            iList.Add(observer);
        }
        
        public void RemoveObserver(string notificationName, Action<INotification> method)
        {
            if(!observerMap.ContainsKey(notificationName))
                return;
            var iList = observerMap[notificationName];
            for(int i=iList.Count-1; i>=0 ; i--)
                if(iList[i].Compare(method))
                    iList.RemoveAt(i);
        }
    }
}


