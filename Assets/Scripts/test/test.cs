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
public class test : MonoBehaviour
{
    IFacade facade;
    IResourceLoader resLoader;
    public GameObject cube;

    private void Start() 
    {
        resLoader = ResourceLoaderSingleton.Instance;
        var loader = resLoader.LoadBundle("test.mat");
        var loader0 = resLoader.LoadBundle("image.png");
        if (loader.ResultObj != null)
        {
            cube.GetComponent<Renderer>().material = loader.ResultObj as Material;
        }
            
        else
            Debug.Log("asset is null");
    }

    public void OnClick()
    {
    }
}
