using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static int score = 0;
	public  int leftBrickLimit = -15;
	public  int rightBrickLimit = 15;
	public  int brickIntervalHoriz = 4;

	public  int brickStartHeight = 16;
	public  int brickLines = 5;
	public  int brickIntervalVert = 2;

	public GameObject brick;
	// Use this for initialization
	void Start () {

		// brick wall initialization
		int height = brickStartHeight;

		for (int y = 0; y < brickLines; y += brickIntervalVert, height += brickIntervalVert) {
			for (int x = leftBrickLimit; x < rightBrickLimit; x += brickIntervalHoriz) {
				Instantiate(brick, new Vector3(x, height, 0), Quaternion.identity);
				}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUIStyle fontStyle = new GUIStyle();
		fontStyle.fontSize = 20;
		fontStyle.normal.textColor = Color.black;
		GUI.Label(new Rect (20,20,150,20), "Score: " + score, fontStyle); 
		
	}
	
	public static void IncreaseScore(){
		score += 10;
	}	
}
