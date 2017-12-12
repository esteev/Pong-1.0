using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {
	private const int WIN_LIMIT = 7;
	private const float BLOKE_MULTIPLIER = 1f;
	private const float WALL_MULTIPLIER = 3f;
	private int GameplayType = 0;
	private float PAD_MULTIPLIER = 1f;
	public GameObject BlokeGroup;
	public GameObject goScreen;
	public GameObject otherScreen;

	public bool GameOver;
	public int coinCount;
	public float AI_Point, player_Point;
	public float AI_BlokePoint,player_BlokePoint;
	public float AI_PadPoint,player_PadPoint;
	public float AI_WallPoint, player_WallPoint;
	public GameObject ground;
	public GameObject AI_Score,player_Score;
	private bool ShowInterAd;
	private float gameStartTime;

	public Text coinEarned;
	public Text coinsEarned;
	public float HighScore;
	private bool HighScoreTuta;


	// Use this for initialization
	void Start () {
		BlokeGroup = GameObject.Find ("BlokeGroup");
		AI_Point = 0;
		player_Point = 0;
		AI_BlokePoint = 0;
		player_BlokePoint = 0;
		AI_PadPoint = 0;
		player_PadPoint = 0;
		goScreen.SetActive (false);
		otherScreen.SetActive (true);
		ground = GameObject.Find ("Ground");
		//AI_Score = GameObject.Find ("AI_Score");
		//player_Score = GameObject.Find ("Player_Score");




		if (SceneManager.GetActiveScene ().name == "Pong_Breaker") {
			GameplayType = 1;
			//GameManager GMscript = ground.GetComponent<GameManager> ();
			//	turn = GMscript.turn;

		} else if (SceneManager.GetActiveScene ().name == "Pong_Breaker_Endless") {
			GameplayType = 2;
			HighScore = PlayerPrefs.GetFloat ("HighScore");
			//	GameManagerEndless GMscript = ground.GetComponent<GameManagerEndless> ();
			//	turn = GMscript.turn;

		} else if (SceneManager.GetActiveScene ().name == "Pong_Breaker_Practice") {
			GameplayType = 3;
			//	GameManagerPractice GMscript = ground.GetComponent<GameManagerPractice> ();
			//	turn = GMscript.turn;
		}else if (SceneManager.GetActiveScene ().name == "Pong_Breaker_Practice_Wall") {
			GameplayType = 4;
			//	GameManagerPractice GMscript = ground.GetComponent<GameManagerPractice> ();
			//	turn = GMscript.turn;
		}

		if (youdidthistoher.Instance.MCDActive == 1) {
			PAD_MULTIPLIER = 0.5f;
		} else if (youdidthistoher.Instance.DrunkActive == 1) {
			PAD_MULTIPLIER = 2f;
		}
	}




	// Update is called once per frame
	void Update () {
		if(GameOver) return;
		//for campaign mode

		//print ("coins "+coinCount);



		if (GameplayType == 1) {
			//current Level
			coinsEarned.text = coinCount.ToString ();
			player_Point = player_BlokePoint * BLOKE_MULTIPLIER + (AI_WallPoint)*WALL_MULTIPLIER;
			AI_Point = AI_BlokePoint * BLOKE_MULTIPLIER+(player_WallPoint)*WALL_MULTIPLIER;
			player_Point *= PAD_MULTIPLIER;

			player_Score.GetComponent<Text> ().text = player_Point + "";
			AI_Score.GetComponent<Text> ().text = AI_Point+"";
			if (BlokeRemaining() == 0) {// practice ends here

				int levelLooseCount=PlayerPrefs.GetInt("levelLooseCount");
				levelLooseCount++;												//level end count
				if (levelLooseCount % 4 == 0) {
					ShowInterAd = true;
				}
				youdidthistoher.Instance.LevelLooseCount = levelLooseCount;
				GameOver = true;
				if (player_Point > AI_Point) {

					int levelReached =PlayerPrefs.GetInt ("campaignLevelReached");
					int currentLevel = PlayerPrefs.GetInt ("currentPlayingLevel");

					if(currentLevel ==levelReached){
						youdidthistoher.Instance.campaignLevelReached = ++levelReached;
						youdidthistoher.Instance.Save ();
						if ((levelReached) % 5 == 0) {
							//////
							AdManager.Instance.ShowVideo ();
							/////
							ShowInterAd=false;
						
						}
					}
				//player is winner
					gogoScreen(0);
				}else{
					gogoScreen(1);
				}				
			}
		} else if (GameplayType == 2) {
			if (Time.time - gameStartTime > 100f) {
				ShowInterAd = true;
			}

			coinsEarned.text = coinCount.ToString ();
			player_Point = player_BlokePoint * BLOKE_MULTIPLIER + (AI_WallPoint)*WALL_MULTIPLIER ;
			AI_Point = AI_BlokePoint * BLOKE_MULTIPLIER + (player_WallPoint )*WALL_MULTIPLIER;
			player_Point *= PAD_MULTIPLIER;
			player_Score.GetComponent<Text> ().text = player_Point + "";
			AI_Score.GetComponent<Text> ().text = AI_Point+"";

			if (HighScore < player_Point) {
				HighScoreTuta = true;
			}



			if (player_WallPoint >= WIN_LIMIT) {//endless game ends here
				GameOver = true;
				//PlayerPrefs.SetInt ("HighScore", (int)player_Point);
				if (HighScoreTuta) {
					youdidthistoher.Instance.HighScore = player_Point;
					youdidthistoher.Instance.Save ();
					HighScoreTuta = false;
				}

				if (player_Point >= AI_Point) {
					//	playerWinEffects ();
					//player is winner
					gogoScreen(0);
				}else{
					//AI is winner

				//	AIWinEffects ();
					gogoScreen(1);
				}
			}		
		} else if (GameplayType == 3) {
			player_Point = player_PadPoint ;
			AI_Point = AI_PadPoint;
			player_Score.GetComponent<Text> ().text = player_Point + "";
		/*	if(player_WallPoint>=WIN_LIMIT){
				if (player_Point >= AI_Point) {
					//player is winner
				}else{
					//AI is winner	
				}
		*/	}

		else if (GameplayType == 4) {
			player_Point = player_PadPoint ;
			AI_Point = AI_PadPoint;
			player_Score.GetComponent<Text> ().text = player_Point + "";
		/*	if(player_WallPoint>=WIN_LIMIT){
				if (player_Point >= AI_Point) {
					//player is winner
				}else{
					//AI is winner	
				}
			}
*/
		}

	}

	public void gogoScreen(int state)
	{	/////
		if(ShowInterAd){
			ShowInterAd = false;
			/////
			AdManager.Instance.ShowInterstitial();
			///// 
		}


		/////
		AdManager.Instance.ShowBanner();
		/////
		otherScreen.SetActive (false);
		goScreen.SetActive (true);
		goScreen.transform.GetChild(0).GetChild (state).gameObject.SetActive (true);
		goScreen.GetComponent<GOBABY> ().state = state;
		coinEarned.text = "Coins Earned : " + GameObject.Find ("Ground").GetComponent<ScoreManager> ().coinCount.ToString ();
		for (int i = 0; i < ground.GetComponent<PowerUp> ().ballList.Length; i++)
			ground.GetComponent<PowerUp> ().ballList [i].SetActive (false);
	}


	int BlokeRemaining(){
		int count = 0;
		foreach (Transform t in BlokeGroup.transform) {	
			if (t.gameObject.activeSelf&& t.gameObject.CompareTag("Bloke"))
				count++;
		}
		return count;
	}

	public void BlokePoint(bool turn){
		if (turn) {
			player_BlokePoint++;	
		}
		else {
			AI_BlokePoint++;
		}
	}

	/*
	void OnDestroy()
	{
		AI_Point = 0;
		player_Point = 0;
		coinCount = 0;
		otherScreen.SetActive (true);
		goScreen.SetActive (false);
		goScreen.transform.GetChild(0).GetChild (0).gameObject.SetActive (false);
		goScreen.transform.GetChild(0).GetChild (1).gameObject.SetActive (false);
	}
	*/
}