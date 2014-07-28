using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public static int initLives = 5;
	private static int lives = initLives;
	private static int score = 0;
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

	public static int maxPlayers = 2;
	public KeyCode startKey = KeyCode.F;
	public KeyCode resetKey = KeyCode.R;
	public KeyCode playerKey = KeyCode.Return;
	public GameObject brick;
	public GameObject ball;
	public GameObject player;
	private IList<GameObject> players = new List<GameObject>();
	private IList<PlayerMovement> playerMovement = new List<PlayerMovement>();

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
		players.Add(Instantiate(player, new Vector3(playerStartX, playerStartY, 0), Quaternion.identity) as GameObject);
		playerMovement.Add(players[players.Count -1].GetComponent<PlayerMovement>());
	}
	
	// Update is called once per frame
	void Update () {

		//In case you want to reset the game to the  initial state
		if(Input.GetKey(resetKey)){
			GameReset();
		}

		//In case new player joins
		if(Input.GetKey(playerKey)){
			AddPlayer();
		}

		// When you lost a life
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

		// When you're about to start playing
		if(Input.GetKey(startKey) && gameReset){
			gameReset = false;
			ball.rigidbody.AddForce(transform.up * 50);

			foreach(PlayerMovement pMovement in  playerMovement){
				if(pMovement != null){
					pMovement.SetCanMove(true);
				}
			}
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
				GUI.Label(new Rect (Screen.width/2 - 60, Screen.height/2 + 40, 150,20), "Press the \"" + startKey + "\" Key " +
					"to start", fontStyle);

		}else{
			GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2, 150,20), "Game Over", fontStyle);
			GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 30, 150,20), "Your score was: " + score, fontStyle);
			GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 60, 150,20), "Press the \"" + resetKey + "\" Key " +
				"to play again.", fontStyle);
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

		int playerOffset = 0;

		if(gameOver){
			players.Clear();
			playerMovement.Clear();
			players.Add(Instantiate(player, new Vector3(playerStartX, playerStartY, 0), Quaternion.identity) as GameObject);
			playerMovement.Add(players[players.Count -1].GetComponent<PlayerMovement>());
		}else {
			foreach(GameObject player in players){
				if(player != null){
					player.transform.position = new Vector3(playerStartX + playerOffset, playerStartY , 0);
					player.rigidbody.velocity = Vector3.zero;
					player.rigidbody.angularVelocity = Vector3.zero;
					player.GetComponent<PlayerMovement>().SetCanMove(false);
				}
				playerOffset += 10;
			}
		}
	}

	public void AddPlayer(){
		// player initialization
		if(players.Count < maxPlayers){
			players.Add(Instantiate(player, new Vector3(playerStartX + 10, playerStartY, 0),
			                        Quaternion.identity) as GameObject);
			playerMovement.Add(players[players.Count -1].GetComponent<PlayerMovement>());
		}
	}

	public void GameReset(){
		gameReset = true;
		gameOver = false;
		score = 0;
		lives = initLives;
		BallReset();
		PlayerReset();
	}
}
