using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Interfaces
{
	public interface IMediator
	{
		string MediatorName{get;}
		object ViewComponent{get;set;}
		IList<string> InterestsNotificationList {get;}
		void HandleNotification(INotification notification);
		void OnRegister();
		void OnRemove();
	}
}

