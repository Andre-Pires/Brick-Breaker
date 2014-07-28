using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public static int initLives = 5;
	private static int lives = initLives;
	private static int score = 0;
	public static int leftBrickLimit = -23;
	public static int rightBrickLimit = 20;
	public static int brickIntervalHoriz = 3;

	public static int brickStartHeight = 16;
	public static int brickLines = 8;
	public static int brickIntervalVert = 1;

	public static int playerStartX = 0;
	public static int playerStartY = 4;

	public static int ballStartX = playerStartX;
	public static int ballStartY = playerStartY + 2;
	public static float ballLowerLimit = 1.5f;

	enum state {Victory, Defeat, LifeLost, Reset, NewPlayer, StartGame, Default}; 
	private state currentState = state.Reset;
	public static int maxPlayers = 2;
	private static int bricksDestroyed = 0;
	public KeyCode startKey = KeyCode.F;
	public KeyCode resetKey = KeyCode.R;
	public KeyCode playerKey = KeyCode.Return;
	public GameObject brick;
	public GameObject ball;
	public GameObject player;
	private IList<GameObject> bricks = new List<GameObject>();
	private IList<GameObject> players = new List<GameObject>();
	private IList<PlayerMovement> playerMovement = new List<PlayerMovement>();

	// Use this for initialization
	void Start () {

		// brick wall initialization
		int height = brickStartHeight;

		for (int y = 0; y < brickLines; y += brickIntervalVert, height += brickIntervalVert) {
			for (int x = leftBrickLimit; x < rightBrickLimit; x += brickIntervalHoriz) {
				bricks.Add(Instantiate(brick, new Vector3(x, height, 0), Quaternion.identity) as GameObject);
			}
		}
		// ball initialization
		ball = Instantiate(ball, new Vector3(ballStartX, ballStartY, 0), Quaternion.identity) as GameObject;

		// player initialization
		players.Add(Instantiate(player, new Vector3(playerStartX, playerStartY, 0), Quaternion.identity) as GameObject);
		playerMovement.Add(players[players.Count -1].GetComponent<PlayerMovement>());
	}

	state Status(){

		if(lives <= 0){
			return state.Defeat;	
		}

		if(Input.GetKey(resetKey)){
			return state.Reset;
		}

		if((bricks.Count == bricksDestroyed)){
			return state.Victory;
		}

		// When you're about to start playing
		if(Input.GetKey(startKey) && (currentState == state.Reset || currentState == state.LifeLost)){
			return state.StartGame;
		}

		if(ball.rigidbody.position.y < ballLowerLimit){
			return state.LifeLost;
		}

		if(Input.GetKey(playerKey)){
			return state.NewPlayer;
		}

		
		return state.Default;
	}
	
	// Update is called once per frame
	void Update () {


		switch(Status ()){
			//In case you win
			case state.Victory:
				if(currentState != state.Victory){
					//Debug.Log(state.Victory);
					GameReset();
					currentState = state.Victory;
				}
				break;
			//In case you want to reset the game to the  initial state
			case state.Reset:
				if(currentState != state.Reset){
					//Debug.Log(state.Reset);
					GameReset();
					currentState = state.Reset;
				}
				break;
			//When you lose
			case state.Defeat:
				if(currentState != state.Defeat){
					//Debug.Log(state.Defeat);
					GameReset();
					currentState = state.Defeat;
				}
				break;
			//In case new player joins
			case state.NewPlayer:
				if(currentState != state.NewPlayer){
					//Debug.Log(state.NewPlayer);
					AddPlayer();
					currentState = state.NewPlayer;
					}
				break;
			//In case new player joins
			case state.LifeLost:
				if(currentState != state.LifeLost){
					//Debug.Log(state.LifeLost);
					BallReset();
					PlayerReset();
					lives--;
					currentState = state.LifeLost;
					}
				break;
			case state.StartGame:
				if(currentState != state.StartGame){
					//Debug.Log(state.StartGame);
					ball.rigidbody.AddForce(transform.up * 2);
					ball.GetComponent<BallMovement>().SetCanMove(true);
				
					foreach(PlayerMovement pMovement in  playerMovement){
						if(pMovement != null){
							pMovement.SetCanMove(true);
						}
					}
					currentState = state.StartGame;
				}
				break;
			default:
				break;//do nothing
		}
	}

	void OnGUI() {
		GUIStyle fontStyle = new GUIStyle();
		fontStyle.fontSize = 20;
		fontStyle.normal.textColor = Color.black;

		GUI.Label(new Rect (20,10,150,20), "Score: " + score, fontStyle);
		GUI.Label(new Rect (Screen.width - 100,10,150,20), "Lives: " + lives, fontStyle);

		switch(currentState){
			//In case you win
			case state.Victory:
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2, 150,20), "You win!", fontStyle);
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 30, 150,20), "Your score was: " + score, fontStyle);
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 60, 150,20), "Press the \"" + startKey + "\" Key " +
				          "to play again.", fontStyle);
				break;

			//In case you want to reset the game to the  initial state
			case state.LifeLost:
			case state.Reset:
				GUI.Label(new Rect (Screen.width/2 - 60, Screen.height/2 + 40, 150,20), "Press the \"" + startKey + "\" Key " +
				          "to start", fontStyle);
				break;
			//When you lose
			case state.Defeat:
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2, 150,20), "Game Over", fontStyle);
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 30, 150,20), "Your score was: " + score, fontStyle);
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 60, 150,20), "Press the \"" + resetKey + "\" Key " +
				          "to play again.", fontStyle);
				break;
			default:
				break;//do nothing
		}
	}
	
	public static void IncreaseScore(){
		score += 10;
	}

	public static void DestroyBrick(){
		bricksDestroyed++;
	}

	public void BallReset(){
		ball.GetComponent<BallMovement>().SetCanMove(false);
		ball.rigidbody.velocity = Vector3.zero;
		ball.rigidbody.angularVelocity = Vector3.zero;
		ball.transform.position = new Vector3(ballStartX, ballStartY , 0);
		
	}

	public void PlayerReset(){

		int playerOffset = 0;

		if(currentState == state.Defeat){
			foreach(GameObject p in players){
				Destroy(p);
			}
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

	public void BricksReset(){
		int height = brickStartHeight;

		foreach(GameObject brick in bricks){
			Destroy(brick);
		}
		bricks.Clear();
		
		for (int y = 0; y < brickLines; y += brickIntervalVert, height += brickIntervalVert) {
			for (int x = leftBrickLimit; x < rightBrickLimit; x += brickIntervalHoriz) {
				bricks.Add(Instantiate(brick, new Vector3(x, height, 0), Quaternion.identity) as GameObject);
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
		score = 0;
		lives = initLives;
		bricksDestroyed = 0;
		BallReset();
		PlayerReset();
		BricksReset();
	}
}
