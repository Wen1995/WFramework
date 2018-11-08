using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoadBundle : MonoBehaviour {

    string bundlePath = "/Resources/AssetBundle";
    private MeshRenderer renderer;
    AssetBundle bundle;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void Load()
    {
        bundle = AssetBundle.LoadFromFile(Application.dataPath + bundlePath + "/testbundle");
        Material mat = bundle.LoadAsset<Material>("image");
        renderer.material = mat;
    }

    public void UnLoad()
    {
        bundle.Unload(true);
    }
}
