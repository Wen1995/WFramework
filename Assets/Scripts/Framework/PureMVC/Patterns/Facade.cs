using System;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Core;
using UnityEngine;
using WFramework.Common;

namespace PureMVC.Pattern
{
    public class Facade : BaseSingleton<Facade, IFacade>, IFacade
    {
        IController mController;
        IView mView;
        IModel mModel;
        public IController Controller
        {
            get{return mController;}
        }

        public IView View
        {
            get{return mView;}
        }

        public IModel Model
        {
            get{return mModel;}
        }

        public Facade()
        {
            mController = PureMVC.Core.Controller.Instance;
            mView = PureMVC.Core.View.Instance;
            mModel = PureMVC.Core.Model.Instance;
        }
        public void ExcuteCommand(INotification notification)
        {
            mController.ExcuteCommand(notification);
        }

        public bool HasCommand(string notificationName)
        {
            return mController.HasCommand(notificationName);
        }

        public bool HasMediator(string mediatorName)
        {
            return mView.HasMediator(mediatorName);
        }

        public bool HasProxy(string proxyName)
        {
            return mModel.HasProxy(proxyName);
        }

        public void RegisterCommand(string notificationName, Func<ICommand> commandClassRef)
        {
            mController.RegisterCommand(notificationName, commandClassRef);
        }

        public void RegisterMediator(IMediator mediator)
        {
            mView.RegisterMediator(mediator);
        }

        public void RegisterNotification(string notificationName, Action<INotification> method)
        {
            mView.RegisterObserver(notificationName, new Observer(method));
        }

        public void RegisterProxy(IProxy proxy)
        {
            mModel.RegisterProxy(proxy);
        }

        public void RemoveCommand(string notificationName)
        {
            mController.RemoveCommand(notificationName);
        }

        public IMediator RemoveMediator(string mediatorName)
        {
            return mView.RemoveMediator(mediatorName);
        }

        public void RemoveNotification(string notificationName, Action<INotification> method)
        {
            mView.RemoveObserver(notificationName, method);
        }

        public IProxy RemoveProxy(string proxyName)
        {
            return mModel.RemoveProxy(proxyName);
        }

        public IMediator RetrieveMediator(string mediatorName)
        {
            return mView.RetrieveMediator(mediatorName);
        }

        public IProxy RetriveProxy(string proxyName)
        {
            return mModel.RetriveProxy(proxyName);
        }

        public void SendNotification(INotification notification)
        {
            mView.NotifyObservers(notification);
        }
    }
}

