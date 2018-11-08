using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Common
{
    /// <summary>
    /// Sington class that need to 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T mInstance = null;
        static object mLock = new object();

        public static T Instance
        {   
            get
            {
                lock (mLock)
                {
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError(string.Format("Singleton class [{0}] is more than 1!!", typeof(T).Name));
                        return mInstance;
                    }
                    if (mInstance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        mInstance = go.AddComponent<T>();
                        mInstance.SendMessage("Init", null, SendMessageOptions.DontRequireReceiver);
                        DontDestroyOnLoad(go);
                    }
                    return mInstance;
                }
            }
        }
    }
}

