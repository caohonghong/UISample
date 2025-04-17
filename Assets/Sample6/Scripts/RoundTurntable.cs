using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Scripts.Util;
using Game.Logic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;


public class RoundTurntable : MonoBehaviour
{
    
    public GameObject itemPrefab; // 项目预制体
    public Transform parentTransform; // 父物体的Transform
    public Button btnOne_Button;
    public Button btnTen_Button;
    public Button btnSkip_Button;
    public RectTransform center_RectTransform;
    public Animation animation; // 动画组件
    public RectTransform imgAuto_RectTransform;
    public  int  totalNum = 10; // 项目数量
    private List<int> totalDatas;
    private List<RoundTurntableItem> itemsCtrls = new List<RoundTurntableItem>();
    private bool isSkipAni = false;
    void Start()
    {
#if UNITY_EDITOR
        CoRoutineExecutors.Create();
#endif
        InitData();
    }

    private void OnEnable()
    {
        btnOne_Button.onClick.AddListener(OnClickOne);
        btnTen_Button.onClick.AddListener(OnClickTen);
        btnSkip_Button.onClick.AddListener(OnClickSkip);
    }

    private void OnDisable()
    {
        btnOne_Button.onClick.RemoveListener(OnClickOne);
        btnTen_Button.onClick.RemoveListener(OnClickTen); 
        btnSkip_Button.onClick.RemoveListener(OnClickSkip);

    }
    
    void InitData()
    {
        GenerateItemsWithAngles(totalNum); 
        UpdateSkipState();
    }
    //测试数据
    List<int> RandomGetTargetPointsByCount(int count,int n)
    {
        // 初始化随机数生成器
        Random random = new Random();

        // 创建一个从0到n-1的序列
        List<int> numbers = Enumerable.Range(0, n).ToList();

        // 洗牌算法随机打乱列表
        for (int i = 0; i < n; i++)
        {
            int randomIndex = random.Next(i, n);  // 在i到n之间选择一个随机索引
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        // 选择前 count 个元素
        return numbers.Take(count).ToList();
    }
    void UpdateSkipState()
    {
        imgAuto_RectTransform.SetActive(isSkipAni);
    }

    #region Click
    void OnClickOne()
    {
        OnClickPlayAni(1);
    }
    void OnClickTen()
    {
        OnClickPlayAni(10);
    }
    void OnClickPlayAni(int count)
    {
        if (isSkipAni)
        {
            ShowReward();
        }
        else
        {
            totalDatas = RandomGetTargetPointsByCount(count, totalNum);
            ShowAnimation();
        }
    }
    private void OnClickSkip()
    {
        isSkipAni = !isSkipAni;
        UpdateSkipState();
    }
    

    #endregion
    #region 生成Items
     void GenerateItemsWithAngles(int n)
    {
        ClearItems();
        for (int i = 0; i < n; i++)
        {
            var obj = Instantiate(itemPrefab, parentTransform);
            float angle = GetItemAngle(n, i + 1);
            obj.transform.localEulerAngles = new Vector3(0, 0, angle);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.name = "item" + (i + 1);
            var itemCtrl= obj.GetComponent<RoundTurntableItem>();
            itemCtrl.InitData(i+1);
            itemsCtrls.Add(itemCtrl);
        }
        
    }
     void ClearItems()
    {
        foreach (var itemsCtrl in itemsCtrls)
        {
           Destroy(itemsCtrl.gameObject);
        }
        itemsCtrls.Clear();
    }
     float GetItemAngle(int n, int itemIndex)
    {
        // 每个项目的角度分配
        float anglePerItem = (float)360.0 / n;
        
        // 计算顺时针方向的具体项目位置
        return  (float)(360.0 - ((itemIndex - 1) * anglePerItem));
    }
    #endregion

      #region 动画

        private bool _stopRequested = false;
        private Coroutine _corutione;

        void StopAnimation()
        {
            _stopRequested = true;

            ShowReward();
        }

        void ShowAnimation()
        {
            _stopRequested = false;
            
            var targetPoints = GetTargetPoints(totalDatas);
            if (targetPoints.Count == 0)
            {
                ShowReward();
                return;
            }

            RunAnimationsSequentially(targetPoints);
        }

        private void RunAnimationsSequentially(List<int> targetPoints)
        {
            RunNextAnimation(targetPoints, 0);
        }

        void RunNextAnimation(List<int> targetPoints, int index)
        {
            if (_stopRequested)
            {
                return;
            }

            if (index < targetPoints.Count)
            {
                ShowOnceAnimation(targetPoints[index], () => RunNextAnimation(targetPoints, index + 1));
            }
            else
            {
                ShowReward();
            }
        }

        void ShowOnceAnimation(int targetPoint, Action callBack)
        {
            if (_stopRequested)
            {
                return;
            }

            var rotationZ = itemsCtrls[targetPoint].gameObject.transform.rotation
                .eulerAngles.z;
            center_RectTransform.rotation = Quaternion.Euler(0, 0, rotationZ);

            animation.Play();
            _corutione = CoRoutineExecutors.Run(WaitAnimationEnd(callBack));
        }

        private IEnumerator WaitAnimationEnd(Action callBack)
        {
            // 等待直到动画不再播放
            while (animation.isPlaying)
            {
                yield return null; // 等待一帧
            }
            callBack?.Invoke();
        }

        List<int> GetTargetPoints(List<int> totalData)
        {
            List<int> points = new List<int>();
            foreach (var position in totalData)
            {
                points.Add(position);
            }
            return points;
        }

        void ShowReward()
        {
            if (_corutione != null)
            {
                CoRoutineExecutors.Stop(_corutione);
            }
        }

    #endregion
    
}
