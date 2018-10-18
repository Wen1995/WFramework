using System;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Pattern;
using UnityEngine;

namespace PureMVC.Pattern
{
    public class Mediator : Notifier, IMediator
    {
        protected IFacade facade;
		protected string mMediatorName;
		protected object mViewComponent; 

        public string MediatorName { get {return mMediatorName;} }

        public object ViewComponent { get {return mViewComponent;} set {mViewComponent = value;} }

        public virtual IList<string> InterestsNotificationList
        {
            get
            {
                return new List<string>();
            }
        }

        public Mediator() : this("Mediator", null)
        {}

		public Mediator(string mediatorName)
		{
			mMediatorName = mediatorName;
            facade = Facade.Instance;
		}

		public Mediator(string mediatorName, object viewComponent)
		{
			mMediatorName = mediatorName;
			mViewComponent = viewComponent;
        }

        public virtual void OnRegister()
        {}

        public virtual void OnRemove()
        {}

        protected void RegisterNotification(string notificationName, Action<INotification> method)
        {
            facade.RegisterNotification(notificationName, method);
        }

        public virtual void HandleNotification(INotification notification)
        {}
    }
}

