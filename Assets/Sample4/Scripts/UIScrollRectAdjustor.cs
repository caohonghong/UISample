using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Common.Scripts.Util;


[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(Graphic))]
public class UIScrollRectAdjustor : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    private ScrollRect _scrollRect;

    private ScrollRect ScrollRect
    {
        get
        {
            if (_scrollRect == null)
            {
                _scrollRect = GetComponent<ScrollRect>();
            }

            return _scrollRect;
        }
    }

    private RectTransform ContentRectTrans => ScrollRect.content;
    private RectTransform ParentTransform => _scrollRect.GetParent().GetComponent<RectTransform>();
    private GridLayoutGroup _gridLayoutGroup;

    private GridLayoutGroup GridLayoutGroup
    {
        get
        {
            if (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = ContentRectTrans.GetComponent<GridLayoutGroup>();
            }

            return _gridLayoutGroup;
        }
    }

    [SerializeField, Header("自动移动时的移动速度")] public float _moveSpeed = 3000;
    private Coroutine _autoMoveCoroutine;
    private Vector2Int _validIndexRange;
    private float ContentPosX => ContentRectTrans.anchoredPosition.x;
    private float[] _posXs;
    private float MinPosX => _posXs[MinPosXIndex];
    private float MaxPosX => _posXs[MaxPosXIndex];
    private int MinPosXIndex => _validIndexRange.y;
    private int MaxPosXIndex => _validIndexRange.x;

    // index:当前哪一列在中央。
    public event Action<int> MoveEndEvent;
    private int _columnCnt;

    private Coroutine _coroutine;

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            CoRoutineExecutors.Stop(_coroutine);
            _coroutine = null;
        }
    }

    private bool IsValidIndex(int index)
    {
        return index >= _validIndexRange.x && index <= _validIndexRange.y;
    }


    public void Init(Vector2Int validIndexRange, int columnCnt = 0)
    {
        // _columnCnt = columnCnt > 0 ? columnCnt : CalcCurColumnCnt();
        // CreatePosXs(_columnCnt);
        // _validIndexRange =
        //     new Vector2Int(Mathf.Max(0, validIndexRange.x), Mathf.Min(_posXs.Length - 1, validIndexRange.y));
        Init(validIndexRange.x, validIndexRange.y, columnCnt);
    }

    public void Init(int min, int max, int columnCnt = 0)
    {
        _columnCnt = columnCnt > 0 ? columnCnt : CalcCurColumnCnt();
        CreatePosXs(_columnCnt);
        _validIndexRange =
            new Vector2Int(Mathf.Max(0, min), Mathf.Min(_posXs.Length - 1, max));
    }

    public void Destroy()
    {
        StopAutoMove();
        _posXs = null;
        _scrollRect = null;
        _gridLayoutGroup = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopAutoMove();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float curX = ContentPosX;
        float suitableX = GetSuitableX(curX, _scrollRect.velocity.x < 0, out int index);
        MoveToTargetX(suitableX, () => MoveEndEvent?.Invoke(index));
    }

    private float GetSuitableX(float curX, bool isLeftMove, out int index)
    {
        float targetX = 0;
        index = 0;
        if (curX < MinPosX)
        {
            targetX = MinPosX;
            index = MinPosXIndex;
        }
        else if (curX > MaxPosX)
        {
            targetX = MaxPosX;
            index = MaxPosXIndex;
        }
        else
        {
            if (isLeftMove)
            {
                targetX = MinPosX;
                index = MinPosXIndex;
                for (int i = MaxPosXIndex; i <= MinPosXIndex; i++)
                {
                    if (_posXs[i] <= curX)
                    {
                        targetX = _posXs[i];
                        index = i;
                        break;
                    }
                }
            }
            else
            {
                targetX = MaxPosX;
                index = MaxPosXIndex;
                for (int i = MinPosXIndex; i >= MaxPosXIndex; i--)
                {
                    if (_posXs[i] >= curX)
                    {
                        targetX = _posXs[i];
                        index = i;
                        break;
                    }
                }
            }
        }

        return targetX;
    }

    private void MoveToTargetX(float x, Action moveEndAction)
    {
        StartAutoMove(ContentPosX, x, moveEndAction);
    }

    private void StartAutoMove(float beginX, float endX, Action moveEndAction)
    {
        StopAutoMove();


        _autoMoveCoroutine = CoRoutineExecutors.Run(AutoMoveCoroutine(beginX, endX, moveEndAction));
    }

    private void StopAutoMove()
    {
        CoRoutineExecutors.Stop(_autoMoveCoroutine);
    }

    private IEnumerator AutoMoveCoroutine(float beginX, float endX, Action moveEndAction)
    {
        var offset = _scrollRect.viewport.rect.width / 2;
        var leftPos = ContentRectTrans.rect.width * ContentRectTrans.pivot.x;
        var maxX = leftPos - offset;
        var minX = leftPos - ContentRectTrans.rect.width + offset;
        endX = Mathf.Clamp(endX, minX, maxX);

        float timer = 0f;
        float moveTime = Mathf.Abs(beginX - endX) / _moveSpeed;
        while (timer < moveTime)
        {
            ContentRectTrans.anchoredPosition = new Vector3(Mathf.Lerp(beginX, endX, timer / moveTime),
                ContentRectTrans.anchoredPosition.y);
            timer += Time.deltaTime;
            yield return null;
        }

        ContentRectTrans.anchoredPosition = new Vector3(endX, ContentRectTrans.anchoredPosition.y);
        ScrollRect.StopMovement();
        moveEndAction?.Invoke();
    }

    private void AutoMoveImmediately(float endX)
    {
        var offset = _scrollRect.viewport.rect.width / 2;
        var leftPos = ContentRectTrans.rect.width * ContentRectTrans.pivot.x;
        var maxX = leftPos - offset;
        var minX = maxX - ContentRectTrans.rect.width + offset;
        endX = Mathf.Clamp(endX, minX, maxX);
        ContentRectTrans.anchoredPosition = new Vector3(endX, ContentRectTrans.anchoredPosition.y);
        ScrollRect.StopMovement();
    }


    // 让第index个子节点显示到中央
    public void SetIndexToCenter(int columnIndex, bool isClick = false)
    {
        if (IsValidIndex(columnIndex))
        {
            if (_coroutine != null)
            {
                CoRoutineExecutors.Stop(_coroutine);
                _coroutine = null;
            }

            _coroutine = CoRoutineExecutors.Run(AdjustViewportWidth(columnIndex, isClick));
        }
    }

    public void SetIndexToCenterImmediately(int columnIndex)
    {
        if (!IsValidIndex(columnIndex)) return;
        if (_coroutine != null)
        {
            CoRoutineExecutors.Stop(_coroutine);
            _coroutine = null;
        }

        float contentWidth = ContentRectTrans.rect.width;
        float viewPortWidth = ScrollRect.GetParent().GetComponent<RectTransform>().rect.width;
        this.GetComponent<Image>().raycastTarget = false;

        float? pos = null;
        if (columnIndex != 0 && columnIndex != _columnCnt - 1)
        {
            pos = _posXs[columnIndex];
        }
        else if (columnIndex == 0)
        {
            pos = _posXs[columnIndex] - GridLayoutGroup.cellSize.x + GridLayoutGroup.spacing.x;
        }
        else if (columnIndex == _columnCnt - 1)
        {
            pos = _posXs[columnIndex] + GridLayoutGroup.cellSize.x - GridLayoutGroup.spacing.x;
        }

        if (pos != null)
        {
            AutoMoveImmediately(pos.Value);
            MoveEndEvent?.Invoke(columnIndex);
        }
    }

    private int CalcCurColumnCnt()
    {
        return (int)((ContentRectTrans.sizeDelta.x + GridLayoutGroup.spacing.x) /
                     (GridLayoutGroup.cellSize.x + GridLayoutGroup.spacing.x));
    }

    // 计算相关数据：各列居中时对应的Content的LocalPosX
    private void CreatePosXs(int cnt, bool isforceSet = true)
    {
        var width = isforceSet
            ? (GridLayoutGroup.cellSize.x * cnt + GridLayoutGroup.spacing.x * (cnt - 1))
            : ContentRectTrans.sizeDelta.x;
        float firstX = width / 2 - GridLayoutGroup.cellSize.x / 2;
        float distance = GridLayoutGroup.cellSize.x + GridLayoutGroup.spacing.x;
        _posXs = new float[cnt];
        for (int i = 0; i < cnt; i++)
        {
            _posXs[i] = firstX - i * distance;
        }
    }

    private void AdjustSpacing(int itemCount)
    {
        if (itemCount <= 0) return;

        float containerWidth = ContentRectTrans.rect.width;
        float itemWidth = _gridLayoutGroup.cellSize.x;

        // 计算初始间距
        float totalItemWidth = itemWidth * itemCount;
        float remainingSpace = containerWidth - totalItemWidth;
        float spacing = remainingSpace / (itemCount - 1);

        // 如果项数为偶数，稍微增加间距使其视觉居中
        if (itemCount % 2 == 0)
        {
            float adjustmentFactor = itemWidth / itemCount;
            spacing += adjustmentFactor;
        }

        // 设置新的间距到 GridLayoutGroup
        _gridLayoutGroup.spacing = new Vector2(spacing, _gridLayoutGroup.spacing.y);

        Debug.Log("Adjusted Spacing: " + spacing);
    }

    private IEnumerator AdjustViewportWidth(int columnIndex, bool isClick)
    {
        yield return new WaitForEndOfFrame();
        float contentWidth = ContentRectTrans.rect.width;
        float viewPortWidth = ScrollRect.GetParent().GetComponent<RectTransform>().rect.width;
        this.GetComponent<Graphic>().raycastTarget = false;
        if (isClick)
        {
            if (columnIndex != 0 && columnIndex != _columnCnt - 1)
            {
                var pos = _posXs[columnIndex];
                MoveToTargetX(pos, () => MoveEndEvent?.Invoke(columnIndex));
            }
            else if (columnIndex == 0)
            {
                var pos = _posXs[columnIndex] - GridLayoutGroup.cellSize.x + GridLayoutGroup.spacing.x;
                MoveToTargetX(pos, () => MoveEndEvent?.Invoke(columnIndex));
            }
            else if (columnIndex == _columnCnt - 1)
            {
                var pos = _posXs[columnIndex] + GridLayoutGroup.cellSize.x - GridLayoutGroup.spacing.x;
                MoveToTargetX(pos, () => MoveEndEvent?.Invoke(columnIndex));
            }
        }
        else if (contentWidth > viewPortWidth)
        {
            var pos = _posXs[columnIndex];
            MoveToTargetX(pos, () => MoveEndEvent?.Invoke(columnIndex));
            this.GetComponent<Graphic>().raycastTarget = true;
        }
    }
}