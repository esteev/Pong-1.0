using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSetter : MonoBehaviour {
	private Transform BlokeGroup;

	private const float BALL_SPEEDUP_MAX = 22f;
	private const float BALL_SPEEDUP_MIN = 12f;
	private const float BALL_SPEEDDOWN_MAX = 8f;
	private const float BALL_SPEEDDOWN_MIN = 4f;
	private const float BALL_SPEEDNORMAL_MAX = 15f;
	private const float BALL_SPEEDNORMAL_MIN = 6f;

	private const float PLAYER_SPEED=20f;

	private const float AI_SMOOTH_MOVEMENT=0.14f;
	private const float PLAYER_SMOOTH_MOVEMENT=0.22f;

	private float ORIGINAL_FORCE_MODIFIER = 0.4f;
	private float MAX_FORCE = 400.0f;


	private float BALL_SPEED_MULTIPLIER = 0.05f;
	private float PAD_SPEED_MULTIPLIER = 0.08f;
	private float PAD_FORCE_MULTIPLIER = 0.05f;
	private float PAD_FORCE_MODIFIER_MULTIPLIER = 0.03f;
	private float SMOOTH_MOVEMENT_MULTIPLIER=0.002f;

	public float ballSpeedUpMax;
	public float ballSpeedUpMin;
	public float ballSpeedDownMax;
	public float ballSpeedDownMin;
	public float ballSpeedNormalMax;
	public float ballSpeedNormalMin;

	public float player_speed;
	public float AISmoothMovement;
	public float playerSmoothMovement;
	public float AI_force;
	public float player_force;
	public float AI_force_modifier;
	public float player_force_modifier;
	private int currentPlayingLevel;
	private float initialTime;

	private GameObject[] ballList;
	public GameObject specials,MCD,drunk;

	void Awake(){
		
	}
	void Start(){

		if (SceneManager.GetActiveScene ().name == "Pong_Breaker") {
			BlokeGroup = GameObject.Find ("BlokeGroup").transform;
			currentPlayingLevel = PlayerPrefs.GetInt ("currentPlayingLevel");
			print (currentPlayingLevel);

			LevelMaker (currentPlayingLevel);
			setValues ((float)currentPlayingLevel);		
		} else {
			initialTime = Time.time;
			InvokeRepeating ("variesWithTime", 0.0f, 2f);
		} 


		if (SceneManager.GetActiveScene ().name == "Pong_Breaker" || SceneManager.GetActiveScene ().name == "Pong_Breaker_Endless") {
			if (youdidthistoher.Instance.MCDActive == 1) {
				GameObject.FindGameObjectWithTag ("player").SetActive (false);
				MCD.SetActive (true);
			} else if (youdidthistoher.Instance.DrunkActive == 1) {
				GameObject.FindGameObjectWithTag ("player").SetActive (false);
				drunk.SetActive (true);
			}
		}
		GameObject.Find ("Ground").GetComponent<ScoreManager> ().enabled = true;
		//PlayerPrefs.GetInt ("CampaignLevelReached");
		//ballList=GameObject.Find("Ground").GetComponent<PowerUp>().ballList;
	}
	void variesWithTime(){
		setValues((int)(Time.time-initialTime)/10);

	}

	void setValues(float valueMultiplier){
		BALL_SPEED_MULTIPLIER = 0.05f;
		PAD_SPEED_MULTIPLIER = 0.08f;
		PAD_FORCE_MULTIPLIER = 0.05f;
		PAD_FORCE_MODIFIER_MULTIPLIER = 0.03f;
	 	SMOOTH_MOVEMENT_MULTIPLIER=0.002f;

		BALL_SPEED_MULTIPLIER = BALL_SPEED_MULTIPLIER * valueMultiplier;
		PAD_SPEED_MULTIPLIER = PAD_SPEED_MULTIPLIER * valueMultiplier;
		SMOOTH_MOVEMENT_MULTIPLIER = SMOOTH_MOVEMENT_MULTIPLIER * valueMultiplier;
		PAD_FORCE_MULTIPLIER = PAD_FORCE_MULTIPLIER * valueMultiplier;
		PAD_FORCE_MODIFIER_MULTIPLIER = PAD_FORCE_MODIFIER_MULTIPLIER * valueMultiplier;

		ballSpeedUpMax = BALL_SPEEDUP_MAX + BALL_SPEED_MULTIPLIER;
		ballSpeedUpMin = BALL_SPEEDUP_MIN + BALL_SPEED_MULTIPLIER;
		ballSpeedDownMax = BALL_SPEEDDOWN_MAX + BALL_SPEED_MULTIPLIER;
		ballSpeedDownMin = BALL_SPEEDDOWN_MIN + BALL_SPEED_MULTIPLIER;
		ballSpeedNormalMax = BALL_SPEEDNORMAL_MAX + BALL_SPEED_MULTIPLIER;
		ballSpeedNormalMin = BALL_SPEEDNORMAL_MIN + BALL_SPEED_MULTIPLIER;
		//print (ballSpeedNormalMax+"Speed normal");
		player_speed = PLAYER_SPEED + PAD_SPEED_MULTIPLIER;

		AI_force = MAX_FORCE + PAD_FORCE_MULTIPLIER;
		player_force = MAX_FORCE + PAD_FORCE_MULTIPLIER;

		AI_force_modifier = ORIGINAL_FORCE_MODIFIER + PAD_FORCE_MODIFIER_MULTIPLIER;
		player_force_modifier = ORIGINAL_FORCE_MODIFIER + PAD_FORCE_MODIFIER_MULTIPLIER;

			

		AISmoothMovement = AI_SMOOTH_MOVEMENT + SMOOTH_MOVEMENT_MULTIPLIER;
		playerSmoothMovement = PLAYER_SMOOTH_MOVEMENT + SMOOTH_MOVEMENT_MULTIPLIER;
//		print (AI_speed);

	}

	void LevelMaker(int levelNo){
		string level = PlayerPrefs.GetString ("level" + levelNo);
//		print (level);
		foreach(string blokeName in level.Split('*')){
			string[] blokeValues = blokeName.Split ('-');
			int type,i,j;
			int.TryParse(blokeValues [0],out type);
			int.TryParse(blokeValues [1],out i);
			int.TryParse(blokeValues [2],out j);
			BlokeCd (type,i ,j ).SetActive (true);
		}
		ExtraFeatures ();
	}

	void ExtraFeatures(){
		ballList=GameObject.Find("Ground").GetComponent<PowerUp>().ballList;
		if(currentPlayingLevel%10==0){
			foreach (GameObject ball in ballList) {
				ball.transform.GetChild (3).gameObject.SetActive (true);
				ball.transform.GetChild (7).gameObject.SetActive (true);
			}
			specials.SetActive (true);
			specials.transform.GetChild (0).gameObject.SetActive (true);
		}
	}

	GameObject BlokeCd(int type,int i,int j){

		return	BlokeGroup.Find ("Bloke" + type + "-" + i.ToString () + "X-" + j.ToString () + "Z").gameObject;
	}
}
