using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;

namespace PureMVC.Pattern
{
	public class Notification : INotification
	{
		public string Name {get;set;}
		public object Body{get;set;}
		public string Type{get;set;}

		public Notification(string name, object body, string type)
		{
			this.Name = name;
			this.Body = body;
			this.Type = type;
		}

		public Notification(string name) : this(name, null, null)
		{
		}

		public Notification(string name, object body) : this(name, body, null)
		{}
	}
}

