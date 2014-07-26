using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour {

	public KeyCode moveLeft = KeyCode.LeftArrow;
	public KeyCode moveRight = KeyCode.RightArrow;

	public float speed = 30; 

	
	// Update is called once per frame
	void Update () {
	 
		if(Input.GetKey(moveLeft) && rigidbody.position.x > -22){
			rigidbody.velocity = new Vector3(speed * -1, 0, 0);
		}
		else if(Input.GetKey(moveRight) && rigidbody.position.x < 22){
			rigidbody.velocity = new Vector3(speed, 0, 0);
		}else{
			rigidbody.velocity = new Vector3(0, 0, 0);
		}
	}
}
