using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utilities
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance_ = null;
        public static T instance
        {
            get
            {
                if (instance_ == null)
                {
                    Type t = typeof(T);
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance_ = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
                return instance_;
            }
        }
        public static bool hasInstance()
        {
            bool ret = true;
            if (instance_ == null)
            {
                ret = false;
            }
            return ret;
        }
        protected virtual void OnDestroy()
        {
            instance_ = null;
        }
    }
}