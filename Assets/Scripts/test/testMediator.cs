using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Pattern;
using PureMVC.Interfaces;

public class testMediator : Mediator 
{

	
	public testMediator(string name) : base(name)
	{}

	public override void OnRegister()
	{
		base.OnRegister();
		SendNotification("eventTest");
	}

	public override IList<string> InterestsNotificationList
	{
		get
		{
			return new List<string>(new List<string>(new string[] {"eventTest"}));
		}
	}

	public override void HandleNotification(INotification notification)
	{
		Debug.Log(string.Format("Notification[{0}]", notification.Name));
	}


	public void Test()
	{
		testProxy proxy = Facade.Instance.RetriveProxy("proxy") as testProxy;
		var list = proxy.GetTestList();
		foreach(var ele in list)
		{
			Debug.Log(ele);
		}
	}
}
