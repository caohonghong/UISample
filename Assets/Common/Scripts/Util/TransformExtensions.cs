using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.Util
{
    public static class TransformExtensions
    {
        public static Transform[] GetAllChild(this Transform This)
        {
            return This.GetAllChild(true);
        }

        public static Transform[] GetAllChild(this Transform This, bool includeInactive)
        {
            IEnumerable componentsInChildren;
            List<Transform> list = new List<Transform>();
            if (includeInactive)
            {
                componentsInChildren = This.GetComponentsInChildren<Transform>(true);
            }
            else
            {
                componentsInChildren = This;
            }

            IEnumerator enumerator = componentsInChildren.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    if (current.parent == This)
                    {
                        list.Add(current);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            return list.ToArray();
        }

        public static List<string> GetAllChildNames(this Transform self)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i != self.childCount; ++i)
            {
                ret.Add(self.GetChild(i).name);
            }

            return ret;
        }

        //不包含子节点
        public static List<T> GetChildrenOfType<T>(this Transform self) where T : Component
        {
            List<T> ret = new List<T>();
            for (int i = 0; i != self.childCount; ++i)
            {
                var com = self.GetChild(i).GetComponent<T>();
                if (com != null)
                {
                    ret.Add(com);
                }
            }

            return ret;
        }


        //删除所有的子节点
        public static void ClearChildren(this Transform self)
        {
            if (self != null)
            {
                int count = self.childCount;
                for (int i = count - 1; i >= 0; i--)
                {
                    GameObject goChild = self.GetChild(i).gameObject;
#if UNITY_EDITOR
                    UnityEngine.Object.DestroyImmediate(goChild);
#else
                    UnityEngine.Object.Destroy(goChild);
#endif
                }
            }
        }

        /// <summary>
        /// Gets the path from root.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="self">Self.</param>
        public static string GetFullPath(this Transform self)
        {
            string path = self.gameObject.name;

            Transform parent = self.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }

        // public static void ResetTransform(this Transform self)
        // {
        //     self.localPosition = Vector3.zero;
        //     self.localRotation = Quaternion.identity;
        //     self.localScale = Vector3.one;
        // }


        public static void CopyTransformSRT(this Transform self, Transform other)
        {
            self.localPosition = other.localPosition;
            self.localRotation = other.localRotation;
            self.localScale = other.localScale;
        }

        public static void CopyTransform(this Transform self, Transform other)
        {
            self.transform.parent = other.parent;
            self.position = other.position;
            self.rotation = other.rotation;
            self.localScale = other.localScale;
            self.transform.parent = null;
        }

        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            if (aParent == null) return null;

            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static bool IsActive(this Transform t)
        {
            return t.gameObject.activeInHierarchy;
        }

        public static void SetChildsActive(this Transform t, bool active)
        {
            for (int i = 0; i < t.childCount; ++i)
            {
                t.GetChild(i).gameObject.SetActive(active);
            }
        }

        public static void SetGlobalScale(this Transform t, Vector3 worldScale)
        {
            // Calculate the local scale needed to achieve the desired global scale
            Vector3 parentScale = t.parent ? t.parent.lossyScale : Vector3.one;
            t.localScale = new Vector3(
                worldScale.x / parentScale.x,
                worldScale.y / parentScale.y,
                worldScale.z / parentScale.z
            );
        }
    }
}