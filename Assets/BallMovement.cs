using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {

	float maxSpeed = 70f;
	// Use this for initialization
	void Start () {
		rigidbody.AddForce(new Vector2(0, 100));
	}

	void Update() {

		if(maxSpeed < rigidbody.velocity.x){
			rigidbody.velocity.Set(maxSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
		}
		else if(maxSpeed < rigidbody.velocity.y){
			rigidbody.velocity.Set(rigidbody.velocity.x, maxSpeed, rigidbody.velocity.z);
		}
		else if(maxSpeed < rigidbody.velocity.z){
			rigidbody.velocity.Set(rigidbody.velocity.x, rigidbody.velocity.y, maxSpeed);
		}
	}

		
	void OnCollisionEnter (Collision collision){
		
		if (collision.gameObject.tag.Equals("Brick")){
			
			GameObject toDestroy = FindClosestBrick();
			if(toDestroy != null){
				Destroy(toDestroy);
			}

			GameMaster.IncreaseScore();
		}


	}
	
	GameObject FindClosestBrick() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Brick");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
}
