using UnityEngine;
using System.Collections;
using System;


public class PlayerMovement : MonoBehaviour {

	public KeyCode moveLeft = KeyCode.LeftArrow;
	public KeyCode moveRight = KeyCode.RightArrow;
    public float currentPosY = 999;
    public float currentPosX = 999;
    public float previousPosX = 999;

	public float speed = 30;
    public float speedLimit = 30;
    public float speedMultiplier = 1.7f;

	
	// Update is called once per frame
	void Update () {
    /*
        if(Math.Abs(speed) <= speedLimit)
		    rigidbody.velocity = new Vector3(speed * speedMultiplier, 0, 0);
        else
            rigidbody.velocity = new Vector3(speedLimit * speedMultiplier, 0, 0);
     */
	}

    public void SetPos(float x)
    {
        /*
        previousPosX = currentPosX;
        currentPosX = x;
        speed = (currentPosX - previousPosX) / Time.deltaTime;
         * */

        rigidbody.position = new Vector3(x, 0, 0);

    }
}
