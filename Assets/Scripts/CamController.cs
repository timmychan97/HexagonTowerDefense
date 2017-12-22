using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {
	public float relativeSpeed = 1f; //change this to increase the speed overrall!


	Vector3 targetVelocity = new Vector3(0, 0, 0);
	Vector3 velocity = new Vector3(0, 0, 0);
	float maxSpeed = 2f; //5 + (curZoom+15)*relativeSpeed/20

	float initY; //23f is a good number
	//Vector3 initForward; //the direction the camera is facing in the beginning

	private float minZoom = -15f;
	public float curZoom = 0f;
	private float maxZoom = 100f;

	void Start () {
		initY = transform.position.y;
		updateMoveSpeed ();
		//initForward = transform.forward;
	}

	// Update is called once per frame
	void Update () {
		Vector3 newVelo = Vector3.zero;
		if(Input.GetKey(KeyCode.W)) {
            newVelo += new Vector3(0, 0, 1f);
        }
        if(Input.GetKey(KeyCode.S)) {
            newVelo -= new Vector3(0, 0, 1f);
        }
        if(Input.GetKey(KeyCode.A)) {
            newVelo -= new Vector3(1f, 0, 0);
        }
        if(Input.GetKey(KeyCode.D)) {
            newVelo += new Vector3(1f, 0, 0);
        }
		//Input.mouseScrollDelta is not working with touchpads, therefor zooming is done in OnGUI()
		newVelo = newVelo.normalized;
        //move the targetVelocity slowly to the middle/zero
		targetVelocity += newVelo;
		if (targetVelocity.magnitude <= 0.001f)
			targetVelocity = Vector3.zero;
		else
			targetVelocity = (targetVelocity - Vector3.zero) * 0.6f; //0.6f is the easeAmount, the "drag in the air" stopping the cam
        //move the real velocity against the targetVelocity
		velocity += (targetVelocity - velocity)/2;
        if (velocity.magnitude <= 0.01f && targetVelocity == Vector3.zero) {
            velocity = Vector3.zero;
		}
		transform.position += velocity* Time.deltaTime * 8 *maxSpeed;
	}

	public void OnGUI()
	{      
		if (Event.current.type == EventType.ScrollWheel) {
			float dir = Event.current.delta.y;
			if (dir != 0f)
				setZoom(curZoom + dir);
		}
	}

	public void zoom(float zoomAmount){ //related to current zoom
		curZoom += zoomAmount;
		transform.position += -transform.forward.normalized * zoomAmount;
	}
	public void setZoom(float zoom){ //set zoom to a specific amount
		//assuming y != 0f
		var startPos = transform.position +
			(transform.forward/Mathf.Abs(transform.forward.y)) *
			(transform.position.y - initY);
		curZoom = Mathf.Clamp (zoom, minZoom, maxZoom);
		transform.position = startPos - transform.forward.normalized * curZoom;
		//change the maxSpeed, so that it matches the relative speed
		updateMoveSpeed();
	}

	public void updateMoveSpeed(){
		maxSpeed = (2f + (curZoom + 15) / 10) * relativeSpeed;
	}

}
