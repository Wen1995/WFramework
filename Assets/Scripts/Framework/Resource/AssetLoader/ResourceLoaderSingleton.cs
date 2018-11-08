using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Common;
using Google.Protobuf.Client;
using UnityEngine.Networking;

/// <summary>
/// 资源加载统一接口对象
/// 自动判断加载方式与加载路径
/// 不论是同步加载还是异步加载，均返回追踪对象
/// 使用编辑器模式下，资源会从Assets/ResourcesAssets中读取
/// </summary>
namespace Framework.Common
{
    /// <summary>
    /// 加载资源统一接口
    /// PS. 资源路径需要完整的后缀名，否则在编辑器中无法加载资源
    /// </summary>
    public interface IResourceLoader
    {
        AbstractResourceLoader LoadBundle(string path, LoaderDelegate callback = null);
        AbstractResourceLoader LoadBundleAsync(string path, LoaderDelegate callback = null);
    }

    public class ResourceLoaderSingleton : MonoSingleton<ResourceLoaderSingleton>, IResourceLoader
    {
        public string EditorAssetsPath { get; private set; }
        //是否使用bundle
        public bool RunBundle { get; private set; }
        //是否使用本地Bundle
        public bool UseLocalBundle{ get; private set;}
        //对应平台
        public string PlatformDir{ get; private set;}
        //当前资源版本
        public string Version{ get; private set;}
        //path -> BundleInfo
        private Dictionary<string, ProtoBundleInfo> mBundleInfoMap = new Dictionary<string, ProtoBundleInfo>();

        public ResourceLoaderSingleton()
        {
            EditorAssetsPath = string.Format("{0}/{1}/", "Assets", "ResourcesAssets");
        #if UNITY_EDITOR && RUN_BUNDLE
            RunBundle = false;
        #else
            RunBundle = true;
        #endif
        #if UNITY_ANDROID
            PlatformDir = "Android";
        #endif
        #if UNITY_IOS
            PlatformDir = "IOS";
        #endif
        #if UNITY_EDITOR
            PlatformDir = "Windows";
        #endif
            Version = "1.0.0";
            UseLocalBundle = true;   
        }

        public void Init()
        {
            LoadProtoBundleInfos();
        }

        public AbstractResourceLoader LoadBundle(string path, LoaderDelegate callback = null)
        {
            var loader = AssetFileLoader.Load(path, callback, LoaderMode.Sync);
            return loader;
        }

        public AbstractResourceLoader LoadBundleAsync(string path, LoaderDelegate callback = null)
        {
            var loader = AssetFileLoader.Load(path, callback, LoaderMode.Async);
            return loader;
        }

        /// <summary>
        /// 同步加载字节码
        /// </summary>
        public byte[] ReadAllBytes(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SendWebRequest();
            while(!www.isDone);     //同步等待加载完成
            if(www.isNetworkError)
            {
                Debug.Log(www.error);
                return null;
            }
            else
            {
                return www.downloadHandler.data;
            }
        }

        public byte[] ReadAllBytesAsync(string url)
        {
            //TODO
            return new byte[1];
        }

        public string GetResourceServerBundleGUID(string url)
        {
            //TODO
            return url;
        }

        public string GetBundleNameByPath(string path)
        {
            if(!mBundleInfoMap.ContainsKey(path))
            {
                Debug.Log(string.Format("AssetBundle Path[{0}] not exist", path));
                return null;
            }
            return mBundleInfoMap[path].Bundlename;
        }

        //加载bundle索引文件
        private void LoadProtoBundleInfos()
        {
            ProtoBundleInfos bundleInfos = null;
            if(UseLocalBundle)
            {
                string url = string.Format(@"{0}/{1}/{2}/ProtoBundleInfos.dat", Application.streamingAssetsPath, PlatformDir, "1.0.0");
                bundleInfos = ProtoBundleInfos.Parser.ParseFrom(ReadAllBytes(url));
            }
            else
            {
                //从服务器获取最新的ProtoBundleInfos
                //TODO
            }

            if(bundleInfos == null)
                Debug.Log("Cannot load BundleInfos. Make sure you have finished AssetBundle build");
            else
            {
                var infoList = bundleInfos.BundleInfos;
                foreach(var item in infoList)
                {
                    //print(item.Path);
                    mBundleInfoMap.Add(item.Path, item);
                }
                    
                Debug.Log("Load BundleInfos successefully");
            }
        }

    }
}

