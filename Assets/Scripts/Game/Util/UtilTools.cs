using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
	public class UtilTools
	{
		/// <summary>
		/// 使Coroutine同步完成
		/// </summary>
		public static void SyncCoroutine(IEnumerator func)
		{
			while(func.MoveNext())
			{
				if(func.Current != null)
					SyncCoroutine((IEnumerator)func.Current);
			}
		}
	}
}

