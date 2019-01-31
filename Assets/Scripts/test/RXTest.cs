using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using System.Net.Sockets;

public class RXTest : MonoBehaviour 
{
	public Button button1;
	public Button button2;

	StringReactiveProperty rxString = new StringReactiveProperty("test");
	Subject<string> subject;
	private void Start() 
	{		
		// var clickStream = Observable.EveryUpdate()
		// .Where(_ => Input.GetMouseButtonDown(0));
		// clickStream.Buffer(TimeSpan.FromSeconds(1), 2)
		// //.Where(xs => xs.Count >= 2)
		// .Subscribe((xs)=> Debug.Log(xs.Count));

		// Observable.FromCoroutine(coA)
		// .SelectMany(coB)
		// .Subscribe(_=>Debug.Log("done"));

		var clickStream1 = button1.onClick.AsObservable();
		clickStream1
		.Subscribe(_ => {MyAction();});
		var clickStream2 = button2.onClick.AsObservable();
		clickStream2.Zip(clickStream1, (a, b) => a)
		.Subscribe(_ => print("Zip Click"));

		Observable.Create<string>(
			observer => 
			{
				clickStream1.Subscribe(_ => observer.OnNext("click"));
				//observer.OnCompleted();
				return Disposable.Empty;
			}
		)
		.Subscribe(PrintStr);

		subject = new Subject<string>();
		subject
		.Throttle(TimeSpan.FromMilliseconds(500))
		.Subscribe(PrintStr).AddTo(this);

		subject.OnNext("1");
		print("end");
	}

	static bool Trigger()
	{
		return true;
	}

	void MyAction()
	{
		//rxString.SetValueAndForceNotify(UnityEngine.Random.Range(0, 10).ToString());
		subject.OnNext("click");
	}

	void MyRaction()
	{}
	IEnumerator<int> Foo1(int a)
	{
		yield return a + 1;
	}

	void PrintStr(string str)
	{
		print(str);
	}

	void PrintStrs(IList<string> strList)
	{
		foreach(var str in strList)
		{
			print(str);
		}
	}
}
