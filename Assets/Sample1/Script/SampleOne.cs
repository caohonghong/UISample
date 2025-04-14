using UnityEngine;
using System.Collections;

public class SampleOne : MonoBehaviour
{

	[Header("Game Mode Scroll")]
	[SerializeField] ScrollRectEnsureVisible gameModeScroll;
	[SerializeField] RectTransform[] scrollElementRectTransforms;

	internal int					scrollState = 0;


	public void ScrollGameModePanel(int a_scrollIndex)
	{
		StartCoroutine(DelayedScrollGameModePanel(a_scrollIndex));
	}


	IEnumerator DelayedScrollGameModePanel(int a_scrollIndex)
	{
		if (a_scrollIndex > 0)
		{
			for (int i = 1; i < scrollElementRectTransforms.Length; ++i)
				scrollElementRectTransforms[i].gameObject.SetActive(i == a_scrollIndex);
		}
		scrollState = a_scrollIndex;

		yield return null;

		//We should be able to scroll right to scrollElement[scrollIndex],
		//but on the first time, sometimes this won't have the right position yet:
		//so instead, always scroll to element 0 or 1.
		gameModeScroll.CenterOnItem(scrollElementRectTransforms[a_scrollIndex > 0 ? 1 : 0], false);
	}

}
