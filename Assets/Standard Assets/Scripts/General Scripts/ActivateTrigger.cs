using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
	void OnCollisionEnter(Collision collision) {

			Destroy(this);
		GameObject.Destroy(this);


	}
}