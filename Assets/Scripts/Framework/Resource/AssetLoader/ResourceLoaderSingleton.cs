using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Common;
using Google.Protobuf.Client;
using UnityEngine.Networking;
using System.IO;
using Game.Utility;
using System;

/// <summary>
/// 资源加载统一接口对象
/// 自动判断加载方式与加载路径
/// 不论是同步加载还是异步加载，均返回追踪对象
/// 使用编辑器模式下，资源会从Assets/ResourcesAssets中读取
/// </summary>
namespace Framework.Common
{
    public enum ResourcePathType
    {
        /// <summary>
        /// 本地找不到，尝试从服务端下载
        /// </summary>
        Invalid,
        /// <summary>
        /// 在PersistentDataPath目录下
        /// </summary>
        InHot,
        /// <summary>
        /// 在SteamingAssets目录下
        /// </summary>
        InLocal,
    }

    public enum ResourceHotFixType
    {
        /// <summary>
        /// 运行时进行热更新
        /// </summary>
        Runtime,
        /// <summary>
        /// 提前更新所有资源
        /// </summary>
        PreDownload,
    }
    /// <summary>
    /// 加载资源统一接口
    /// PS. 资源路径需要完整的后缀名
    /// </summary>
    public interface IResourceLoader
    {
        AbstractResourceLoader LoadBundle(string path, LoaderDelegate callback = null);
        AbstractResourceLoader LoadBundleAsync(string path, LoaderDelegate callback = null);
    }

    public class ResourceLoaderSingleton : MonoSingleton<ResourceLoaderSingleton>, IResourceLoader
    {
        //设置------------------------------------------------------------
        /// <summary>
        ///是否使用bundle 
        /// </summary>
        public bool RunBundle { get; private set; }
        /// <summary>
        /// 是否使用本地Bundle
        /// </summary>
        public bool UseLocalBundle { get; private set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string CDNPath { get; private set; }
        /// <summary>
        /// 热更新模式
        /// </summary>
        public ResourceHotFixType HotFixType { get; private set; }
        //---------------------------------------------------------------- 
        /// <summary>
        /// 编辑器下资源存放路径
        /// </summary>
        public string EditorAssetsPath { get; private set; }
        /// <summary>
        /// 热更新目录前缀
        /// </summary>
        public string PersistentPathPreDir{ get; private set; }
        /// <summary>
        /// 本地资源目录前缀
        /// </summary>
        public string StreamingAssetsPathPreDir{ get; private set; }
        /// <summary>
        /// 对应平台
        /// </summary>
        public string PlatformDir{ get; private set;}
        /// <summary>
        /// 当前资源版本
        /// </summary>
        public string Version{ get; private set;}

        ///assetPath => BundleInfo
        private Dictionary<string, ProtoBundleInfo> mBundleInfoMap = new Dictionary<string, ProtoBundleInfo>();
        ///assetPath => BundleInfo
        private Dictionary<string, ProtoBundleInfo> mServerBundleInfoMap = new Dictionary<string, ProtoBundleInfo>();

        /// <summary>
        /// 初始化： 从服务器下载最新的bundleInfo和mainfest
        /// </summary>
        private void Awake()
        {
            Initialize();
            //加载 bundleInfo索引
            LoadProtoBundleInfos();
            //如果热更新类型为提前下载，下载所有新资源
            if (HotFixType == ResourceHotFixType.PreDownload)
                DownloadAllResource();
            //下载mainfest TODO
        }
        
        private void Initialize()
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
            UseLocalBundle = false;
            CDNPath = "127.0.0.1:8080";
            StreamingAssetsPathPreDir = string.Format("{0}/{1}/{2}/", Application.persistentDataPath, PlatformDir, Version);
            PersistentPathPreDir = string.Format("{0}/{1}/{2}/", Application.streamingAssetsPath, PlatformDir, Version);
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

        public string GetBundlePathByPath(string path)
        {
            if(!mBundleInfoMap.ContainsKey(path))
            {
                Debug.Log(string.Format("AssetBundle Path[{0}] not exist", path));
                return null;
            }
            return mBundleInfoMap[path].BundlePath;
        }

        public string GetBundlePathInServer(string bundleName)
        {
            return "";
        }

        /// <summary>
        /// 通过相对路径获得资源的真实路径
        /// </summary>
        public void GetResoureActualPath(string relativePath, out string actualPath, out ResourcePathType type)
        {
            //如果使用本地bundle,则无视热更新目录
            if (UseLocalBundle)
            {
                actualPath = string.Format(@"{0}/{1}", Application.streamingAssetsPath, relativePath);
                if (File.Exists(actualPath))
                    type = ResourcePathType.InLocal;
                else
                    type = ResourcePathType.Invalid;
                return;
            }
            //首先判断热更新目录是否存在
            string hotPath = string.Format(@"{0}/{1}", Application.persistentDataPath, relativePath);
            if (File.Exists(hotPath))
            {
                actualPath = hotPath;
                type = ResourcePathType.InHot;
                return;
            }
            //热更新目录不存在，判断本地资源是否存在
            string localPath = string.Format(@"{0}/{1}", Application.streamingAssetsPath, relativePath);
            if (File.Exists(localPath))
            {
                actualPath = localPath;
                type = ResourcePathType.InLocal;
                return;
            }
            //都不存在，返回Invalid和hotPath，因为可能之后会从服务端下载资源，存放在热更新目录
            actualPath = hotPath;
            type = ResourcePathType.Invalid;
        }


        /// <summary>
        /// 从服务器下载文件并保存在本地
        /// </summary>
        public void DownloadFile(string relativePath)
        {
            UtilTools.SyncCoroutine(DownloadFileAsync(relativePath));
        }
        public void DownloadFile(string[] relativePaths)
        {
            UtilTools.SyncCoroutine(DownloadFileAsync(relativePaths));
        }

        /// <summary>
        /// 从服务器下载文件并保存在本地(异步)
        /// </summary>
        public IEnumerator DownloadFileAsync(string relativePath)
        {
            yield return DownloadFileFromServer(relativePath);
        }
        /// <summary>
        /// 从服务器下载文件并保存在本地(异步)
        /// </summary>
        public IEnumerator DownloadFileAsync(string[] relativePaths)
        {
            yield return DownloadFileFromServer(relativePaths);
        }

        private IEnumerator DownloadFileFromServer(string relativePath)
        {
            UnityWebRequest request = UnityWebRequest.Get(string.Format(@"{0}/{1}", CDNPath, relativePath));
            request.SendWebRequest();
            while(!request.isDone)
                    yield return null;
            string savePath = string.Format(@"{0}/{1}", Application.persistentDataPath, relativePath);
            CreateFile(savePath, request.downloadHandler.data);
        }

        private IEnumerator DownloadFileFromServer(string[] relativePaths)
        {
            foreach (var path in relativePaths)
            {
                UnityWebRequest request = UnityWebRequest.Get(string.Format(@"{0}/{1}", CDNPath, path));
                request.SendWebRequest();
                while(!request.isDone)
                    yield return null;
                string savePath = string.Format(@"{0}/{1}", Application.persistentDataPath, path);
                CreateFile(savePath, request.downloadHandler.data);
            }
        }

        /// <summary>
        /// 从服务端下载到内存
        /// </summary>
        public void DownloadBytes(string relativePath, out byte[] btsData)
        {
            byte[] res = null;
            UtilTools.SyncCoroutine(DownloadBytesFromServer(relativePath, (data) => {
                res = data;
            }));
            btsData = res;
        }
        /// <summary>
        /// 从服务端下载到内存(异步)
        /// </summary>
        public IEnumerator DownloadBytesAsync(string relativePath, Action<byte[]> callback)
        {
            yield return StartCoroutine(DownloadBytesFromServer(relativePath, callback));
        }
        private IEnumerator DownloadBytesFromServer(string relativePath, Action<byte[]> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(string.Format(@"{0}/{1}", CDNPath, relativePath));
            request.SendWebRequest();
            while(!request.isDone)
                yield return null;
            Debug.Log(request.downloadHandler.data.Length);
            callback(request.downloadHandler.data);
        }

        /// <summary>
        /// 加载BundleInfo文件
        /// </summary>
        private void LoadProtoBundleInfos()
        {
            ProtoBundleInfos bundleInfos = null;
            ProtoBundleInfos serverBundleInfos = null;
            if(UseLocalBundle)
            {
                string url = string.Format(@"{0}/{1}", StreamingAssetsPathPreDir, UtilConstVal.ProtoBundleInfoRelativePath);
                bundleInfos = ProtoBundleInfos.Parser.ParseFrom(ReadAllBytes(url));
                serverBundleInfos = ProtoBundleInfos.Parser.ParseFrom(ReadAllBytes(url));
            }
            else
            {
                byte[] btsData = null;
                string url = string.Format(@"{0}/{1}", PersistentPathPreDir, UtilConstVal.ProtoBundleInfoRelativePath);
                btsData = ReadAllBytes(url);
                bundleInfos = ProtoBundleInfos.Parser.ParseFrom(btsData);
                //从服务器获取最新的ProtoBundleInfos
                DownloadBytes(string.Format(@"{0}/{1}/{2}", PlatformDir, Version, UtilConstVal.ProtoBundleInfoRelativePath), out btsData);
                serverBundleInfos = ProtoBundleInfos.Parser.ParseFrom(btsData);
            }

            if(bundleInfos == null)
                Debug.Log("Cannot load BundleInfos");
            else
            {
                var infoList = bundleInfos.BundleInfos;
                Debug.Log("local");
                foreach(var item in infoList)
                {
                    mBundleInfoMap.Add(item.AssetPath, item);
                    //Debug.Log(item.Bundlename);
                }
                Debug.Log("server");
                infoList = serverBundleInfos.BundleInfos;
                foreach (var item in infoList)
                {
                    //Debug.Log(item.Bundlename);
                    mServerBundleInfoMap.Add(item.AssetPath, item);
                }
                    
            }
        }


        /// <summary>
        /// 下载所有新资源
        /// </summary>
        private void DownloadAllResource()
        {
            foreach (var bundleInfo in mServerBundleInfoMap)
            {
                //TODO
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fullPath">完整路径</param>
        /// <param name="data">二进制数据</param>
        private void CreateFile(string fullPath, byte[] data)
        {
            FileInfo fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
            
            Stream stream = fileInfo.Create();
            stream.Write(data, 0, data.Length);
            stream.Close();
            stream.Dispose();
        }

        
        /// <summary>
        /// 比较版本
        /// </summary>
        private bool IsVersionUp(string versionA, string versionB)
        {
            string[] splitsA = versionA.Split('.');
            string[] splitsB = versionB.Split('.');
            if (splitsA.Length < 3 || splitsB.Length < 3)
            {
                Debug.Log("Asset version format wrong!! should be like 'x.x.x'");
                return true;
            }
            if (System.Convert.ToInt32(splitsA[0]) > System.Convert.ToInt32(splitsB[0]))
                return true;
            if (System.Convert.ToInt32(splitsA[1]) > System.Convert.ToInt32(splitsB[1]))
                return true;
            if (System.Convert.ToInt32(splitsA[2]) > System.Convert.ToInt32(splitsB[2]))
                return true;
            return false;
        }

    }
}

