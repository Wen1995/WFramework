using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Interfaces
{
	public interface IFacade
	{
		IController Controller{get;}
		IView View{get;}
		IModel Model{get;}
		//command
		void RegisterCommand(string notificationName, Func<ICommand> commandClassRef);
		void ExcuteCommand(INotification notification);
		void RemoveCommand(string notificationName);
		bool HasCommand(string notificationName);
		//notifier
		void SendNotification(INotification notification);
		void RegisterNotification(string notificationName, Action<INotification> method);
		void RemoveNotification(string notificationName, Action<INotification> method);
		//mediator
		void RegisterMediator(IMediator mediator);
		IMediator RetrieveMediator(string mediatorName);
		IMediator RemoveMediator(string mediatorName);
		bool HasMediator(string mediatorName);
		//proxy
		void RegisterProxy(IProxy proxy);
		IProxy RemoveProxy(string proxyName);
		IProxy RetriveProxy(string proxyName);
		bool HasProxy(string proxyName);
		//handler
	}
}

