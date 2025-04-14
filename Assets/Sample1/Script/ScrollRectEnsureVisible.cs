using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


[RequireComponent(typeof(ScrollRect))]
public class ScrollRectEnsureVisible : MonoBehaviour
{
    

	#region Public Variables

	public float _AnimTime = 0.15f;
	public bool _LockX = false;
	public bool _LockY = true;
	public RectTransform _MaskTransform;

	#endregion

	#region Private Variables

	private ScrollRect mScrollRect;
	internal RectTransform mContent;
	bool initialised = false;

	#endregion  

	void Initialise()
	{
		mScrollRect = GetComponent<ScrollRect>();
		mContent = mScrollRect.content;
		initialised = true;
	}

	#region Public Methods

	public void CenterOnItem(RectTransform target, bool snap, float overrideAnimTime = -1.0f, Action onComplete = null)
	{
		if (!initialised)
			Initialise();

		Vector3 maskCenterPos = _MaskTransform.position + new Vector3(_MaskTransform.rect.center.x * _MaskTransform.lossyScale.x,
			_MaskTransform.rect.center.y * _MaskTransform.lossyScale.y,
			0.0f);
		Vector3 itemCenterPos = target.position;
		Vector3 difference = maskCenterPos - itemCenterPos;
		difference.z = 0;

		Vector3 newPos = mContent.position + difference;
		if (_LockX)
			newPos.x = mContent.position.x;
		if (_LockY)
			newPos.y = mContent.position.y;

		if (snap)
			mContent.position = new Vector3(newPos.x, newPos.y, newPos.z);
		else
			StartCoroutine(DelayedCenter(new Vector3(newPos.x, newPos.y, newPos.z), overrideAnimTime, onComplete));
	}

	IEnumerator DelayedCenter(Vector3 a_endPos, float a_overrideAnimTime, Action a_onComplete)
	{
		float startTime = Time.time;
		Vector3 startPos = mContent.position;
		float animTime = (a_overrideAnimTime < 0.0f ? _AnimTime : a_overrideAnimTime);
		while (Time.time < startTime + animTime)
		{
			yield return null;

			mContent.position = Vector3.Lerp(startPos, a_endPos, (Time.time - startTime) / animTime);
		}
		mContent.position = a_endPos;
		if (a_onComplete != null)
			a_onComplete();
	}

	#endregion
}