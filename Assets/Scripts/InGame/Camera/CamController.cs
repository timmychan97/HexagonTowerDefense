using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamController : MonoBehaviour
{
	public float relativeSpeed = 1f; //change this to increase the speed overrall!
	public float minAngle = 20f;
	public float maxAngle = 80f;

	Vector3 targetVelocity = new Vector3(0, 0, 0);
	Vector3 velocity = new Vector3(0, 0, 0);
	float maxSpeed = 2f; // 5 + (curZoom+15)*relativeSpeed/20

	float initY; // 23f is a good number
	// Vector3 initForward; //the direction the camera is facing in the beginning

	private float minZoom = -15f;
	public float curZoom = 0f;
	private float maxZoom = 100f;

	Dictionary<KeyCode, Vector3> inputDict;

	private void InitInputDictionary()
    {
		inputDict = new Dictionary<KeyCode, Vector3> { 
			{ KeyCode.W, new Vector3(0, 0, 1f) },
			{ KeyCode.A, new Vector3(-1f, 0, 0) },
			{ KeyCode.S, new Vector3(0, 0, -1f) },
			{ KeyCode.D, new Vector3(1f, 0, 0) },
		};
    }

    void Start()
    {
		InitInputDictionary();

		initY = transform.position.y;
		UpdateMoveSpeed ();
		// initForward = transform.forward;
	}

	// Update is called once per frame
	void Update ()
	{
		// Input.mouseScrollDelta is not working with touchpads, therefor zooming is done in OnGUI()

		// Calculate move amount and direction
		Vector3 newVelo = Vector3.zero;
		foreach (var entry in inputDict)
			if (Input.GetKey(entry.Key)) newVelo += entry.Value;
		newVelo = newVelo.normalized;

		// Move the targetVelocity slowly to the middle/zero
		targetVelocity += newVelo;
		if (targetVelocity.magnitude <= 0.001f)
			targetVelocity = Vector3.zero;
		else
			targetVelocity = (targetVelocity - Vector3.zero) * 0.6f; // 0.6f is the easeAmount, the "drag in the air" stopping the cam

        // Move the real velocity against the targetVelocity
		velocity += (targetVelocity - velocity)/2;
        if (velocity.magnitude <= 0.01f && targetVelocity == Vector3.zero) {
            velocity = Vector3.zero;
		}
		transform.position += velocity * Time.deltaTime * 8 * maxSpeed;
	}

	public void OnGUI()
	{      
		if (Event.current.type == EventType.ScrollWheel) {
			if (EventSystem.current.IsPointerOverGameObject())
			{
				// The mouse pointer is over EventSystem game object, which is a UI element.
				// Do not scroll the game camera.
				return;
            }

			float dir = Event.current.delta.y;
			if (dir != 0f)
				SetZoom(curZoom + dir);
		}
	}

	public void Zoom(float zoomAmount)
	{
		// Related to current zoom
		curZoom += zoomAmount;
		transform.position += -transform.forward.normalized * zoomAmount;
	}

	public void SetZoom(float zoom)
	{ 
		// Set zoom to a specific amount
		// Assuming y != 0f
		var startPos = transform.position +
			(transform.forward/Mathf.Abs(transform.forward.y)) *
			(transform.position.y - initY);
		curZoom = Mathf.Clamp (zoom, minZoom, maxZoom);
		transform.position = startPos - transform.forward.normalized * curZoom;
		// Change the maxSpeed, so that it matches the relative speed
		UpdateMoveSpeed();
	}

    public void UpdateMoveSpeed() => maxSpeed = (2f + (curZoom + 15) / 10) * relativeSpeed;

}
