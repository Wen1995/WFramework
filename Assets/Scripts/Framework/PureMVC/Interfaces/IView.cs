using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Interfaces
{
    public interface IView
    {
        // Observer
        void RegisterObserver(string notificationName, IObserver observer);

        void RemoveObserver(string notificationName, Action<INotification> method);

        void NotifyObservers(INotification notification);
        // Mediator
        void RegisterMediator(IMediator mediator);

        IMediator RetrieveMediator(string mediatorName);

        IMediator RemoveMediator(string mediatorName);

        bool HasMediator(string mediatorName);
    }
}

