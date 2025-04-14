//----------携程执行器，用于执行携程，可以通过id来区分不同的携程执行器，可以通过id来停止携程

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.Util
{
    public class CoRoutineExecutors : MonoSingleton<CoRoutineExecutors>
    {
        #region 自定义倒计时

        public enum WaitForTimeType
        {
            OneSecond = 1,
            FiveSeconds = 5,
            QuarterMin = 15,
            HalfMin = 30,
            Min = 60,
            FiveMin = 300
        }

        private Dictionary<WaitForTimeType, WaitForSeconds> _minSeconds;
        private Dictionary<WaitForTimeType, Action> _timeFunc;

        #endregion

        private int _secondCount;
        private Action<int> _actionOnSecond;
        private Action _updateAction;
        private Action _fixedUpdateAction;
        private Action _lateUpdateAction;


        protected override void OnCreated()
        {
            base.OnCreated();

            _minSeconds = new Dictionary<WaitForTimeType, WaitForSeconds>();
            _timeFunc = new Dictionary<WaitForTimeType, Action>();
            Array waitFor = Enum.GetValues(typeof(WaitForTimeType));
            foreach (var waitItem in waitFor)
            {
                WaitForTimeType timeType = (WaitForTimeType)waitItem;
                _minSeconds.Add(timeType, new WaitForSeconds((int)waitItem));
                _timeFunc.Add(timeType, null);
                Run(DoMinTask(timeType));
            }


            _orderInManager = -10000;
            Run(DoSecondTask());
        }

        public void AddSecondTask(Action<int> task)
        {
            _actionOnSecond += task;
        }

        // 即使没有Add，Remove也不会报错，多次Remove也不会报错
        public void RemoveSecondTask(Action<int> task)
        {
            _actionOnSecond -= task;
        }

        public void AddTimeTask(WaitForTimeType waitForTimeType, Action task)
        {
            _timeFunc[waitForTimeType] += task;
        }

        public void RemoveTimeTask(WaitForTimeType waitForTimeType, Action task)
        {
            _timeFunc[waitForTimeType] -= task;
        }

        public void AddUpdateTask(Action task)
        {
            _updateAction += task;
        }

        public void RemoveUpdateTask(Action task)
        {
            _updateAction -= task;
        }

        public void AddFixedUpdateTask(Action task)
        {
            _fixedUpdateAction += task;
        }

        public void RemoveFixedUpdateTask(Action task)
        {
            _fixedUpdateAction -= task;
        }

        public void AddLateUpdateTask(Action task)
        {
            _lateUpdateAction += task;
        }

        public void RemoveLateUpdateTask(Action task)
        {
            _lateUpdateAction -= task;
        }

        private IEnumerator DoSecondTask()
        {
            _secondCount = 0;
            while (true)
            {
                _actionOnSecond?.Invoke(_secondCount);
                _secondCount++;
                yield return _minSeconds[WaitForTimeType.OneSecond];
            }
        }

        private IEnumerator DoMinTask(WaitForTimeType waitForTimeType)
        {
            while (true)
            {
                _timeFunc[waitForTimeType]?.Invoke();
                yield return _minSeconds[waitForTimeType];
            }
        }

        private static IEnumerator WaitForFrames(Action callback, int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null; // 等待一帧
            }

            callback?.Invoke();
        }

        private static IEnumerator WaitForSecs(Action callback, float secs)
        {
            yield return new WaitForSeconds(secs);
            callback?.Invoke();
        }

        /// <summary>
        /// 等待n帧执行回调
        /// </summary>
        /// <param name="callback">回调</param>
        /// <param name="frames">等待的帧数</param>
        public static void RunAfterFrames(Action callback, int frames)
        {
            if (callback == null || frames <= 0)
                return;

            Run(WaitForFrames(callback, frames));
        }

        /// <summary>
        /// 等待n秒执行回调
        /// </summary>
        /// <param name="callback">回调</param>
        /// <param name="secs">等待的秒数</param>
        public static Coroutine RunAfterSecs(Action callback, float secs)
        {
            if (callback == null || secs <= 0)
                return null;

            return Run(WaitForSecs(callback, secs));
        }

        public static Coroutine Run(IEnumerator routine)
        {
            if (routine == null)
                return null;
            if (!HasInstance())
            {
                Debug.LogError($"CoRoutineExecutors.Run failed, instance is null");
                return null;
            }

            return Instance.StartCoroutine(routine);
        }


        public static Coroutine Run(IEnumerator routine, System.Action callback)
        {
            if (routine == null)
                return null;
            if (!HasInstance())
            {
                Debug.LogError($"CoRoutineExecutors.Run failed, instance is null");
                return null;
            }

            return Instance.StartCoroutine(wrap(routine, callback));
        }


        public static void Stop(Coroutine coroutine)
        {
            if (!HasInstance())
            {
                Debug.LogError($"CoRoutineExecutors.Run failed, instance is null");
                return;
            }

            if (coroutine != null)
            {
                Instance.StopCoroutine(coroutine);
            }
        }

        public static void StopAll()
        {
            if (!HasInstance())
            {
                Debug.LogError($"CoRoutineExecutors.Run failed, instance is null");
                return;
            }

            Instance.StopAllCoroutines();
        }

        private static IEnumerator wrap(IEnumerator routine, System.Action callback)
        {
            yield return routine;
            if (callback != null)
            {
                callback();
            }
        }

        private void Update()
        {
            _updateAction?.Invoke();
        }

        private void LateUpdate()
        {
            _lateUpdateAction?.Invoke();
        }

        private void FixedUpdate()
        {
            _fixedUpdateAction?.Invoke();
        }
    }
}