using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Logic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Sample7.Scripts
{
    public class UIRoulette: MonoBehaviour
    {
        public UIRouletteItem[] items;
        public Button btnOne_Button;
        public Button btnFive_Button;
        public Toggle _toggleSkip_Toggle;
        
        private int GridsCount = 10;

        private List<int> tempItemList = new List<int>();
        private long _curGridId;
        public AnimationCurve _levelAnimCurve;
        public int _loopCount = 3;
        public float _levelAnimDuration = 5f;
        public float _delayTime = 1.5f;
        private DG.Tweening.Tween _levelTween;
        private Coroutine _coroutine;
        private int _mulCount = 5;

        private int _startIndex = 1;

        private bool _isAni = false; //是否在播放动画
        private bool _IsClickGap = false;
        
        private List<UIRouletteItem> _itemSubCtrls = new List<UIRouletteItem>();
        private bool isSkip;
        private void Start()
        {
            InitData();
        }

        private void OnEnable()
        {
           btnOne_Button.onClick.AddListener(OnClickOne);
           btnFive_Button.onClick.AddListener(OnClickFive);
           _toggleSkip_Toggle.onValueChanged.AddListener(OnValueChange);
        }
        private void OnDisable()
        {
            btnOne_Button.onClick.RemoveListener(OnClickOne);
            btnFive_Button.onClick.RemoveListener(OnClickFive);
        }
        private void OnValueChange(bool value)
        {
            isSkip = value;
        }

        private void OnClickOne()
        {
            ClickInfo(1);
        }
        private void OnClickFive()
        {
            ClickInfo(5);
        }
        void ClickInfo(int count)
        {
            if (isSkip)
            {
                _isAni = false;
                ShowReward();
            }
            else
            {
                tempItemList = RandomGetTargetPointsByCount(count, GridsCount);
                _isAni = true;
                PlayNext();
            }      

        }
        void InitData()
        {
            for (int i = 1; i < items.Length+1; i++)
            {
                var item = items[i-1].GetComponent<UIRouletteItem>();
                item.InitData(i);
                _itemSubCtrls.Add(item);
            }
            isSkip = _toggleSkip_Toggle.isOn;
        }
        
        List<int> RandomGetTargetPointsByCount(int count,int n)
        {
            // 初始化随机数生成器
            Random random = new Random();

            // 创建一个从1到n的序列
            List<int> numbers = Enumerable.Range(1, n+1).ToList();

            // 洗牌算法随机打乱列表
            for (int i = 1; i < n+1; i++)
            {
                int randomIndex = random.Next(i, n);  // 在i到n之间选择一个随机索引
                int temp = numbers[i];
                numbers[i] = numbers[randomIndex];
                numbers[randomIndex] = temp;
            }

            // 选择前 count 个元素
            return numbers.Take(count).ToList();
        }
        
      private void PlayNext()
        {
            if (tempItemList.Count > 0)
            {
                _curGridId = tempItemList[0];
                tempItemList.RemoveAt(0);
                Debug.Log("PlayNext _curGridId:" + _curGridId);
                PlayMoveAni();
            }
        }


        void PlayMoveAni()
        {
            int tempIndex = 0;
            int endIndex = 0;
            int curIndex = 1;
            //得到最终的index
            foreach (var itemSubCtrl in _itemSubCtrls)
            {
                if (itemSubCtrl._gridId == _curGridId)
                {
                    tempIndex = itemSubCtrl._index;
                    endIndex = itemSubCtrl._index + (_loopCount - 1) * GridsCount;
                    break;
                }
            }

            int lastIndex = 0;

            _levelTween = DOTween.To(() => _startIndex,
                    tempNewValue => curIndex = tempNewValue, endIndex, _levelAnimDuration)
                .SetEase(_levelAnimCurve)
                .OnUpdate(() =>
                {
                    int loop = curIndex / GridsCount;
                    int index = curIndex - GridsCount * loop;
                    int selectIndex = index == 0 ?GridsCount : index;
                    foreach (var itemSubCtrl in _itemSubCtrls)
                    {
                        itemSubCtrl.SetSelectState(itemSubCtrl._index == selectIndex);
                    }

                    if (selectIndex != lastIndex)
                    {
                        lastIndex = selectIndex;
                        foreach (var itemSubCtrl in _itemSubCtrls)
                        {
                            if (itemSubCtrl._index == selectIndex)
                            {
                               // AudioController.Play("sfx_ui_turntable_02");
                                break;
                            }
                        }
                    }
                }).OnComplete(() =>
                {
                    _startIndex = tempIndex;
                    _levelTween = null;
                    if (_coroutine != null)
                    {
                        StopCoroutine(_coroutine);
                    }

                    _coroutine = StartCoroutine(DalyPopReward());
                });
        }
        public IEnumerator DalyPopReward()
        {
            yield return new WaitForSeconds(_delayTime);
            //展示奖励后延迟在播放下一个
            //此方法根据实际需求展示
            ShowReward(delegate
            {
                PlayNext();
            });
            
            _coroutine = null;
        }

        void ShowReward(Action callback = null)
        {
            Debug.Log("showReward");
            callback?.Invoke();
        }


    }
}