using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Pattern;
using Framework.Common;

namespace PureMVC.Core
{
    public class Controller : BaseSingleton<Controller>
    {
        protected static IController instance;
        protected IView view;
        protected readonly IDictionary<string, Func<ICommand>> commandMap;
        protected IObservers observers;

        /// <summary>
        /// Constructor of Controller
        /// </summary>
        protected Controller()
        {
            observers = Observers.Instance as IObservers;
            commandMap = new Dictionary<string, Func<ICommand>>();
        }

        /// <summary>
        /// Register a Command, using delegate to increate performance
        /// if is first time to register notification , create observer
        /// </summary>
        /// <param name="notificationName">name of notification</param>
        /// <param name="commandClassRef"></param>
        public void RegisterCommand(string notificationName, Func<ICommand> commandClassRef)
        {
            //if is fist time to register certain notification, register obsever
            if (commandMap.ContainsKey(notificationName))
            {
                return;
            }
            commandMap.Add(notificationName, commandClassRef);
        }

        public void ExcuteCommand(INotification notification)
        {
            string name = notification.Name;
            ICommand command = commandMap[name]();
            command.Excute(notification);
        }

        public void RemoveCommand(string notificationName)
        {
            if (commandMap.ContainsKey(notificationName))
            {
                Debug.LogWarning(string.Format("Notification[{0}] not exist", notificationName));
            }
            else
                commandMap.Remove(notificationName);
        }

        public bool HasCommand(string notificationName)
        {
            return commandMap.ContainsKey(notificationName);
        }

        public void RegisterObserver(string notificationName, IObserver observer)
        {
            observers.RegisterObserver(notificationName, observer);
        }

        public void RemoveObserver(string notificationName, Action<INotification> method)
        {
            observers.RemoveObserver(notificationName, method);
        }

        public void NotifyObserver(INotification notification)
        {
            observers.NotifyObserver(notification);
        }
    }
}

;