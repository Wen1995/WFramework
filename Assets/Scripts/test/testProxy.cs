using System.Collections;
using System.Collections.Generic;
using PureMVC.Pattern;
using UnityEngine;

public class testProxy : Proxy 
{
	List<int> testList = null;
	public testProxy(string name) : base(name, null)
	{}

	public override void OnRegister()
	{
		base.OnRegister();
		testList = new List<int>();
		testList.Add(1);
		testList.Add(2);
	}

	public List<int> GetTestList()
	{
		return testList;
	}
}
