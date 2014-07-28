﻿using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour {

	public KeyCode moveLeft = KeyCode.LeftArrow;
	public KeyCode moveRight = KeyCode.RightArrow;
	public bool canMove = false;

	public float speed = 30; 
	public float leftPlayerLimit = -22; 
	public float rightPlayerLimit = 21; 

	public void SetCanMove(bool move){
		canMove = move;
	}
	
	// Update is called once per frame
	void Update () {
	 	
		if(canMove){
			if(Input.GetKey(moveLeft) && rigidbody.position.x > leftPlayerLimit){
				rigidbody.velocity = new Vector3(speed * -1, 0, 0);
			}
			else if(Input.GetKey(moveRight) && rigidbody.position.x < rightPlayerLimit){
				rigidbody.velocity = new Vector3(speed, 0, 0);
			}else{
				rigidbody.velocity = new Vector3(0, 0, 0);
			}
		}
	}
}