using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Core;
using PureMVC.Pattern;

public class test : MonoBehaviour
{
    IFacade facade;
    private void Start() 
    {
        facade = Facade.Instance;
        // IProxy proxy = new testProxy("proxy");
        // facade.RegisterProxy(proxy);
        // IMediator mediator = new testMediator("mediator");
        // facade.RegisterMediator(mediator);
        // testMediator tM = facade.RetrieveMediator("mediator") as testMediator;
        // tM.Test();
    }

    void Foo(INotification notification)
    {
        Debug.Log("Foo");
    }
}
