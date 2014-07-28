using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {

	public float speed = 13f;
	public bool canMove = false;

	// Use this for initialization
	void Start () {
	}

	void Update() {

		if(canMove){
			if(rigidbody.velocity.x != 0 & rigidbody.velocity.x > 0){
				rigidbody.velocity = new Vector3(speed, rigidbody.velocity.y, rigidbody.velocity.z);
			}else if(rigidbody.velocity.x != 0 & rigidbody.velocity.x < 0) {
				rigidbody.velocity = new Vector3(-speed, rigidbody.velocity.y, rigidbody.velocity.z);
			}

			if(rigidbody.velocity.y != 0 & rigidbody.velocity.y > 0){
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, speed, rigidbody.velocity.z);
			}else if(rigidbody.velocity.y != 0 & rigidbody.velocity.y < 0) {
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, -speed, rigidbody.velocity.z);
			}

			if(rigidbody.velocity.z != 0){
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
			}
		}
	}

	public void SetCanMove(bool move){
		canMove = move;
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
