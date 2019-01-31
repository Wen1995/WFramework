using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Common
{
	public class AssetBundleLoader : AbstractResourceLoader 
	{
		public static AssetBundleLoader Load(string url, LoaderDelegate callback = null, LoaderMode mode = LoaderMode.Sync)
		{
			var loader = AutoNew<AssetBundleLoader>(url, callback, false, mode);
			//Create new Loader
			return loader;
		}

		protected override void Init(string url, params object[] args)
		{
			base.Init(url, args);
			LoaderMode mode = (LoaderMode)args[0];
			//transform url to bundlename
			string bundlePath = ResourceLoaderSingleton.Instance.GetBundlePathByPath(url);
			if(bundlePath == null)
			{
				OnError();
				return;
			}
			ResourceLoaderSingleton.Instance.StartCoroutine(LoadAssetBundle(bundlePath, mode));
		}

		private IEnumerator LoadAssetBundle(string bundlePath, LoaderMode mode)
		{
			//Load bytes via HotBytesLoader
			var bytesLoader = HotBytesLoader.Load(bundlePath, mode);
			while(!bytesLoader.IsComplete) yield return null;
			byte[] btsData = bytesLoader.Bytes;
			//Parse bytes, get assetbundle
			Object assetBundleObj = null;
			if(mode == LoaderMode.Sync)
				assetBundleObj = AssetBundle.LoadFromMemory(btsData);
			if(assetBundleObj != null)
			{
				OnFinish(assetBundleObj);
			}
		}

		private void OnError()
		{
			Debug.Log("Load AssetBundle Failed");
		}
	}
}

