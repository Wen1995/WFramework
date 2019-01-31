using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Core;
using PureMVC.Pattern;
using Framework.Common;
using UnityEngine.UI;
using Google.Protobuf.Client;
using UnityEngine.Networking;
using System.Linq;

public class test : MonoBehaviour
{
    IFacade facade;
    IResourceLoader resLoader;
    public GameObject cube;

    private void Start() 
    {
        // resLoader = ResourceLoaderSingleton.Instance;
        // var loader = resLoader.LoadBundle("test.mat");
        // var loader0 = resLoader.LoadBundle("image.png");
        // if (loader.ResultObj != null)
        // {
        //     cube.GetComponent<Renderer>().material = loader.ResultObj as Material;
        // }
        // else
        //     Debug.Log("asset is null");
    }

    public void OnClick()
    {

    }

    IEnumerator TestCo(int i)
    {
        if(i > 0)
            yield return TestCo(i - 1);
        Debug.Log(i);
        yield return null;
    }

    public void WaitCoroutine(IEnumerator func)
    {
        while(func.MoveNext())
        {
            if(func.Current != null)
                WaitCoroutine((IEnumerator)func.Current);
        }
    }
}
