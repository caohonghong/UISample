using System;
using UnityEngine;

namespace Common.Scripts.Util
{
    public class OrderActionUnit
    {
        public int _order;
        public Action _destroyAction;

        public OrderActionUnit(Action action, int order, int index)
        {
            // 优先按order，其次入栈顺序
            if (order > 100000)
            {
                Debug.LogError($"OrderActionUnit order too large: {order}");
            }

            if (index > 1000)
            {
                Debug.LogError($"OrderActionUnit too many actions: {index}");
            }

            order = order * 1000 + index;

            _order = order;
            _destroyAction = action;
        }
    }
}