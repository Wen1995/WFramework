using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Interfaces
{
    public interface IObservers
    {
        void NotifyObserver(INotification notification);
        void RegisterObserver(string notificationName, IObserver observer);
        void RemoveObserver(string notificationName, Action<INotification> method);
    }
}

