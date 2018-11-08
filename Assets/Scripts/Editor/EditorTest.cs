using UnityEditor;
using UnityEngine;

public class EditorTest : Editor {

    static string output_path = "/Resources/AssetBundle";

    [MenuItem("Project Tools/BundleBuild")]
    public static void BundleBuild()
    {
        output_path = Application.dataPath + output_path;
        BuildPipeline.BuildAssetBundles(output_path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

}
