using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {


    //initial variables to control game settings
	private static int score = 0;
	public static int initLives = 3;
	private static int lives = initLives;
	public static int leftBrickLimit = -23;
	public static int rightBrickLimit = 23;
	public static int brickIntervalHoriz = 3;

	public static int brickStartHeight = 16;
	public static int brickLines = 8;
	public static int brickIntervalVert = 1;

	public static int playerStartX = 0;
	public static int playerStartY = 4;

	public static int ballStartX = playerStartX;
	public static int ballStartY = playerStartY + 2;
	public static float ballLowerLimit = -0.5f;

	public KeyCode resetKey = KeyCode.R;
	public KeyCode playerKey = KeyCode.Return;

    //------------------------------------------

    // Game variables 
    public float playerX = 0;
    public float playerY = 999;
	public GameObject brick;
	public GameObject ball;
	public GameObject player;
	private enum state {Victory, Defeat, LifeLost, Reset, NewPlayer, StartGame, Default}; 
	private state currentState = state.Reset;
	private static int bricksDestroyed = 0;
	public static int maxPlayers = 1;
	private IList<GameObject> bricks = new List<GameObject>();
	private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

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
	}

	private state Status(){

        //applies when ball reaches lower threshold and it isn't the player's last life
		if (ball.rigidbody.position.y < ballLowerLimit && lives != 0)
        {
            return state.LifeLost;
        }

        if (lives <= 0)
        {
            return state.Defeat;
        }

        if (Input.GetKey(resetKey))
        {
            return state.Reset;
        }

        if (bricks.Count == bricksDestroyed)
        {
            return state.Victory;
        }
        
        if (toStart() == state.StartGame  && !(currentState == state.Defeat || currentState == state.Victory))
        {
            return state.StartGame;
        }
        return currentState;
	}
	
	// Update is called once per frame
	void Update () {

        switch(Status ()){
			//In case you win
			case state.Victory:
				if(currentState != state.Victory){
                    Debug.Log(state.Victory);
                    GameReset();
                    currentState = state.Victory;
				}
                if (toStart() == state.StartGame)
                {
                    Debug.Log(state.StartGame);
                    ball.rigidbody.AddForce(transform.up * 2);
                    ball.GetComponent<BallMovement>().SetCanMove(true);
                    currentState = state.StartGame;
                }
				break;
			//In case you want to reset the game to the  initial state
			case state.Reset:
				if(currentState != state.Reset){
					Debug.Log(state.Reset);
					GameReset();
					currentState = state.Reset;
				}
				break;
            //In case you want to reset the game to the  initial state
            case state.LifeLost:
                if (currentState != state.LifeLost)
                {
                    Debug.Log(state.LifeLost);
                    BallReset();
                    PlayerReset();
                    lives--;
                    currentState = state.LifeLost;
                }
                break;
			//When you lose
			case state.Defeat:
                if (currentState != state.Defeat)
                {
                    Debug.Log(state.Defeat);
                    GameReset();
                    currentState = state.Defeat;
                }
                if (toStart() == state.StartGame)
                {
                    Debug.Log(state.StartGame);
                    ball.rigidbody.AddForce(transform.up * 2);
                    ball.GetComponent<BallMovement>().SetCanMove(true);
                    currentState = state.StartGame;
                }
				break;
			case state.StartGame:
				if(currentState != state.StartGame){
					Debug.Log(state.StartGame);
					ball.rigidbody.AddForce(transform.up * 2);
					ball.GetComponent<BallMovement>().SetCanMove(true);
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
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 60, 150,20), "Go up to the keep playing.", fontStyle);
				break;

			//In case you want to reset the game to the  initial state
			case state.LifeLost:
			case state.Reset:
				GUI.Label(new Rect (Screen.width/2 - 60, Screen.height/2 + 40, 150,20), "Go up to the ball to start", fontStyle);
				break;
			//When you lose
			case state.Defeat:
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2, 150,20), "Game Over", fontStyle);
				GUI.Label(new Rect (Screen.width/2 - 30, Screen.height/2 + 30, 150,20), "Your score was: " + score, fontStyle);
                GUI.Label(new Rect(Screen.width / 2 - 30, Screen.height / 2 + 60, 150, 20), "Go up to the ball to play again.", fontStyle);
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

    private state toStart(){

        if (playerY < 15 && players.Count > 0)
            return state.StartGame;
        else
            return state.Default;
    }

	public void BallReset(){
		ball.GetComponent<BallMovement>().SetCanMove(false);
		ball.rigidbody.velocity = Vector3.zero;
		ball.rigidbody.angularVelocity = Vector3.zero;
		ball.transform.position = new Vector3(ballStartX, ballStartY , 0);
		
	}

    public void PlayerReset()
    {
        playerY = 999;

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

	public void AddPlayer(string id, float x){
		// player initialization
		if(players.Count < maxPlayers){
			Vector3 pos = new Vector3(x, playerStartY, 0);
			players.Add(id ,Instantiate(player, pos,Quaternion.identity) as GameObject);
		}
	}

    public void SetPosition(string id, float x, float y){
        
        if (players.Count < maxPlayers && !(players.ContainsKey(id))){
            Debug.Log("Player added!!");
            AddPlayer(id, x);
        }


        // Converting X position
        float old_rangeX = 1 - 0; // old_max - old_min;
        float new_rangeX = rightBrickLimit - leftBrickLimit; // new_max - new_min;
        float convertedPosX = leftBrickLimit + (x - 0) * new_rangeX / old_rangeX; // new_min + (x - old_min) * new_range / old_range;

        // Converting Y position
        float old_rangeY = 1 - 0; // old_max - old_min;
        float new_rangeY = 25 - 0; // new_max - new_min;
        float convertedPosY = 0 + (y - 0) * new_rangeY / old_rangeY; //new_min + (x - old_min) * new_range / old_range;

        if (convertedPosY < playerY) // y axis is inverted in the game floor
            playerY = convertedPosY;
        if (convertedPosX > playerX)
            playerX = convertedPosX;

                 
        if (players.ContainsKey(id))
        {
            players[id].GetComponent<PlayerMovement>().SetPos(convertedPosX);
            //Debug.Log("Converted position X: " + convertedPosX + " Y: " + convertedPosY);
        }
    }

    public void RemovePlayer(string deadPlayer)
    {
        Destroy(players[deadPlayer]);
        Destroy(players[deadPlayer].GetComponent<PlayerMovement>());
        players.Remove(deadPlayer);
    }

	public void GameReset(){
		score = 0;
		lives = initLives;
		bricksDestroyed = 0;
		BallReset();
		BricksReset();
        PlayerReset();
	}
}
