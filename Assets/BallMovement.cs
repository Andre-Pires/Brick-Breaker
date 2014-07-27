using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {

	public float speed = 70f;

	// Use this for initialization
	void Start () {
	}

	void Update() {

		if(speed < rigidbody.velocity.x){
			rigidbody.velocity.Set(speed, rigidbody.velocity.y, rigidbody.velocity.z);
		}
		else if(speed < rigidbody.velocity.y){
			rigidbody.velocity.Set(rigidbody.velocity.x, speed, rigidbody.velocity.z);
		}
		else if(speed < rigidbody.velocity.z){
			rigidbody.velocity.Set(rigidbody.velocity.x, rigidbody.velocity.y, speed);
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
