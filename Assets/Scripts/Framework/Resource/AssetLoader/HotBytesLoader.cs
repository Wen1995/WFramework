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

		protected override void Init(string bundleName, params object[] args)
		{
			base.Init(bundleName, args);
			LoaderMode mode = (LoaderMode)args[0];

            //判断版本

            string url = string.Format("{0}/{1}/{2}/{3}", 
			Application.streamingAssetsPath, 
			ResourceLoaderSingleton.Instance.PlatformDir, 
			"1.0.0", bundleName);
			if(mode == LoaderMode.Sync)
			{
				Bytes = ResourceLoaderSingleton.Instance.ReadAllBytes(url);
			}
			else
			{
				//TODO 异步加载
			}

			OnFinish(null);
		}

		// private IEnumerator LoadBytes(string url, LoaderMode mode)
		// {
			
		// }
	}
}

