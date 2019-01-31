using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Google.Protobuf;
using Google.Protobuf.Client;

public class BundleDataInfo
{
    public string bundlePath;
    public string assetPath;
    public BundleDataInfo(string bundlePath, string assetPath)
    {
        this.bundlePath = bundlePath;
        this.assetPath = assetPath;
    }
}

public class AssetBundleBuild : Editor {

    static string resourcePath = Application.dataPath + @"/ResourcesAssets";
    static string bundleOutputPath = Application.dataPath + @"/StreamingAssets";
    static string version = "1.0.0";

    static Dictionary<string, BundleDataInfo> bundleDataMap = new Dictionary<string, BundleDataInfo>();
    static Dictionary<string, int> assetsRefCountMap = new Dictionary<string, int>();       //key : GUID, value : refCount
    static Dictionary<string, string> assetsPathMap = new Dictionary<string, string>();      //key : GUID, value : path

    public static void ClearAllCache()
    {
        bundleDataMap.Clear();
        assetsRefCountMap.Clear();
        assetsPathMap.Clear();
    }

    [MenuItem("Project Tools/Build Tools/AssetBundle/Simple Build")]
    public static void BuildSimpleBundle()
    {
        BuildPipeline.BuildAssetBundles(bundleOutputPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Project Tools/Build Tools/AssetBundle/Build Windows AssetBundle")]
    public static void BuildWindowsBundle()
    {
        ClearAllCache();
        //Store all file bundleinfo recursively
        GetDirectory(resourcePath);
        BuildAssetBundle(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Project Tools/Build Tools/AssetBundle/Build Android AssetBundle")]
    public static void BuildAndroidBundle()
    {
        //TODO
    }

    [MenuItem("Project Tools/Build Tools/AssetBundle/Build IOS AssetBundle")]
    public static void BuildIOSBundle()
    {
        //TODO
    }

    public static void GetDirectory(string path)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        if (folder != null)
        {
            //Recurse child folder
            foreach (var childFolder in folder.GetDirectories())
                GetDirectory(childFolder.FullName);

            //Set BundleInfo for every file
            foreach (var file in folder.GetFileSystemInfos())
            {
                if (file is DirectoryInfo       ||
                   file.Name.EndsWith(".meta")  ||
                   file.Name.EndsWith(".lua")   ||
                   file.Name.EndsWith(".json")  ||
                   file.Name.EndsWith(".cs"))
                    continue;
#if UNITY_EDITOR_WIN
                string assetPath = @"Assets" + file.FullName.Replace(Application.dataPath.Replace("/", "\\"), "").Replace("\\", "/");
#else
                string assetPath = @"Assets" + file.FullName.Replace(Application.dataPath, "");
#endif
                //set GUID as BundleName
                string GUID = AssetDatabase.AssetPathToGUID(assetPath);
                AssetImporter ai = AssetImporter.GetAtPath(assetPath);
                ai.assetBundleName = GUID;

                if (!bundleDataMap.ContainsKey(GUID))
                {
                    BundleDataInfo info = new BundleDataInfo(GUID, assetPath.Replace("Assets/ResourcesAssets/", ""));
                    bundleDataMap.Add(info.bundlePath, info);
                }

                //Get Dependency
                string[] dps = AssetDatabase.GetDependencies(assetPath);
                foreach(var dpPath in dps)
                {
                    if (dpPath == assetPath || dpPath.EndsWith(".cs"))
                        continue;
                    AddRefCount(AssetDatabase.AssetPathToGUID(dpPath), dpPath);
                }
            }
        }
    }

    //Add ref count and store path
    public static void AddRefCount(string GUID, string path)
    {
        if (!assetsRefCountMap.ContainsKey(GUID))
            assetsRefCountMap.Add(GUID, 1);
        else
            assetsRefCountMap[GUID]++;
        if(!assetsPathMap.ContainsKey(GUID))
            assetsPathMap.Add(GUID, path);
    }


    public static void BuildAssetBundle(BuildTarget target)
    {
        //set bundlename for asset which is refed more than once
        foreach (var pair in assetsRefCountMap)
        {
            if (pair.Value > 1)
            {
                string path = assetsPathMap[pair.Key];
                AssetImporter ai = AssetImporter.GetAtPath(path);
                ai.assetBundleName = pair.Key;
                if(!bundleDataMap.ContainsKey(pair.Key))
                {
                    BundleDataInfo info = new BundleDataInfo(pair.Key, path.Replace("Assets/ResourcesAssets/", ""));
                    bundleDataMap.Add(info.bundlePath, info);
                }
            }
        }
        AssetDatabase.RemoveUnusedAssetBundleNames();

        //set bundle options
        BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
        //set output path
        string outputPath = bundleOutputPath;
        if (target == BuildTarget.StandaloneWindows64)
            outputPath = bundleOutputPath + "/Windows/" + version;
        else if (target == BuildTarget.Android)
            outputPath = bundleOutputPath + "/Android/" + version;
        else if (target == BuildTarget.iOS)
            outputPath = bundleOutputPath + "/IOS/" + version;
        
        if(!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        BuildPipeline.BuildAssetBundles(outputPath, options, target);
        BuildBundleInfoProtoFile(outputPath);
        AssetDatabase.Refresh();
        Debug.Log("AssetBundle Create Finished");
    }

    /// <summary>
    /// 创建AssetBundle索引文件,使用protobuf序列化
    /// </summary>
    public static void BuildBundleInfoProtoFile(string outputPath)
    {
        //TODO
        outputPath = outputPath + "/ProtoBundleInfos.dat";
        ProtoBundleInfos protoBundleInfos = new ProtoBundleInfos();
        foreach(var pair in bundleDataMap)
        {
           BundleDataInfo info = pair.Value;
           protoBundleInfos.BundleInfos.Add(new ProtoBundleInfo{
               BundlePath = info.bundlePath,
               AssetPath = info.assetPath,
               Version = version,
           });
        }
        //Create File
        using(var output = File.Create(outputPath))
        {
           protoBundleInfos.WriteTo(output);
        }
    }
}
