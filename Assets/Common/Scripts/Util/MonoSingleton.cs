using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.Util
{
    public static class MonoSingletonManager
    {
        // 如果做成列表，可有序控制
        // private static List<GameObject> s_allInstances = new List<GameObject>();
        const string kMonoSingletonDynamic = "__MonoSingletonDynamic";
        static List<OrderActionUnit> _destroyActions = new List<OrderActionUnit>();

        public static void DestroyAllInstance()
        {
            Debug.Log($"MonoSingletonManager.DestroyAllInstance");
            var singletonRoot = GameObject.Find(kMonoSingletonDynamic);
            if (singletonRoot != null)
            {
                try
                {
                    _destroyActions.Sort((u1, u2) => u1._order - u2._order);
                    // has sorted!
                    for (int i = _destroyActions.Count - 1; i >= 0; i--)
                    {
                        _destroyActions[i]._destroyAction();
                    }

                    GameObject.DestroyImmediate(singletonRoot);
                }
                catch (Exception e)
                {
                    // 这个有异常的话，其实就彻底完蛋了... 最好再细分颗粒度，挨个释放
                    Debug.LogError($"MonoSingletonManager.DestroyAllInstance {e}");
                }
            }

            _destroyActions.Clear();
        }

        public static void AddChild(GameObject o, Action destroyAction, int order)
        {
            // const string kMonoSingletonDynamic = "__MonoSingletonDynamic";
            var singletonRoot = GameObject.Find(kMonoSingletonDynamic);
            if (singletonRoot == null)
            {
                singletonRoot = new GameObject(kMonoSingletonDynamic);
                GameObject.DontDestroyOnLoad(singletonRoot);
            }

            o.transform.SetParent(singletonRoot.transform);

            var u = new OrderActionUnit(destroyAction, order, _destroyActions.Count);
            _destroyActions.Add(u);
        }
    }

/*
 * 1. 非线程安全
 * 2. 派生类必须调用base.Awake
 * 3. 支持场景中的单件，或者无中生有
 */
//@ref https://github.com/UnityCommunity/UnitySingleton/blob/master/Assets/Scripts/Singleton.cs
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T s_instance;

        // 决定最后的析构顺序，按 (order, index) 倒序析构
        // 派生类重载OnCreated设置
        protected int _orderInManager = 0;

        // if true, manager will control the instance, otherwise, subclass will control itself
        protected bool _controlByManager = true;

        // protected 

        public static bool HasInstance()
        {
            return s_instance != null;
        }

        static void ClearInstance()
        {
            if (s_instance != null)
            {
                DestroyImmediate(s_instance.gameObject);
            }

            s_instance = null;
        }

        // 多次调用安全
        public static T Create()
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<T>();
                if (s_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    s_instance = obj.AddComponent<T>();
                }
                // 顺序添加
                // s_allInstances.Add(s_instance.gameObject);
            }

            return s_instance;
        }

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();
                    if (s_instance == null)
                    {
                        Debug.LogError($"should create MonoSingleton.Instance {typeof(T).Name} first");
                        // yy 强制要求自行创建，规避被动的Instance歧义创建
                        // GameObject obj = new GameObject();
                        // obj.name = typeof(T).Name;
                        // s_instance = obj.AddComponent<T>();
                        return null;
                    }
                }

                return s_instance;
            }
        }

        private static void TrySetParentForNewSingleton(GameObject o, int orderInManager)
        {
            if (Application.isPlaying)
            {
                MonoSingletonManager.AddChild(o, ClearInstance, orderInManager);
            }
        }

        protected virtual void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as T;
                OnCreated();
                // 统一管理
                DontDestroyOnLoad(gameObject);
                if (_controlByManager)
                {
                    TrySetParentForNewSingleton(gameObject, this._orderInManager);
                }
            }
            else
            {
                if (_controlByManager)
                {
                    Debug.LogError($"repeat MonoSingleton.Awake {typeof(T).Name} {gameObject.name}");
                    Destroy(gameObject);
                }
            }
        }

        protected virtual void OnCreated()
        {
            // TODO by subclass, such as set order
            Debug.Log($"MonoSingleton.OnCreated {typeof(T).Name}");
        }

        // 避免别人不调用，，再简化
        protected virtual void OnDestroy()
        {
            Debug.Log($"MonoSingleton.OnDestroy {typeof(T).Name}");
        }
    }
}