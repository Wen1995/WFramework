using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Common;
using PureMVC.Core;
using PureMVC.Pattern;
using System;

namespace PureMVC.Core
{
    public class View : BaseSingleton<View>
    {
        IObservers observers;
        private readonly IDictionary<string, IMediator> mediatorMap;

        protected View()
        {
            observers = Observers.Instance as IObservers;
            mediatorMap = new Dictionary<string, IMediator>();
        }

        public bool HasMediator(string mediatorName)
        {
            return mediatorMap.ContainsKey(mediatorName);
        }

        public void RegisterMediator(IMediator mediator)
        {
            
            if(HasMediator(mediator.MediatorName))
            {
                Debug.LogWarning(string.Format("Mediator[{0}] already registered", mediator.MediatorName));
                return;
            }
            //register observer
            IList<string> list = mediator.InterestsNotificationList;
            if(list.Count > 0)
            {
                IObserver observer = new Observer(mediator.HandleNotification);
                for(int i=0; i < list.Count; i++)
                    RegisterObserver(list[i], observer);
            }
            mediatorMap.Add(mediator.MediatorName, mediator);
            Debug.Log(string.Format("Register Mediator[{0}]", mediator.MediatorName));
            mediator.OnRegister();
        }

        public IMediator RemoveMediator(string mediatorName)
        {
            if(!HasMediator(mediatorName))
            {
                Debug.LogWarning(string.Format("Mediator[{0}] not exist"));
                return null;
            }
            IMediator mediator = mediatorMap[mediatorName];
            mediatorMap.Remove(mediatorName);
            mediator.OnRemove();
            //remove observer
            IList<string> list = mediator.InterestsNotificationList;
            if(list.Count > 0)
            {
                for(int i=0; i<list.Count; i++)
                    RemoveObserver(list[i], mediator.HandleNotification);
            }
            Debug.Log(string.Format("Remove Mediator[{0}]", mediator.MediatorName));
            return mediator;
        }

        public IMediator RetrieveMediator(string mediatorName)
        {
            if(!HasMediator(mediatorName))
            {
                Debug.LogWarning(string.Format("Mediator[{0}] not exist"));
                return null;
            }
            return mediatorMap[mediatorName];
        }

        public void RegisterObserver(string notificationName, IObserver observer)
        {
            observers.RegisterObserver(notificationName, observer);
        }

        public void NotifyObservers(INotification notification)
        {
            observers.NotifyObserver(notification);
        }

        public void RemoveObserver(string notificationName, Action<INotification> method)
        {
            observers.RemoveObserver(notificationName, method);
        }
    }
}

