using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static int score = 0;
	public static int lives = 5;
	public static int leftBrickLimit = -23;
	public static int rightBrickLimit = 23;
	public static int brickIntervalHoriz = 3;

	public static int brickStartHeight = 16;
	public static int brickLines = 8;
	public static int brickIntervalVert = 1;

	public static int playerStartX = 0;
	public static int playerStartY = 2;

	public static int ballStartX = playerStartX;
	public static int ballStartY = playerStartY + 2;
	public static float ballLowerLimit = 1.5f;
	private bool gameReset = true;
	private bool gameOver = false;

	public KeyCode startGame = KeyCode.F;
	public GameObject brick;
	public GameObject ball;
	public GameObject player;
	public PlayerMovement playerMovement;

	// Use this for initialization
	void Start () {

		// brick wall initialization
		int height = brickStartHeight;

		for (int y = 0; y < brickLines; y += brickIntervalVert, height += brickIntervalVert) {
			for (int x = leftBrickLimit; x < rightBrickLimit; x += brickIntervalHoriz) {
				Instantiate(brick, new Vector3(x, height, 0), Quaternion.identity);
				}
		}
		// ball initialization
		ball = Instantiate(ball, new Vector3(ballStartX, ballStartY, 0), Quaternion.identity) as GameObject;

		// player initialization
		player = Instantiate(player, new Vector3(playerStartX, playerStartY, 0), Quaternion.identity) as GameObject;
		playerMovement = player.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {

		if(ball.rigidbody.position.y < ballLowerLimit){
			BallReset();
			PlayerReset();
			gameReset = true;
			lives--;

			if(lives <= 0){
				gameReset = false;
				gameOver = true;
			}
		}


		if(Input.GetKey(startGame) && gameReset){
			gameReset = false;
			playerMovement.SetCanMove(true);
			ball.rigidbody.AddForce(transform.up * 50);
		}
	}

	void OnGUI() {
		GUIStyle fontStyle = new GUIStyle();
		fontStyle.fontSize = 20;
		fontStyle.normal.textColor = Color.black;

		if(!gameOver){
			GUI.Label(new Rect (20,10,150,20), "Score: " + score, fontStyle);
			GUI.Label(new Rect (Screen.width - 100,10,150,20), "Lives: " + lives, fontStyle);

			if(!gameOver && gameReset)
				GUI.Label(new Rect (Screen.width/2 - 60, Screen.height/2 + 40, 150,20), "Press the " + startGame + " Key to start", fontStyle);

		}else{
			GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2, 150,20), "Game Over", fontStyle);
			GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 40, 150,20), "Your score was: " + score, fontStyle);
		}
	}
	
	public static void IncreaseScore(){
		score += 10;
	}

	public void BallReset(){
		ball.transform.position = new Vector3(ballStartX, ballStartY , 0);
		ball.rigidbody.velocity = Vector3.zero;
		ball.rigidbody.angularVelocity = Vector3.zero;
		
	}

	public void PlayerReset(){
		player.transform.position = new Vector3(playerStartX, playerStartY , 0);
		player.rigidbody.velocity = Vector3.zero;
		player.rigidbody.angularVelocity = Vector3.zero;
		playerMovement.SetCanMove(false);
	}
}
