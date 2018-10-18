using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;

namespace PureMVC.Interfaces
{
	public interface IController
	{
		void RegisterCommand(string notificationName, Func<ICommand> commandClassRef);

		void ExcuteCommand(INotification notification);

		void RemoveCommand(string notificationName);

		bool HasCommand(string notificationName);

		void RegisterObserver(string notificationName, IObserver observer);

		void NotifyObserver(INotification notification);
	}
}

