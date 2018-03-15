using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMenuPlayer : MonoBehaviour
{
	enum States { None, Dragging, StoppedDragging, ReturningToStartPos };

	[SerializeField] Rect screenRelativeTouchArea = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
	[SerializeField] float rotationSpeed;
	[SerializeField] float idleTimeBeforeReturning = 2.0f;
	[SerializeField] float returnBackSpeed = 180.0f;

	float targetRotY;
	float startingRotY;
	float horizontalVelocity;
	Vector3 eulerRot;
	States state;
	float timeToStartReturning;

	#if UNITY_EDITOR
	Vector3 lastMousePos;
	#endif

	void Awake()
	{
		startingRotY = transform.localEulerAngles.y;
	}

	void OnEnable()
	{
		#if UNITY_EDITOR
		lastMousePos = Vector3.zero;
		#endif

		eulerRot = transform.localEulerAngles;
		SetState(States.None);
	}

	void Update()
	{
		switch (state)
		{
			case States.None:
				CheckForTouchDown();
				break;

			case States.Dragging:
				#if UNITY_EDITOR
				if (Input.GetMouseButton(0))
				{
					Vector3 mousePos = Input.mousePosition;
					if (lastMousePos != Vector3.zero)
					{
						Vector3 diff = (mousePos - lastMousePos) * Time.deltaTime * rotationSpeed;
						targetRotY -= diff.x;
					}

					lastMousePos = mousePos;
				}
				else
				{
					lastMousePos = Vector3.zero;
					SetState(States.StoppedDragging);
				}
				#else
					if (Input.touchCount > 0)
					{
						Touch touch = Input.GetTouch(0);
						float diff = touch.deltaPosition.x * Time.deltaTime * rotationSpeed;
						targetRotY -= diff;
					}
					else
						SetState(States.StoppedDragging);
				#endif
				break;

			case States.StoppedDragging:
				if (Time.fixedTime > timeToStartReturning)
					SetState(States.ReturningToStartPos);
				else
					CheckForTouchDown();
				break;

			case States.ReturningToStartPos:
				if (targetRotY > startingRotY)
				{
					targetRotY -= returnBackSpeed * Time.deltaTime;
					if (targetRotY < startingRotY)
						SetState(States.None);
				}
				else
				{
					targetRotY += returnBackSpeed * Time.deltaTime;
					if (targetRotY > startingRotY)
						SetState(States.None);
				}
				break;

			default:
				throw new UnityException("Unhandled RotateMenu state " + state);
		}
	}

	void CheckForTouchDown()
	{
		#if UNITY_EDITOR
			if (Input.GetMouseButtonDown(0) && IsPositionInTouchArea(Input.mousePosition.x, Input.mousePosition.y))
				SetState(States.Dragging);
		#else
			if ((Input.touchCount > 0) && IsPositionInTouchArea(Input.touches[0].position.x, Input.touches[0].position.y))
			SetState(States.Dragging);
		#endif
	}

	void SetState(States _newState)
	{
		state = _newState;
		switch (state)
		{
			case States.None:
				targetRotY = eulerRot.y;
				break;

			case States.StoppedDragging:
				timeToStartReturning = Time.fixedTime + idleTimeBeforeReturning;
				break;

			case States.ReturningToStartPos:
				while (targetRotY < startingRotY - 180.0f)
					targetRotY += 360.0f;
				while (targetRotY > startingRotY + 180.0f)
					targetRotY -= 360.0f;
				eulerRot.y = targetRotY;
				break;

			default:
				break;
		}
	}

	bool IsPositionInTouchArea(float _xPos, float _yPos)
	{
		float relX = _xPos / Screen.width;
		float relY = 1.0f - (_yPos / Screen.height);

		return	(relX > screenRelativeTouchArea.x) && (relX < screenRelativeTouchArea.x + screenRelativeTouchArea.width) &&
				(relY > screenRelativeTouchArea.y) && (relY < screenRelativeTouchArea.y + screenRelativeTouchArea.height);
	}

	private void LateUpdate()
	{
		if (state != States.None)
		{
			eulerRot.y = Mathf.SmoothDamp(eulerRot.y, targetRotY, ref horizontalVelocity, 0.1f);
			transform.localEulerAngles = eulerRot;
		}
	}
}
