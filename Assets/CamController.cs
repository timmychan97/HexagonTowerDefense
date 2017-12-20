using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {
    Vector3 targetVelocity = new Vector3(0, 0, 0);
    Vector3 velocity = new Vector3(0, 0, 0);
    // Use this for initialization
    float maxSpeed = 5f;

    /*Ease Properties:*/
    float easeDuration = 3f;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.W)) {
            print("W");
            targetVelocity += new Vector3(0, 0, 2f);
        }
        if(Input.GetKey(KeyCode.S)) {
            targetVelocity -= new Vector3(0, 0, 2f);
        }
        if(Input.GetKey(KeyCode.A)) {
            targetVelocity -= new Vector3(2f, 0, 0);
        }
        if(Input.GetKey(KeyCode.D)) {
            targetVelocity += new Vector3(2f, 0, 0);
        }
        print("f" + targetVelocity);
        //move the targetVelocity slowly to the middle/zero
        if(targetVelocity.magnitude == 0.01f)
            targetVelocity = Vector3.zero;
        else
            targetVelocity = (targetVelocity-Vector3.zero)*0.3f; //t is [0,1]
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, maxSpeed); //limits the speed below maxSpeed
        print(targetVelocity);
        //move the real velocity against the targetVelocity
        velocity = Vector3.Lerp(velocity,targetVelocity, 1f);
        if (velocity.magnitude <= 0.01f && targetVelocity == Vector3.zero) {
            velocity = Vector3.zero;
        }
        transform.position += velocity;
	}
}
