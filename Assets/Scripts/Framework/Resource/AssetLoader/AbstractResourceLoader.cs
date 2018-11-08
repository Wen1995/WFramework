using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Common
{
    public delegate void LoaderDelegate(bool isOK, Object resObj);

    public class AbstractResourceLoader
    {
        private static LoaderDelegate mLoaderCallback;
        protected readonly List<LoaderDelegate> mFinishCallback = new List<LoaderDelegate>();
        
        protected Object mResObject;
        public Object ResultObj
        {
            get{return mResObject;}
            protected set{mResObject = value;}
        }

        int mRefCount = 0;
        public int RefCount
        {
            get { return mRefCount; }
        }

        protected bool mIsComplete;
        public bool IsComplete
        {
            get{return mIsComplete;}
            protected set{mIsComplete = value;}
        }
        
        protected bool mIsSuccessed;
        public bool IsSuccessed
        {
            get{return mIsSuccessed;}
            protected set{mIsSuccessed = value;}
        }


        public static T AutoNew<T>(string url, LoaderDelegate callback, bool forceCreateNew = false, LoaderMode mode = LoaderMode.Sync)
            where T : AbstractResourceLoader, new()
        {
            if(callback != null)
                mLoaderCallback = callback;
            T loader = new T();
            loader.Init(url, mode);
            return loader;
        }

        protected virtual void Init(string url, params object[] args)
        {
            mIsComplete = false;
            mIsSuccessed = false;
        }

        protected virtual void OnFinish(Object res)
        {
            ResultObj = res;
            mIsComplete = true;
            //excute callback
        }

        protected void AddCallback(LoaderDelegate callback)
        {
            if(callback != null)
            {
                mFinishCallback.Add(callback);
            }
        }
    }
}

