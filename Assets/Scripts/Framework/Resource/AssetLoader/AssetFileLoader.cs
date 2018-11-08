using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.Common
{
    public enum LoaderMode
    {
        Sync,
        Async
    }

    public class AssetFileLoader : AbstractResourceLoader
    {
        private LoaderDelegate mLoaderCallback;
        public static AssetFileLoader Load(string url, LoaderDelegate callback, LoaderMode mode = LoaderMode.Sync)
        {
            var loader = AutoNew<AssetFileLoader>(url, callback, false, mode);
            return loader;
        }

        protected override void Init(string url, params object[] args)
        {
            base.Init(url, args);
            LoaderMode mode = (LoaderMode)args[0];
            //TODO
            ResourceLoaderSingleton.Instance.StartCoroutine(InitCo(url, mode));
        }

        IEnumerator InitCo(string url, LoaderMode mode)
        {
            Object resObj = null;
            if (!ResourceLoaderSingleton.Instance.RunBundle && Application.isEditor)
            {
#if UNITY_EDITOR
                resObj = AssetDatabase.LoadAssetAtPath(ResourceLoaderSingleton.Instance.EditorAssetsPath + url, typeof(Object));
#endif
                if(resObj == null)
                    Debug.Log(string.Format("url[{0}] cant find in ResourcesAssets", url));
            }
            else
            {
                //if not in editor, read from Rsources or AssetBundle
                if(!ResourceLoaderSingleton.Instance.RunBundle)
                {
                    //remove extension
                    url = url.Substring(0, url.Length - Path.GetExtension(url).Length);
                    //read from resources
                    resObj = Resources.Load(url);
                    if(resObj == null)
                        Debug.Log(string.Format("url[ {0}] cant find in Resources"));
                }
                else
                {
                    //load from bundle
                    var loader = AssetBundleLoader.Load(url, null, mode);
                    while (!loader.IsComplete)
                    {
                        yield return null;
                    }
                    AssetBundle bundle = loader.ResultObj as AssetBundle;
                    resObj = bundle.LoadAsset(url);
                }
            }
            if (resObj == null)
                Debug.Log(string.Format("AssetPath [{0}] is NULL or cannot load", url));
            

            OnFinish(resObj);
        }
    }
}

