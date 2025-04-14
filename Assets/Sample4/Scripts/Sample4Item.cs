using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sample4Item : MonoBehaviour
{
    public RectTransform nomalState_RectTransform;
    public RectTransform selectState_RectTransform;
    public Button button;

    public int index;
    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    
    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }
    private void OnClick()
    {
     
    }

    public void SetSelectState(bool isSelect)
    {
        nomalState_RectTransform.gameObject.SetActive(!isSelect);
        selectState_RectTransform.gameObject.SetActive(isSelect);
    }

    public void InitData(int i)
    {
        index = i;
    }
}
