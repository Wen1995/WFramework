using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Pattern;
using PureMVC.Interfaces;
using System;

namespace PureMVC.Pattern
{
	public class MacroCommand : ICommand 
	{
		private Queue<Type> mSubCommands = new Queue<Type>();
		//private IList<ICommand> mSubCommands = new List<ICommand>();
		
		public MacroCommand()
		{
		}

		public void AddSubCommand(Type commandType)
		{
			mSubCommands.Enqueue(commandType);
		}
		public void Excute(INotification notification)
		{
			while(mSubCommands.Count > 0)
			{
				Type type = mSubCommands.Dequeue();
				object command = Activator.CreateInstance(type);
				if(command is ICommand)
					(command as ICommand).Excute(notification);
			}
		}
	}
}

