using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	private const float NORMAL_BALL_SIZE = 0.75f;
	private const float BIG_BALL_SIZE = 1f;
	private const float BALL_HEIGHT_FROM_GROUND=0.375f;

	private const float NORMAL_PAD_SIZE = 2.5f;
	private const float BIG_PAD_SIZE = 3.5f;
	private const float SMALL_PAD_SIZE=1.8f;

	private const float BLOKE_MIN_SPAWNX = -3f;
	private const float BLOKE_MAX_SPAWNX = 3f;

	private float SPEEDUP_MAX ;
	private float SPEEDUP_MIN ;
	private float SPEEDDOWN_MAX ;
	private float SPEEDDOWN_MIN ;
	private float SPEEDNORMAL_MAX ;
	private float SPEEDNORMAL_MIN ;

	private const float POWER_5_DURATION =5f;				//gun
	private const float POWER_7_DURATION = 7f;			//multi ball,flare ball, vip ball;
	private const float POWER_10_DURATION = 10f;			//pad,bigball,speed ball,magnet
	private const float PURCHASED_DURATION = 24f;

	private const float NORM_Y=0.4f;
	private const float SMOOTH=0.2f;
	private GameObject ball,player,AI;

	//true - player || false - AI
	public Transform BlokeGroup;
	public Transform playerTransform, AITransform; 
	public GameObject ballTransform;

	////for recording duration of time
	private	float[] timePad=new float[2];
	private	float[] timeGunPad=new float[2];
	private	float[] timeMagnetPad=new float[2];
	private float timeBigBall,timeFlareBall,timeMultiBall,timeBallSpeed,timeVIPBall;

	///for checking work is happening or not
	private bool[] workPad=new bool[2];
	private bool[] workGunPad=new bool[2];
	private bool[] workMagnetPad=new bool[2];
	private bool workBigBall,workFlareBall,workMultiBall,workBallSpeed,workVIPBall;
	//for storing starting time of powerUp
	private float[] STimePad=new float[2];
	private float[] STimeGunPad=new float[2];
	private float[] STimeMagnetPad=new float[2];

	private float STimeBigBall,STimeFlareBall,STimeMultiBall,STimeBallSpeed,STimeVIPBall;

	public static bool padLongPurchased,flareBallPurchased,VIPBallPurchased,magnetPadPurchased,gunPadPurchased,bigBallPurchased,multiBallPurchased;
	///for current reference to the active pads
	private Magnet MagnetScript;
	public GameObject playerMagnetPad,AIMagnetPad,playerGunPad,AIGunPad;
	private bool BlockGiraDe;


	public Transform currentPlayerPad, currentAIPad;
	public GameObject[] ballList = new GameObject[3];

	public bool animPlayerPad,animAIPad,animBall,animMultiBall;
	public Vector3 playerPadScale,AIPadScale,BallScale;
	private GameSetter setter;
	private Material[] materials;
	private Transform selectedPlayerPad;
	public GameObject drunkPad;

	// Use this for initialization

	void Start () {
	//	padLongPurchased = true;
		player = GameObject.FindWithTag ("player");
		AI = GameObject.FindWithTag ("AI");
		setter = GameObject.Find ("Ground").GetComponent<GameSetter> ();
		ballList [0] = GameObject.Find ("Ball");
		BlokeGroup = GameObject.Find ("BlokeGroup").transform;
		materials = Resources.LoadAll<Material> ("SpecialMaterials");
		if (youdidthistoher.Instance.DrunkActive == 1) {
			selectedPlayerPad = drunkPad.transform;
		} else {
			selectedPlayerPad = player.transform;
		}
		currentAIPad = AI.transform;
		currentPlayerPad = selectedPlayerPad;

		ballList[1]=Instantiate(ballTransform,new Vector3(0,0.5f,0),Quaternion.identity);
		ballList[2]=Instantiate(ballTransform,new Vector3(0,0.5f,0),Quaternion.identity);
		ballList [1].gameObject.SetActive (false);
		ballList [2].gameObject.SetActive (false);
		currentAIPad.transform.position = new Vector3 (8,0.4f,0);
		currentAIPad.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		currentPlayerPad.transform.position = new Vector3 (-8,0.4f,0);
		currentPlayerPad.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		ballTransform.transform.position = new Vector3 (-7,0.5f,0);
		ballTransform.GetComponent<Rigidbody> ().velocity = Vector3.zero;

	}

	// Update is called once per frame
	void Update () {
		setGame ();
		khareedLiya ();
		PowerUpWorking ();
		BlokeGirao ();
		magnetRelease ();
		BallKoMaterialDo ();
	}
    void BallKoMaterialDo(){
		if ((workVIPBall || workBallSpeed) && (ballList [0].GetComponent<Renderer> ().material == materials [0])) {
			if (workVIPBall) {
				for (int i = 0; i < 3; i++) {
					ballList [i].GetComponent<Renderer> ().material = materials [3];
				}
			}  
			else {
				if (timeBallSpeed > 0) {
					for (int i = 0; i < 3; i++) {
						ballList [i].GetComponent<Renderer> ().material = materials [2];
					}
				}  else {
					for (int i = 0; i < 3; i++) {
						ballList [i].GetComponent<Renderer> ().material = materials [1];
					}
				}

			}
		}
	}

	void setGame(){
		
		SPEEDUP_MAX = setter.ballSpeedUpMax;
		SPEEDUP_MIN = setter.ballSpeedUpMin;
		SPEEDDOWN_MAX = setter.ballSpeedDownMax;
		SPEEDDOWN_MIN = setter.ballSpeedDownMin;
		SPEEDNORMAL_MAX = setter.ballSpeedNormalMax;
		SPEEDNORMAL_MIN = setter.ballSpeedNormalMin;
	}
	void magnetRelease(){
		if (!playerMagnetPad.activeSelf) {
			MagnetScript = playerMagnetPad.GetComponentInChildren<Magnet> ();
			MagnetScript.release ();
		}
		if(!AIMagnetPad.activeSelf) {
			MagnetScript = AIMagnetPad.GetComponentInChildren<Magnet> ();
			MagnetScript.release ();
		}
	}

	void BlokeGirao(){
		if (BlockGiraDe) {
			bool kardo=true;
			for(int i=0;i<3;i++) {
				if (ballList[i].activeSelf && BLOKE_MIN_SPAWNX-0.5f < ballList[i].transform.position.x && ballList[i].transform.position.x< BLOKE_MAX_SPAWNX+0.5f )
					kardo = false;
			}
			if (kardo) {
				foreach (Transform t in BlokeGroup.transform) {
					if (t.gameObject.activeSelf) {
						while (t.position.y != NORM_Y) {
							Vector3 toMove = new Vector3 (t.position.x, NORM_Y, t.position.z);
							t.transform.position = Vector3.MoveTowards (t.transform.position, toMove, SMOOTH);
						}
					}
				}
			}
		}
	}
	void khareedLiya(){
		if(padLongPurchased){
			PUpadLong (true);
			timePad [0] += PURCHASED_DURATION;
			timePad [0] -= POWER_10_DURATION;
			padLongPurchased = false;
		}

		if(flareBallPurchased){
			PUflareBall ();
			timeFlareBall += PURCHASED_DURATION;
			timeFlareBall -= POWER_7_DURATION;
			flareBallPurchased = false;
		}

		if(VIPBallPurchased){
			PUVIPBall ();
			timeVIPBall+= PURCHASED_DURATION;
			timeVIPBall -= POWER_7_DURATION;
			VIPBallPurchased = false;
		}

		if(bigBallPurchased){
			PUbigBall ();
			timeBigBall+= PURCHASED_DURATION;
			timeBigBall -= POWER_10_DURATION;
			bigBallPurchased = false;
		}

		if(multiBallPurchased){
			PUmultiBall ();
			timeMultiBall+= PURCHASED_DURATION;
			timeMultiBall -= POWER_7_DURATION;
			multiBallPurchased = false;
		}

		if(gunPadPurchased){
			PUgunPad (true);
			timeGunPad[0]+= PURCHASED_DURATION;
			timeGunPad [0] -= POWER_5_DURATION;
			gunPadPurchased = false;
		}

		if(magnetPadPurchased){
			PUmagnetPad (true);
			timeMagnetPad[0]+= PURCHASED_DURATION;
			STimeMagnetPad [0] -= POWER_10_DURATION;
			magnetPadPurchased = false;

		}

	}
	// power up adre constantly checked if they are true or not
	// work bool 
	void PowerUpWorking(){

		if (workPad [0]) {
			if (Time.time - STimePad [0] > Mathf.Abs (timePad [0])) {
				workPad [0] = false;
				STimePad [0] = timePad [0] = 0f;
				playerPadScale= new Vector3 (0.5f, 0.5f, NORMAL_PAD_SIZE);
				animPlayerPad = true;

			}
		}

		//workPad[1] for AI pad length
		if (workPad [1]) {
			if (Time.time - STimePad [1] > Mathf.Abs (timePad [1])) {
				workPad [1] = false;
				STimePad [1] = timePad [1] = 0f;
				//currentAIPad.localScale = new Vector3 (0.5f, 0.5f, NORMAL_PAD_SIZE);
				AIPadScale= new Vector3 (0.5f, 0.5f, NORMAL_PAD_SIZE);
				animAIPad = true;
			}
		}
		//workBigBall for ball Size 
		if (workBigBall) {
			if (Time.time - STimeBigBall > timeBigBall) {
				workBigBall = false;
				STimeBigBall = timeBigBall = 0f;
				BallScale=new Vector3 (NORMAL_BALL_SIZE, NORMAL_BALL_SIZE, NORMAL_BALL_SIZE);
				animBall = true;
				/*for (int i = 0; i < 3; i++) {
					ballList[i].transform.localScale = new Vector3 (NORMAL_BALL_SIZE, NORMAL_BALL_SIZE, NORMAL_BALL_SIZE);
				}*/

			}
		}

		//workBallSpeed for ball's speed
		if (workBallSpeed) {
			if (Time.time - STimeBallSpeed > Mathf.Abs (timeBallSpeed)) {
				workBallSpeed = false;
				STimeBallSpeed = timeBallSpeed = 0f;
				for (int i = 0; i < 3; i++) {
					ballList [i].GetComponent<BallS> ().MAX_SPEED = SPEEDNORMAL_MAX;
					ballList [i].GetComponent<BallS> ().MIN_SPEED = SPEEDNORMAL_MIN;	
					ballList [i].transform.GetChild (0).gameObject.SetActive (false);
					ballList [i].transform.GetChild (2).gameObject.SetActive (false);
					ballList [i].GetComponent<Renderer> ().material = materials [0];
				}
			}
		}

		//workFlareBall for making ball a flare
		if (workFlareBall) {
			if (Time.time - STimeFlareBall > timeFlareBall) {
				workFlareBall = false;
				STimeFlareBall = timeFlareBall = 0f;
				foreach (Transform child in BlokeGroup.transform)
				{	child.GetComponent<Collider> ().isTrigger = false;
				}
				for (int i = 0; i < 3; i++) {
					ballList [i].transform.GetChild (1).gameObject.SetActive (false);
				}
			}

		}

		//workMultiBall for generating multiple balls
		if (workMultiBall) {
			if (Time.time - STimeMultiBall > timeMultiBall) {
				workMultiBall = false;
				STimeMultiBall = timeMultiBall = 0f;
				animMultiBall = true;
				//ballList [1].gameObject.SetActive (false);
				//ballList [2].gameObject.SetActive (false);
			}
		}

		//workGunPad[0] for player gun pad
		if (workGunPad [0]) {
			if (Time.time - STimeGunPad [0] > Mathf.Abs (timeGunPad [0])) {
				workGunPad [0] = false;
				STimeGunPad [0] = timeGunPad [0] = 0f;
				currentPlayerPad.gameObject.SetActive (false);
				selectedPlayerPad.position = currentPlayerPad.transform.position;
				selectedPlayerPad.localScale = currentPlayerPad.transform.localScale;
				currentPlayerPad = selectedPlayerPad;
				currentPlayerPad.gameObject.SetActive(true);

			}
		}

		//workGunPad[1] for AI gun pad
		if (workGunPad [1]) {
			if (Time.time - STimeGunPad [1] > Mathf.Abs (timeGunPad [1])) {
				workGunPad [1] = false;
				STimeGunPad [1] = timeGunPad [1] = 0f;
				currentAIPad.gameObject.SetActive (false);
				AI.transform.position = currentAIPad.transform.position;
				AI.transform.localScale = currentAIPad.transform.localScale;
				currentAIPad = AI.transform;
				currentAIPad.gameObject.SetActive(true);
			}
		}

		//workMagnetPad[0] for player Magnet pad
		if (workMagnetPad [0]) {
			if (Time.time - STimeMagnetPad [0] > Mathf.Abs (timeMagnetPad [0])) {
				workMagnetPad [0] = false;
				STimeMagnetPad [0] = timeMagnetPad [0] = 0f;
				currentPlayerPad.gameObject.SetActive (false);
				selectedPlayerPad.position = currentPlayerPad.transform.position;
				selectedPlayerPad.localScale = currentPlayerPad.transform.localScale;
				currentPlayerPad = selectedPlayerPad;
				currentPlayerPad.gameObject.SetActive(true);
			}
		}

		//workMagnetPad[1] for AI Magnet pad
		if (workMagnetPad [1]) {
			if (Time.time - STimeMagnetPad [1] > Mathf.Abs (timeMagnetPad [1])) {
				workMagnetPad [1] = false;
				STimeMagnetPad [1] = timeMagnetPad [1] = 0f;
				currentAIPad.gameObject.SetActive (false);
				AI.transform.position = currentAIPad.transform.position;
				AI.transform.localScale = currentAIPad.transform.localScale;
				currentAIPad = AI.transform;
				currentAIPad.gameObject.SetActive(true);
			}
		}
		if (workVIPBall) {
			if (Time.time - STimeVIPBall > timeVIPBall) {
				workVIPBall = false;
				STimeVIPBall = timeVIPBall = 0f;
				BlockGiraDe = true;
				/*for (int i = 0; i < 3; i++) {
					ballList [i].GetComponent<vipBehaviour> ().enabled = false;
				}*/
				GameObject.Find("Ground").GetComponent<vipBehaviour>().enabled=false;
				for (int i = 0; i < 3; i++) {
					ballList [i].transform.GetChild (4).gameObject.SetActive (false);
					ballList [i].transform.GetChild (5).gameObject.SetActive (false);
					ballList [i].GetComponent<Renderer> ().material = materials [0];
				}


			}
		}

	}


	////////////////////////////////////////////////////////////////////////////////////////////////////////
	//calling of every power up for first time 

	public void PUpadLong(bool turn){
		if (turn) {
			if (workPad [0]) {
				timePad [0] -= Time.time - STimePad [0];
			}
			timePad [0] += POWER_10_DURATION;
			STimePad [0] = Time.time;
			workPad [0] = true;                        
			if (timePad [0] > 0) {
				playerPadScale= new Vector3 (0.5f, 0.5f, BIG_PAD_SIZE);
				animPlayerPad = true;
			} else if (timePad [0] < 0) {
				playerPadScale= new Vector3 (0.5f, 0.5f, SMALL_PAD_SIZE);
				animPlayerPad = true;
			}

		} else {
			if (workPad [1]) {
				timePad [1] -= Time.time - STimePad [1];
			}

			timePad [1] += POWER_10_DURATION;
			STimePad [1] = Time.time;
			workPad [1] = true;                        
			if (timePad [1] > 0) {
				AIPadScale= new Vector3 (0.5f, 0.5f, BIG_PAD_SIZE);
				animAIPad = true;
			} else if (timePad [1] < 0) {
				AIPadScale= new Vector3 (0.5f, 0.5f, SMALL_PAD_SIZE);
				animAIPad = true;
			}
		}
	}
	public void PUpadShort(bool turn){
		if (turn) {
			if (workPad [0]) {
				timePad [0] -= Time.time - STimePad [0];
			}
			timePad [0] -= POWER_10_DURATION;
			STimePad [0] = Time.time;
			workPad [0] = true;                        
			if (timePad [0] > 0) {
				playerPadScale= new Vector3 (0.5f, 0.5f, BIG_PAD_SIZE);
				animPlayerPad = true;
				//currentPlayerPad.localScale = new Vector3 (0.5f, 0.5f, BIG_PAD_SIZE);
			} else if (timePad [0] < 0) {
				playerPadScale= new Vector3 (0.5f, 0.5f, SMALL_PAD_SIZE);
				animPlayerPad = true;
				//currentPlayerPad.localScale = new Vector3 (0.5f, 0.5f, SMALL_PAD_SIZE);
			}

		} else {
			if (workPad [1]) {
				timePad [1] -= Time.time - STimePad [1];
			}
			timePad [1] -= POWER_10_DURATION;
			STimePad [1] = Time.time;
			workPad [1] = true;                        //AI pad short
			if (timePad [1] > 0) {
				AIPadScale= new Vector3 (0.5f, 0.5f, BIG_PAD_SIZE);
				animAIPad = true;
				//currentAIPad.localScale = new Vector3 (0.5f, 0.5f, BIG_PAD_SIZE);
			} else if (timePad [1] < 0) {
				//currentAIPad.localScale = new Vector3 (0.5f, 0.5f, SMALL_PAD_SIZE);
				AIPadScale= new Vector3 (0.5f, 0.5f, SMALL_PAD_SIZE);
				animAIPad = true;
			}
		}
	}

 	public void PUbigBall(){
		if (workBigBall) {
			timeBigBall -= Time.time - STimeBigBall;
		}
		timeBigBall += POWER_10_DURATION;
		STimeBigBall = Time.time;
		workBigBall = true;

		BallScale=new Vector3 (BIG_BALL_SIZE, BIG_BALL_SIZE, BIG_BALL_SIZE);
		animBall = true;
		/*for (int i = 0; i < 3; i++) {
			ballList[i].transform.localScale = new Vector3 (BIG_BALL_SIZE, BIG_BALL_SIZE, BIG_BALL_SIZE);
		}*/
	}

	public void PUspeedUp(){
		if (workBallSpeed) {
			timeBallSpeed -= Time.time - STimeBallSpeed;
		}
		timeBallSpeed += POWER_10_DURATION;
		STimeBallSpeed = Time.time;
		if (timeBallSpeed > 0) {
			for (int i = 0; i < 3; i++) {
				ballList [i].GetComponent<BallS> ().MAX_SPEED = SPEEDUP_MAX;
				ballList [i].GetComponent<BallS> ().MIN_SPEED = SPEEDUP_MIN;	
				ballList [i].transform.GetChild (0).gameObject.SetActive (true);
				ballList [i].GetComponent<Renderer> ().material = materials [2];
			}
		} else if (timeBallSpeed < 0) {
			for (int i = 0; i < 3; i++) {
				ballList [i].GetComponent<BallS> ().MAX_SPEED = SPEEDDOWN_MAX;
				ballList [i].GetComponent<BallS> ().MIN_SPEED = SPEEDDOWN_MIN;	
				ballList [i].transform.GetChild (2).gameObject.SetActive (true);
				ballList [i].GetComponent<Renderer> ().material = materials [1];
			}

		}
		workBallSpeed = true;
	}

	public void PUspeedDown(){
		if (workBallSpeed) {
			timeBallSpeed -= Time.time - STimeBallSpeed;
		}
		timeBallSpeed -= POWER_10_DURATION;
		STimeBallSpeed = Time.time;
		if (timeBallSpeed > 0) {
			for (int i = 0; i < 3; i++) {
				ballList [i].GetComponent<BallS> ().MAX_SPEED = SPEEDUP_MAX;
				ballList [i].GetComponent<BallS> ().MIN_SPEED = SPEEDUP_MIN;
				ballList [i].transform.GetChild (0).gameObject.SetActive (true);
				ballList [i].GetComponent<Renderer> ().material = materials [2];
			}
		} else if (timeBallSpeed < 0) {
			for (int i = 0; i < 3; i++) {
				ballList [i].GetComponent<BallS> ().MAX_SPEED = SPEEDDOWN_MAX;
				ballList [i].GetComponent<BallS> ().MIN_SPEED = SPEEDDOWN_MIN;	
				ballList [i].transform.GetChild (2).gameObject.SetActive (true);
				ballList [i].GetComponent<Renderer> ().material = materials [1];
			}
		}
		workBallSpeed = true;

	}

	public void PUflareBall(){
		if (workFlareBall) {
			timeFlareBall -= Time.time - STimeFlareBall;
		}
		foreach (Transform child in BlokeGroup.transform)
		{	child.GetComponent<Collider> ().isTrigger = true;
		}
		for (int i = 0; i < 3; i++) {
			ballList [i].transform.GetChild (1).gameObject.SetActive (true);
		}

		timeFlareBall += POWER_7_DURATION;
		STimeFlareBall = Time.time;
		workFlareBall = true;
	}

	public void PUmultiBall(){
		if (workMultiBall) {
			timeMultiBall -= Time.time - STimeMultiBall;
		}
		ballList [1].gameObject.SetActive (true);
		ballList [2].gameObject.SetActive (true);
		if (!workMultiBall) {
			ballList [1].transform.localScale = ballList [0].transform.localScale;
			ballList [2].transform.localScale = ballList [0].transform.localScale;

			ballList [1].transform.position = ballList [0].transform.position;
			ballList [2].transform.position = ballList [0].transform.position;

			ballList [1].GetComponent<Rigidbody> ().velocity = ballList [0].GetComponent<Rigidbody> ().velocity;
			ballList [2].GetComponent<Rigidbody> ().velocity = ballList [0].GetComponent<Rigidbody> ().velocity;
		}
		timeMultiBall += POWER_7_DURATION;
		STimeMultiBall = Time.time;
		workMultiBall = true;

	}
	public void PUgunPad(bool turn){
		if (turn) {
			if (workGunPad [0]) {
				timeGunPad [0] -= Time.time - STimeGunPad [0];
			}
			timeGunPad [0] += POWER_5_DURATION;
			STimeGunPad [0] = Time.time;
			workGunPad [0] = true;		

			currentPlayerPad.gameObject.SetActive (false);
			playerGunPad.transform.position = currentPlayerPad.transform.position;
			playerGunPad.transform.localScale = currentPlayerPad.transform.localScale;
			currentPlayerPad = playerGunPad.transform;
			currentPlayerPad.gameObject.SetActive (true);

		} else {
			if (workGunPad [1]) {
				timeGunPad [1] -= Time.time - STimeGunPad [1];
			}

			timeGunPad [1] += POWER_5_DURATION;
			STimeGunPad [1] = Time.time;
			workGunPad [1] = true;						

			currentAIPad.gameObject.SetActive (false);
			AIGunPad.transform.position = currentAIPad.transform.position;
			AIGunPad.transform.localScale = currentAIPad.transform.localScale;
			currentAIPad = AIGunPad.transform;
			currentAIPad.gameObject.SetActive (true);
		}
	}

	public void PUmagnetPad(bool turn){
		if (turn) {
			if (workMagnetPad [0]) {
				timeMagnetPad [0] -= Time.time - STimeMagnetPad [0];
			}
			timeMagnetPad [0] += POWER_10_DURATION;
			STimeMagnetPad [0] = Time.time;
			workMagnetPad [0] = true;

			currentPlayerPad.gameObject.SetActive (false);
			playerMagnetPad.transform.position = currentPlayerPad.transform.position;
			playerMagnetPad.transform.localScale = currentPlayerPad.transform.localScale;
			currentPlayerPad = playerMagnetPad.transform;
			currentPlayerPad.gameObject.SetActive (true);

		} else {
			if (workMagnetPad [1]) {
				timeMagnetPad [1] -= Time.time - STimeMagnetPad [1];
			}

			timeMagnetPad [1] += POWER_10_DURATION;
			STimeMagnetPad [1] = Time.time;
			workMagnetPad [1] = true;

			currentAIPad.gameObject.SetActive (false);
			AIMagnetPad.transform.position = currentAIPad.transform.position;
			AIMagnetPad.transform.localScale = currentAIPad.transform.localScale;
			currentAIPad = AIMagnetPad.transform;
			currentAIPad.gameObject.SetActive (true);

		}
	}
	public void PUVIPBall(){
		if (workVIPBall) {
			timeVIPBall -= Time.time - STimeVIPBall;
		}

		GameObject.Find("Ground").GetComponent<vipBehaviour>().enabled=true;
		timeVIPBall += POWER_7_DURATION;
		STimeVIPBall = Time.time;
		workVIPBall = true;
		for (int i = 0; i < 3; i++) {
			ballList [i].transform.GetChild (4).gameObject.SetActive (true);
			ballList [i].transform.GetChild (5).gameObject.SetActive (true);

			ballList [i].GetComponent<Renderer> ().material = materials [3];
		}

	}

}
