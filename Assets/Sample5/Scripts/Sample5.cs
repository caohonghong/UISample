using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Scripts.Util;
using Game.Logic;
using UnityEngine;

public class Sample5 : MonoBehaviour
{
  
    public TimeSelector TimeSelector_TimeSelector;

    private void Start()
    {
        long cdEndTimeStamp = 1698231180; // 示例，以实际例子替换;
        TimeSelector_TimeSelector.InitTimeSelector(cdEndTimeStamp);

    }
}
