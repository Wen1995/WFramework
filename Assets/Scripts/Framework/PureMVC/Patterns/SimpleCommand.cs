using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Interfaces
{
    public class SimpleCommand : ICommand
    {
		private readonly Action<INotification> mMethod;

		public SimpleCommand(Action<INotification> method)
		{
			mMethod = method;
		}
        public void Excute(INotification notification)
        {
			mMethod(notification);
        }
    }
}

