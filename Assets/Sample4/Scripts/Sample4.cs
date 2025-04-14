using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Scripts.Util;
using UnityEngine;

public class Sample4 : MonoBehaviour
{
    public RectTransform Item_RectTransform;
    public UIScrollRectAdjustor ScrollView_UIScrollRectAdjustor;
    public RectTransform Content_RectTransform;
    
    private List<Sample4Item> _uiItemSubCtrls = new List<Sample4Item>();

    private int _toggleIndex = -1;
    void Start()
    {
#if UNITY_EDITOR
        CoRoutineExecutors.Create();
#endif
        RefreshBotttomData();
    }

    private void OnDestroy()
    {

    }

    private void ClearItems()
    {
        foreach (var itemSubCtrl in _uiItemSubCtrls)
        {
            Destroy(itemSubCtrl.gameObject);
        }

        _uiItemSubCtrls.Clear();
    }

    void RefreshBotttomData()
    {
        ClearItems();
        for (int i = 0; i < 20; i++)
        {
            var obj = Instantiate(Item_RectTransform, Content_RectTransform);
            transform.SetParent(Content_RectTransform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            var subCtrl = obj.GetComponent<Sample4Item>();
            subCtrl.InitData(i);
            subCtrl.button.onClick.AddListener(
                delegate { ClickToggle(subCtrl.index); }
            );
            _uiItemSubCtrls.Add(subCtrl);
        }
        if (_toggleIndex == -1)
        {
            _toggleIndex = _uiItemSubCtrls.First().index;
        }
        
        
        ScrollView_UIScrollRectAdjustor.Init(new Vector2Int(0, _uiItemSubCtrls.Count - 1),
            _uiItemSubCtrls.Count);
        RefreshToggle();
            
    }
    
    void ClickToggle(int index)
    {
        if (_toggleIndex == index)
        {
            return;
        }

        _toggleIndex = index;
        RefreshToggle(true);

      
    }
    
    private void RefreshToggle(bool isClick = false)
    {
        if (isClick)
        {
            ScrollView_UIScrollRectAdjustor._moveSpeed = 3000;
        }
        else
        {
            ScrollView_UIScrollRectAdjustor._moveSpeed = 900000;
        }
        ScrollView_UIScrollRectAdjustor.SetIndexToCenter(_toggleIndex, isClick);
        
        foreach (var itemSubCtrl in _uiItemSubCtrls)
        {
              itemSubCtrl.SetSelectState(itemSubCtrl.index == _toggleIndex);
        }
    }
    

    

}
