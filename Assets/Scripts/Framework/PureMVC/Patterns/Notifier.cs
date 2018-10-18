using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Core;

namespace PureMVC.Pattern
{
    public class Notifier : INotifier
    {
        public void SendNotification(string notificationName)
        {
			Facade.Instance.SendNotification(new Notification(notificationName));
        }

        public void SendNotification(string notificationName, object body)
        {
            Facade.Instance.SendNotification(new Notification(notificationName, body));
        }

        public void SendNotification(string notificationName, object body, string type)
        {
            Facade.Instance.SendNotification(new Notification(notificationName, body, type));
        }
    }
}

