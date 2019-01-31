using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Common
{
	/// <summary>
	/// HotBytesLoader直接读取AssetBundle字节码
	/// 如果热更新目录存在目标资源，优先加载热更新目录的资源
	/// </summary>
	public class HotBytesLoader : AbstractResourceLoader
	{
		public byte[] Bytes
		{
			get;
			protected set;
		}
		public static HotBytesLoader Load(string bundleName, LoaderMode mode)
		{
			var loader = AutoNew<HotBytesLoader>(bundleName, null, false, mode);
			return loader;
		}

		protected override void Init(string bundlePath, params object[] args)
		{
			base.Init(bundlePath, args);
			LoaderMode mode = (LoaderMode)args[0];
            string fullBundlePath = string.Format(@"{0}/{1}/{2}",
                                                                ResourceLoaderSingleton.Instance.PlatformDir,
                                                                ResourceLoaderSingleton.Instance.Version,
                                                                bundlePath);
            ResourceLoaderSingleton.Instance.StartCoroutine(InitCo(fullBundlePath, mode));
		}

        protected IEnumerator InitCo(string url, LoaderMode mode)
        {
            //获取资源的真实路径
            string actualUrl;
            ResourcePathType type;
            ResourceLoaderSingleton.Instance.GetResoureActualPath(url, out actualUrl, out type);
            //如果资源路径无效，且允许运行时下载，尝试从服务端下载
            if (type == ResourcePathType.Invalid && ResourceLoaderSingleton.Instance.HotFixType == ResourceHotFixType.Runtime)
            {
                if(mode == LoaderMode.Sync)
                    ResourceLoaderSingleton.Instance.DownloadFile(url);
                else
                    yield return ResourceLoaderSingleton.Instance.DownloadFileAsync(url);
            }
            if (mode == LoaderMode.Sync)
            {
                Bytes = ResourceLoaderSingleton.Instance.ReadAllBytes(actualUrl);
            }
            else
            {
                //TODO 异步加载
                yield return null;
            }
            OnFinish(null);
        }
	}
}

