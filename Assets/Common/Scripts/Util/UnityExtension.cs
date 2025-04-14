//----------------辅助代码一些可以方便使用的方法合集----------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Scripts.Util
{
    /// <summary>
    /// Unity 扩展。
    /// </summary>
    public static class UnityExtension
    {
        // default: himself & his direct parent
        public static T GetComponentInRecentParent<T>(this GameObject gameObject, int level = 2) where T : Component
        {
            if (gameObject == null)
            {
                return null;
            }

            Transform parent = gameObject.transform;
            for (int i = 0; i < level; i++)
            {
                if (parent == null)
                {
                    return null;
                }

                T component = parent.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }

                parent = parent.parent;
            }

            return null;
        }

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <typeparam name="T">要获取或增加的组件。</typeparam>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>获取或增加的组件。</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <param name="type">要获取或增加的组件类型。</param>
        /// <returns>获取或增加的组件。</returns>
        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);
            if (component == null)
            {
                component = gameObject.AddComponent(type);
            }

            return component;
        }

        /// <summary>
        /// 获取 GameObject 是否在场景中。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>GameObject 是否在场景中。</returns>
        /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
        public static bool InScene(this GameObject gameObject)
        {
            return gameObject.scene.name != null;
        }

        /// <summary>
        /// 递归设置游戏对象的层次。
        /// </summary>
        /// <param name="gameObject"><see cref="UnityEngine.GameObject" /> 对象。</param>
        /// <param name="layer">目标层次的编号。</param>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].gameObject.layer = layer;
            }
        }

        /// <summary>
        /// 将gameObject缩放为原来的大小
        /// </summary>
        /// <param name="gameObject"><see cref="UnityEngine.GameObject" /> 对象。</param>
        public static void ResetLocalScale(this GameObject gameObject)
        {
            gameObject.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 通过缩放将gameObject隐藏
        /// </summary>
        /// <param name="gameObject"><see cref="UnityEngine.GameObject" /> 对象。</param>
        public static void HideByLocalScale(this GameObject gameObject)
        {
            gameObject.transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// 取 <see cref="UnityEngine.Vector3" /> 的 (x, y, z) 转换为 <see cref="UnityEngine.Vector2" /> 的 (x, z)。
        /// </summary>
        /// <param name="vector3">要转换的 Vector3。</param>
        /// <returns>转换后的 Vector2。</returns>
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        /// <summary>
        /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 转换为 <see cref="UnityEngine.Vector3" /> 的 (x, 0, y)。
        /// </summary>
        /// <param name="vector2">要转换的 Vector2。</param>
        /// <returns>转换后的 Vector3。</returns>
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0f, vector2.y);
        }

        public static Vector3 ToXZ(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0f, vector3.z);
        }

        public static Vector3 ToXZ(this Vector3 vector3, float y)
        {
            return new Vector3(vector3.x, y, vector3.z);
        }

        /// <summary>
        /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 和给定参数 y 转换为 <see cref="UnityEngine.Vector3" /> 的 (x, 参数 y, y)。
        /// </summary>
        /// <param name="vector2">要转换的 Vector2。</param>
        /// <param name="y">Vector3 的 y 值。</param>
        /// <returns>转换后的 Vector3。</returns>
        public static Vector3 ToVector3(this Vector2 vector2, float y)
        {
            return new Vector3(vector2.x, y, vector2.y);
        }

        #region Transform

        /// <summary>
        /// 设置绝对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.x = newValue;
            transform.position = v;
        }

        /// <summary>
        /// 设置绝对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.y = newValue;
            transform.position = v;
        }

        /// <summary>
        /// 设置绝对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.z = newValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值增量。</param>
        public static void AddPositionX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.x += deltaValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值增量。</param>
        public static void AddPositionY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.y += deltaValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 坐标值增量。</param>
        public static void AddPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.z += deltaValue;
            transform.position = v;
        }

        /// <summary>
        /// 设置相对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetLocalPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.x = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetLocalPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.y = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetLocalPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.z = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值。</param>
        public static void AddLocalPositionX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.x += deltaValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值。</param>
        public static void AddLocalPositionY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.y += deltaValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 坐标值。</param>
        public static void AddLocalPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.z += deltaValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对尺寸的 x 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 分量值。</param>
        public static void SetLocalScaleX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.x = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 设置相对尺寸的 y 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 分量值。</param>
        public static void SetLocalScaleY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.y = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 设置相对尺寸的 z 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 分量值。</param>
        public static void SetLocalScaleZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.z = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 x 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 分量增量。</param>
        public static void AddLocalScaleX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.x += deltaValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 y 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 分量增量。</param>
        public static void AddLocalScaleY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.y += deltaValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 z 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 分量增量。</param>
        public static void AddLocalScaleZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.z += deltaValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 二维空间下使 <see cref="UnityEngine.Transform" /> 指向指向目标点的算法，使用世界坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="lookAtPoint2D">要朝向的二维坐标点。</param>
        /// <remarks>假定其 forward 向量为 <see cref="UnityEngine.Vector3.up" />。</remarks>
        public static void LookAt2D(this Transform transform, Vector2 lookAtPoint2D)
        {
            Vector3 vector = lookAtPoint2D.ToVector3() - transform.position;
            vector.y = 0f;

            if (vector.magnitude > 0f)
            {
                transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
            }
        }

        public static void ResetTransform(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void SetParentAndResetTransform(this Transform transform, Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static Vector3 GetXZ(this Transform transform)
        {
            var position = transform.position;
            return new Vector3(position.x, 0, position.z);
        }

        public static Vector3 GetXZ(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0, vector3.z);
        }

        public static void ExecuteAll(this Transform transform, Action<Transform> action, Func<Transform, bool> check)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTran = transform.GetChild(i);
                if (check(childTran))
                {
                    action(childTran);
                    ExecuteAll(childTran, action, check);
                }
                else
                {
                    action(childTran);
                }
            }
        }

        public static void ExecuteAllType1(this Transform transform, Action<Transform> action,
            Func<Transform, bool> check)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTran = transform.GetChild(i);
                if (check(childTran))
                {
                    action(childTran);
                }

                ExecuteAllType1(childTran, action, check);
            }
        }

        public static T[] GetChildComponents<T>(this Component component) where T : UnityEngine.Object
        {
            if (component != null && component.transform != null)
            {
                List<T> _result = new List<T>();
                int childCount = component.transform.childCount;
                for (int offset = 0; offset < childCount; offset++)
                {
                    if (typeof(T) == typeof(GameObject))
                    {
                        _result.Add(component.transform.GetChild(offset).gameObject as T);
                    }
                    else if (typeof(T) == typeof(Transform))
                    {
                        _result.Add(component.transform.GetChild(offset) as T);
                    }
                    else
                    {
                        T comp = component.transform.GetChild(offset)?.GetComponent<T>();
                        if (comp != null)
                        {
                            _result.Add(comp);
                        }
                    }
                }

                return _result.ToArray();
            }
            else
            {
                // 处理component为null或没有transform的情况
                return new T[0];
            }
        }

        public static void SetActive(this Component component, bool active)
        {
            if (component.gameObject.activeSelf != active)
            {
                component.gameObject.SetActive(active);
            }
        }

        public static void SetActiveByScale(this Component component, bool active)
        {
            if (active)
            {
                component.gameObject.ResetLocalScale();
            }
            else
            {
                component.gameObject.HideByLocalScale();
            }
        }

        //递归查找子节点
        public static Transform DeepFindChild(this Transform root, string childName)
        {
            Transform result = null;
            result = root.Find(childName);
            if (result == null)
            {
                foreach (Transform transform in root)
                {
                    result = DeepFindChild(transform, childName);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        public static Transform FindTransform(this Transform root, string path)
        {
            var startIndex = 0;
            var index = path.IndexOf('/');
            if (index == 0)
            {
                startIndex = 1;
                index = path.IndexOf('/', startIndex);
            }

            if (index == -1)
                return startIndex == 0
                    ? GameObject.Find(path)?.transform
                    : GameObject.Find(path.Substring(1))?.transform;
            if (index > 0)
            {
                var rootPath = path.Substring(startIndex, index);
                var rootTransform = GameObject.Find(rootPath)?.transform;
                return rootTransform == null ? null : rootTransform.Find(path.Substring(index + 1));
            }

            return null;
        }

        public static Transform FindOrCreate(this Transform parent, string name)
        {
            var trans = parent.Find(name);
            if (trans != null) return trans;
            trans = new GameObject(name).transform;
            trans.SetParent(parent, false);

            return trans;
        }

        #endregion Transform

        /*public static void ToGray(this Image image)
        {
            var grayMaterial = Addressables.LoadAssetAsync<Material>("UIGray.mat");
            grayMaterial.WaitForCompletion();
            image.material = grayMaterial.Result;
            image.SetMaterialDirty();
        }

        public static void ToGray(this RawImage image)
        {
            var grayMaterial = Addressables.LoadAssetAsync<Material>("UIGray.mat");
            grayMaterial.WaitForCompletion();
            image.material = grayMaterial.Result;
            image.SetMaterialDirty();
        }*/

        public static void ToColored(this Image image)
        {
            image.material = null;
        }

        public static void ToColored(this RawImage image)
        {
            image.material = null;
        }


        /*public static void ToGray(this Button button, bool includeChild = false)
        {
            button.image?.ToGray();
            if (includeChild)
            {
                var list = button.GetComponentsInChildren<Image>();
                foreach (var img in list)
                {
                    img.ToGray();
                }
            }
        }

        public static void ToColored(this Button button, bool includeChild = false)
        {
            button.image?.ToColored();
            if (includeChild)
            {
                var list = button.GetComponentsInChildren<Image>();
                foreach (var img in list)
                {
                    img.ToColored();
                }
            }
        }*/

        /// <summary>
        /// 关闭按钮交互
        /// </summary>
        /// <param name="button">按钮组件</param>
        /// <param name="muteAnimation">是否关闭点击动画（默认有轻微放大缩小效果的那个）</param>
        /// <param name="includeChild">子节点的图片是否也一起变灰</param>
        /*
        public static void ToMute(this Button button, bool muteAnimation = false, bool includeChild = true)
        {
            if (!button.interactable) return;
            ColorBlock colors = button.colors;
            // 之后要用shader置灰所以这里需要保证为白色，防止interactable设为false的时候灰色叠加
            colors.disabledColor = new Color(1f, 1f, 1f, 1f);
            button.colors = colors;
            button.ToGray(includeChild);
            button.interactable = false;
            if (muteAnimation && button.image != null)
            {
                button.image.raycastTarget = false;
            }

            button.enabled = false;
        }

        public static void ToInteractable(this Button button, bool includeChild = true)
        {
            if (button.interactable) return;
            button.enabled = true;
            button.ToColored(includeChild);
            if (button.image != null)
            {
                button.image.raycastTarget = true;
            }

            button.interactable = true;
        }
        */

        /// <summary>
        /// 按钮先mute后隔指定时间后重新激活（基于DoTween）
        /// </summary>
        /// <param name="button"></param>
        /// <param name="interval"></param>
        /// <param name="muteAnimation"></param>
        /// <param name="includeChild">子节点图片是否跟着一起变</param>
        /*
        public static void ToMuteThenInteractable(this Button button, float interval, bool muteAnimation = false,
            bool includeChild = true)
        {
            if (button == null) return;

            button.ToMute(includeChild);
            DOTween.Sequence()
                .Append(DOTween.To(() => interval, x => interval = x, 0f, interval))
                .InsertCallback(interval - 1f, () =>
                {
                    if (button != null && button.gameObject != null)
                        button.ToInteractable(includeChild);
                })
                .SetLink(button.gameObject); // 当游戏对象被销毁时，自动kill该tween
        }
        */



        // find recent interface in ancestor by affinity
        public static T FindRecentInterfaceInAncestor<T>(this Transform child) where T : class
        {
            Transform current = child.parent;
            while (current != null)
            {
                var t = current.GetComponent<T>();
                if (t != null)
                {
                    return t;
                }

                current = current.parent;
            }

            return null;
        }

        /// <summary>
        /// string not null or empty
        /// </summary>
        public static bool NotNullOrEmpty(this string str) => !string.IsNullOrEmpty(str);

        /// <summary>
        /// Is array null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this T[] collection) => collection == null || collection.Length == 0;

        /// <summary>
        /// Is list null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IList<T> collection) => collection == null || collection.Count == 0;

        /// <summary>
        /// Is collection null or empty. IEnumerable is relatively slow. Use Array or List implementation if possible
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();

        /// <summary>
        /// Collection is not null or empty
        /// </summary>
        public static bool NotNullOrEmpty<T>(this T[] collection) => !collection.IsNullOrEmpty();

        /// <summary>
        /// Collection is not null or empty
        /// </summary>
        public static bool NotNullOrEmpty<T>(this IList<T> collection) => !collection.IsNullOrEmpty();

        /// <summary>
        /// Collection is not null or empty
        /// </summary>
        public static bool NotNullOrEmpty<T>(this IEnumerable<T> collection) => !collection.IsNullOrEmpty();

        public static string FormatFileSize(this long bytes)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes = bytes / 1024;
            }

            return String.Format("{0:0.##} {1}", bytes, sizes[order]);
        }

        public static IEnumerator PlayAnimation(this Animation anim, string animName, Action action)
        {
            var clip = anim.GetClip(animName);
            if (clip == null)
            {
                yield break;
            }

            anim.Play(animName);
            yield return new WaitForSeconds(clip.length);
            action?.Invoke();
        }


        public static IEnumerator WaitToAction(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }
    }
}