using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.Util
{
    public static class GameObjExtensions
    {
        public static float GetLocalScaleX(this GameObject self)
        {
            return self.transform.localScale.x;
        }

        public static float GetLocalScaleY(this GameObject self)
        {
            return self.transform.localScale.y;
        }

        public static float GetLocalScaleZ(this GameObject self)
        {
            return self.transform.localScale.z;
        }

        public static float GetLocalPositionX(this GameObject self)
        {
            return self.transform.localPosition.x;
        }

        public static T GetComponentWithLog<T>(this GameObject self) where T : Component
        {
            return self.GetComponent<T>();
        }

        public static string GetPath(this GameObject go)
        {
            List<string> list = new List<string>();
            var t = go.transform;
            while (t != null)
            {
                list.Add(t.name);
                t = t.parent;
            }

            list.Reverse();
            return string.Join("/", list.ToArray());
        }

        public static Transform GetParent(this Component self)
        {
            return self.transform.parent;
        }

        public static Transform GetParent(this GameObject self)
        {
            return self.transform.parent;
        }

        public static void SetLocalPositionY(this GameObject self, float y)
        {
            self.transform.localPosition =
                new Vector3(self.transform.localPosition.x, y, self.transform.localPosition.z);
        }

        public static void SetLocalPosition(this GameObject self, float x, float y, float z)
        {
            self.transform.localPosition = new Vector3(x, y, z);
        }

        public static void SetLocalScale(this GameObject self, Vector3 v)
        {
            self.transform.localScale = v;
        }

        public static Vector3 GetPosition(this GameObject self)
        {
            return self.transform.position;
        }

        public static void SetLocalPositionX(this GameObject self, float x)
        {
            self.transform.localPosition =
                new Vector3(x, self.transform.localPosition.y, self.transform.localPosition.z);
        }

        public static GameObject FindGameObjectWithLog(this GameObject self, string name)
        {
            Transform transform = self.transform.Find(name);
            return ((transform == null) ? null : transform.gameObject);
        }

        public static T SafeGetComponent<T>(this GameObject self) where T : Component
        {
            T component = self.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            return self.AddComponent<T>();
        }

        public static Component SafeGetComponent(this GameObject self, System.Type type)
        {
            Component component = self.GetComponent(type);
            if (component != null)
            {
                return component;
            }

            return self.AddComponent(type);
        }

        public static GameObject FindDeepChild(this GameObject self, string name, bool includeInactive = false)
        {
            foreach (Transform transform in self.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (transform.name == name &&
                    transform != self.transform)
                {
                    return transform.gameObject;
                }
            }

            return null;
        }

        public static GameObject FindDeep(this GameObject self, string name, bool includeInactive = false)
        {
            foreach (Transform transform in self.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (transform.name == name)
                {
                    return transform.gameObject;
                }
            }

            return null;
        }

        public static GameObject FindChildOneDepth(this GameObject self, string name, bool includeInactive = false)
        {
            foreach (Transform transform in self.transform.GetComponentInChildren<Transform>(includeInactive))
            {
                if (transform.name == name)
                {
                    return transform.gameObject;
                }
            }

            return null;
        }

        public static List<GameObject> FindAllChild(this GameObject self, bool includeInactive = false)
        {
            List<GameObject> res = new List<GameObject>();
            foreach (Transform transform in self.transform.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (transform != self.transform && transform.parent == self.transform)
                {
                    res.Add(transform.gameObject);
                }
            }

            return res;
        }

        public static List<GameObject> FindAll2Child(this GameObject self, bool includeInactive = false)
        {
            List<GameObject> res = new List<GameObject>();
            foreach (Transform transform in self.transform.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (transform != self.transform && transform.parent.parent == self.transform)
                {
                    res.Add(transform.gameObject);
                }
            }

            return res;
        }

        public static List<GameObject> FindAllDeepByPattern(this GameObject self, IEnumerable<string> patterns,
            bool includeInactive = false)
        {
            List<GameObject> res = new List<GameObject>();
            foreach (Transform transform in self.GetComponentsInChildren<Transform>(includeInactive))
            {
                foreach (var pattern in patterns)
                {
                    if (transform.name.Contains(pattern))
                    {
                        res.Add(transform.gameObject);
                        break;
                    }
                }
            }

            return res;
        }

        public static List<GameObject> FindDeep(this GameObject self, HashSet<string> names,
            bool includeInactive = false)
        {
            List<GameObject> res = new List<GameObject>();
            foreach (Transform transform in self.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (names.Contains(transform.name))
                {
                    res.Add(transform.gameObject);
                }
            }

            return res;
        }

        public static GameObject FindDeepNoCase(this GameObject self, string name, bool includeInactive = false)
        {
            foreach (Transform transform in self.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (transform.name.ToLower() == name.ToLower())
                {
                    return transform.gameObject;
                }
            }

            return null;
        }

        public static GameObject FindDeepNoCaseContain(this GameObject self, string name, bool includeInactive = false)
        {
            foreach (Transform transform in self.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (transform.name.ToLower().Contains(name.ToLower()))
                {
                    return transform.gameObject;
                }
            }

            return null;
        }

        public static GameObject FindDeepWithLog(this GameObject self, string name, bool includeInactive = false)
        {
            return self.FindDeep(name, includeInactive);
        }

        public static void SetParent(this GameObject self, GameObject parent)
        {
            self.transform.parent = (parent == null) ? null : parent.transform;
        }

        public static void SetLocalPosition(this GameObject self, Vector3 v)
        {
            self.transform.localPosition = v;
        }

        public static void SetLocalEulerAngle(this GameObject self, Vector3 v)
        {
            self.transform.localEulerAngles = v;
        }

        public static Vector3 GetLocalPosition(this GameObject self)
        {
            return self.transform.localPosition;
        }

        public static void LookAt(this GameObject self, Vector3 worldPosition, Vector3 worldUp)
        {
            self.transform.LookAt(worldPosition, worldUp);
        }

        public static float GetPositionX(this GameObject self)
        {
            return self.transform.position.x;
        }

        public static float GetPositionY(this GameObject self)
        {
            return self.transform.position.y;
        }

        public static float GetPositionZ(this GameObject self)
        {
            return self.transform.position.z;
        }

        public static void SetEulerAngleX(this GameObject self, float x)
        {
            self.transform.eulerAngles = new Vector3(x, self.transform.eulerAngles.y, self.transform.eulerAngles.z);
        }

        public static void SetEulerAngleY(this GameObject self, float y)
        {
            self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, y, self.transform.eulerAngles.z);
        }

        public static void SetLocalEulerAngleY(this GameObject self, float y)
        {
            var localEuler = self.transform.localEulerAngles;
            self.transform.localEulerAngles = new Vector3(localEuler.x, y, localEuler.z);
        }

        public static void SetEulerAngleZ(this GameObject self, float z)
        {
            self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, self.transform.eulerAngles.y, z);
        }

        public static T GetComponentInChildrenWithLog<T>(this GameObject self) where T : Component
        {
            return self.GetComponentInChildren<T>();
        }

        public static void SafeSetParent(this GameObject self, Component parent)
        {
            self.SafeSetParent(parent.gameObject);
        }

        public static void SafeSetParent(this GameObject self, GameObject parent)
        {
            Transform transform = self.transform;
            Vector3 localPosition = transform.localPosition;
            Quaternion localRotation = transform.localRotation;
            Vector3 localScale = transform.localScale;
            transform.parent = parent.transform;
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
            transform.localScale = localScale;
            self.layer = parent.layer;
        }

        public static bool HasChild(this GameObject self)
        {
            return (0 < self.transform.childCount);
        }

        public static GameObject GetChild(this GameObject self, string name)
        {
            Transform tr = self.transform.Find(name);
            if (tr == null)
                return null;
            return tr.gameObject;
        }

        public static GameObject[] GetChildren(this GameObject self)
        {
            var children = new GameObject[self.transform.childCount];
            for (int i = 0; i < self.transform.childCount; i++)
            {
                children[i] = self.transform.GetChild(i).gameObject;
            }

            return children;
        }

        public static GameObject GetOrCreateChild(this GameObject self, string name)
        {
            GameObject go = null;
            if (self != null)
                go = self.FindDeep(name);
            else
                go = GameObject.Find(name);
            if (go == null)
            {
                go = new GameObject(name);
                go.transform.parent = self == null ? null : self.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
            }

            return go;
        }

        public static void AttachToParent(this GameObject self, GameObject parent, bool setLayer = true)
        {
            self.transform.parent = parent.transform;
            self.transform.localPosition = Vector3.zero;
            self.transform.localRotation = Quaternion.identity;
            self.transform.localScale = Vector3.one;
            if (setLayer)
            {
                self.layer = parent.layer;
            }
        }

        public static void Identity(this GameObject self)
        {
            self.transform.localPosition = Vector3.zero;
            self.transform.localRotation = Quaternion.identity;
            self.transform.localScale = Vector3.one;
        }

        public static bool IsIdentity(this GameObject self)
        {
            if (self.transform.localPosition != Vector3.zero)
                return false;
            if (self.transform.localEulerAngles != Vector3.zero)
                return false;
            if (self.transform.localScale != Vector3.one)
                return false;
            return true;
        }

        public static bool IsChildNameUnique(this GameObject self)
        {
            if (self.transform.childCount <= 0)
                return true;

            Dictionary<string, int> mapNames = new Dictionary<string, int>();
            for (int i = 0; i < self.transform.childCount; i++)
            {
                GameObject goChild = self.transform.GetChild(i).gameObject;
                if (mapNames.ContainsKey(goChild.name))
                    return false;
                mapNames.Add(goChild.name, i);
            }

            return true;
        }

        public static void SafeDestroy(this GameObject self)
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(self);
            }
            else
            {
                GameObject.DestroyImmediate(self);
            }
        }

        public static void RemoveAllChildrenGameObjects(this GameObject self)
        {
            GameObject rootGameObject = self;
            // remove all children
            int childCount = rootGameObject.transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                if (Application.isPlaying)
                {
                    GameObject.Destroy(rootGameObject.transform.GetChild(i).gameObject);
                }
                else
                {
                    GameObject.DestroyImmediate(rootGameObject.transform.GetChild(i).gameObject);
                }
            }
        }

        public static void SetLayer(this GameObject obj, int layer)
        {
            obj.layer = layer;
        }

        /*public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.SetLayer(layer);
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }*/
        

        public static void SetLayerByNameRecursively(this GameObject obj, string nameLayer)
        {
            int layer = LayerMask.NameToLayer(nameLayer);
            if (layer >= 0)
                obj.SetLayerRecursively(layer);
        }

        public static void SetLayerByName(this GameObject obj, string nameLayer)
        {
            int layer = LayerMask.NameToLayer(nameLayer);
            if (layer >= 0)
                obj.SetLayer(layer);
        }


        public static void RemoveAllComponentsInChildren<T>(this GameObject self) where T : Component
        {
            GameObject rootGameObject = self;
            var comps = rootGameObject.GetComponentsInChildren<T>(true);
            foreach (var comp in comps)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(comp);
                }
                else
                {
                    Object.DestroyImmediate(comp);
                }
            }
        }

        public static void RemoveComponent<T>(this GameObject self) where T : Component
        {
            GameObject rootGameObject = self;
            var comp = rootGameObject.GetComponent<T>();
            if (Application.isPlaying)
            {
                Object.Destroy(comp);
            }
            else
            {
                Object.DestroyImmediate(comp);
            }
        }

        public static Vector3 CalculateRendererCenterOffset(this GameObject self)
        {
            var allRenderers = new List<Renderer>();

            var meshRenderers = self.GetComponentsInChildren<MeshRenderer>();
            var skinnedMeshRenderers = self.GetComponentsInChildren<SkinnedMeshRenderer>();
            allRenderers.AddRange(meshRenderers);
            allRenderers.AddRange(skinnedMeshRenderers);
            if (allRenderers.Count == 0)
            {
                return Vector3.zero;
            }
            else
            {
                var bound = new Bounds(allRenderers[0].bounds.center, allRenderers[0].bounds.size);
                for (int i = 1; i != allRenderers.Count; ++i)
                {
                    bound.Encapsulate(allRenderers[i].bounds);
                }

                return bound.center - self.transform.position;
            }
        }

        /// <summary>
        /// 查找孩子
        ///     . 使用 广度优先 遍历算法
        ///     . 有一定性能开销, 为提高性能:
        ///         . 目标节点层次不要太深
        ///         . 尽量提供详细的路径名
        ///         . 查找多个节点时, 可先查找它们的公共父节点, 然后在该公共节点下查找
        ///     . id 格式
        ///         . "name"                    -- 单个名字
        ///         . "name/name/.../name"      -- 多个名字组合的路径, 越详细性能越好
        /// </summary>
        public static GameObject FindChild(this GameObject go, string id, bool check_visible = false,
            bool raise_error = true)
        {
            if (go == null)
            {
                if (raise_error) Debug.LogError("FindChild, go is null");
                return null;
            }

            var t = FindChild(go.transform, id, check_visible, raise_error);
            return t != null ? t.gameObject : null;
        }
        //public static GameObject FindChild(this UIBehaviour bhv, string id, bool check_visible = false, bool raise_error = true)
        //{
        //	return bhv.gameObject.FindChild(id, check_visible, raise_error);
        //}

        // 查找孩子
        public static T FindChild<T>(this GameObject go, string id, bool check_visible = false, bool raise_error = true)
            where T : Component
        {
            var t = FindChild(go.transform, id, check_visible, raise_error);
            return t != null ? t.GetComponent<T>() : null;
        }
        //public static T FindChild<T>(this UIBehaviour bhv, string id, bool check_visible = false, bool raise_error = true) where T : Component
        //{
        //	return bhv.gameObject.FindChild<T>(id, check_visible, raise_error);
        //}

        //判断节点的子节点中有无某个节点（只搜索一层）
        public static bool HaveChild(this GameObject go, string id)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).name == id)
                {
                    return true;
                }
            }

            return false;
        }

        //删除一个子节点（只搜索一层）
        public static void DelChild(this GameObject go, string id)
        {
            var trans = go.transform;
            for (int i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);
                if (child.name == id)
                {
                    GameObjectUtil.Destroy(child.gameObject);
                }
            }
        }
        
        //删除所有子节点
        public static void DelAllChild(this GameObject go)
        {
            var trans = go.transform;
            while (trans.childCount > 0)
            {
                var child = trans.GetChild(0);
                GameObjectUtil.Destroy(child.gameObject);
            }
        }

        // 根据 ID 查找
        static Transform FindChild(Transform t, string id, bool check_visible, bool raise_error)
        {
            if (string.IsNullOrEmpty(id)) return null;
            if (check_visible && !t.IsActive()) return null; // 不可见
            if (id == ".") return t; // 自己

            // 用 / 分割路径
            if (id.IndexOf('/') >= 0)
            {
                var arr = id.Split('/');
                foreach (var a in arr)
                {
                    t = FindChildDirect(t, a, check_visible);
                    if (t == null)
                    {
                        if (raise_error) Debug.LogError("FindChild failed, id:" + id);
                        break;
                    }
                }

                return t;
            }

            // 直接查找
            //var t2 = t.FindChild(id);
            //if (t2 != null) return t2;

            // 直接查找
            t = FindChildDirect(t, id, check_visible);
            if (t == null)
            {
                if (raise_error) Debug.LogError("FindChild failed, id: " + id);
            }

            return t;
        }

        static Transform FindChildDirect(Transform t, string id, bool check_visible)
        {
            UnityEngine.Profiling.Profiler.BeginSample("FindChildDirect");
            var queue = s_findchild_stack;
            queue.Enqueue(t);
            while (queue.Count > 0)
            {
                t = queue.Dequeue();
                var t2 = t.Find(id);
                if (t2 != null)
                {
                    if (!check_visible || t2.IsActive())
                    {
                        queue.Clear();
                        UnityEngine.Profiling.Profiler.EndSample();
                        return t2;
                    }
                }

                for (int i = 0, count = t.childCount; i < count; i++)
                {
                    t2 = t.GetChild(i);
                    if (!check_visible || t2.IsActive())
                    {
                        queue.Enqueue(t2);
                    }
                }
            }

            UnityEngine.Profiling.Profiler.EndSample();
            return null;
        }

        static Queue<Transform> s_findchild_stack = new Queue<Transform>();


        public static void SetActive(this GameObject go, string id, bool active, bool raise_error = true)
        {
            var go_obj = go.FindChild(id, false, raise_error);
            if (go_obj != null) go_obj.SetActiveIfNeed(active);
        }

        public static void SetChildsActive(this GameObject go, bool active)
        {
            go.transform.SetChildsActive(active);
        }

        public static void SetChildsActiveIfNeed(this GameObject go, bool active)
        {
            var t = go.transform;
            for (int i = 0; i < t.childCount; ++i)
            {
                t.GetChild(i).gameObject.SetActiveIfNeed(active);
            }
        }

        public static void SetActiveIfNeed(this GameObject go, bool active)
        {
            if (go.activeSelf != active) go.SetActive(active);
        }

        public static bool IsActive(this GameObject go)
        {
            return go.activeInHierarchy;
        }
    }

    public static class GameObjectUtil
    {
        public static void Destroy(Object obj)
        {
            if (Application.isEditor)
            {
                Object.DestroyImmediate(obj, true);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        public static GameObject FindOrCreate(string name)
        {
            var findResult = GameObject.Find(name);
            if (findResult == null)
            {
                return new GameObject(name);
            }
            else return findResult;
        }
    }
}