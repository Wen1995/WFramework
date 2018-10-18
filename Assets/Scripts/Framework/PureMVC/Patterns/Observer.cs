using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using IObserver = PureMVC.Interfaces.IObserver;


namespace PureMVC.Pattern
{
    public class Observer : IObserver
    {
        protected Action<INotification> mMethod;
        protected object mNotifyContex = null;

        public Observer(Action<INotification> method) : this(method, null)
        {
        }

        public Observer(Action<INotification> method, object notifyContext)
        {
            mMethod = method;
            mNotifyContex = notifyContext;
        }
        public Action<INotification> Method 
        { 
            get{return mMethod;}
            set{mMethod = value;}
        }

        public void NotifyObserver(INotification notification)
        {
            if(mMethod != null)
                mMethod(notification);
        }

        public bool Compare(Action<INotification> method)
        {
            return method == mMethod;
        }
    }
}

    