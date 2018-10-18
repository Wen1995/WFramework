using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Interfaces
{
    public interface IObserver
    {
        /// <summary>
        /// Callback method
        /// </summary>
        Action<INotification> Method { get;set; }

        void NotifyObserver(INotification notification);
        bool Compare(Action<INotification> method);
    }
}

