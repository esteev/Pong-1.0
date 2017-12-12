using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private const float COIN_PROB=0.3f;
	private const float POWER_UP_PROB=0.12f;
	private const float SPAWN_RATE = 3f;
	private const float BLOKE_MIN_SPAWNX = -3f;
	private const float BLOKE_MAX_SPAWNX = 3f;
	private const float	BLOKE_MIN_SPAWNZ = -5f;
	private const float BLOKE_MAX_SPAWNZ = 5f;
	public int health=70;
	private const float BLOKE_HEIGHT = 1f;
	private const float BLOKE_WIDTH=0.5f;
	private const float BLOKE_HEIGHT_FROM_GROUND = 0.4f;
	private const int TYPE_OF_BLOKE=7;
	private const int CORD_X_MAX = (int)((BLOKE_MAX_SPAWNX - BLOKE_MIN_SPAWNX) / BLOKE_WIDTH) ;
	private const int CORD_Z_MAX=(int)((BLOKE_MAX_SPAWNZ-BLOKE_MIN_SPAWNZ)/BLOKE_HEIGHT)+1;

	private int playerBlokeScore=0, AIBlokeScore=0;
	public bool turn,BlokeHit;
	public Text level;

	public GameObject powerUp,padLong,padShort,bigBall,speedUp,speedDown,flareBall,multiBall,magnetPad,gunPad,VIPBall;
	public GameObject coin,BlockRock, BlockToggle, BlockBlink,BlokeBlast,Bloke1,Bloke2,Bloke3;
	private Transform BlokeGroup;
	public GameObject tempBloke,g,w1,w2,w3,w4;
	public GameObject camera1,camera2,camera3;

	public static bool padLongPurchased,flareBallPurchased,VIPBallPurchased,magnetPadPurchased,gunPadPurchased,bigBallPurchased,multiBallPurchased;
	private float padLongPurchasedTime,flareBallPurchasedTime,VIPBallPurchasedTime,magnetPadPurchasedTime,gunPadPurchasedTime,bigBallPurchasedTime,multiBallPurchasedTime;
	private const float PURCHASED_DURATION=25f;

	public Material invisiMat;

	void Start () {
		////////
		/// 
		AdManager.Instance.HideBanner();
		/// /////
		GOBABY.coinMoveCount=0;
		//youdidthistoher.Instance.loader ();
		if (SceneManager.GetActiveScene ().name == "Pong_Breaker") {
			level.text = "Level No: " + PlayerPrefs.GetInt ("currentPlayingLevel").ToString ();
		}
	
		g.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentGround];
		w1.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentWall];
		w2.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentWall];
		w3.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentWall];
		w4.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentWall];

		switch (youdidthistoher.Instance.currentCameraMode) {

		case 0:
			camera1.SetActive (true);
			camera2.SetActive (false);
			camera3.SetActive (false);

			break;
		case 1:
			camera1.SetActive (false);
			camera2.SetActive (true);
			camera3.SetActive (false);

			break;
		case 2:
			camera1.SetActive (false);
			camera2.SetActive (false);
			camera3.SetActive (true);

			//	camera3.transform.SetParent (PowerUp.currentPlayerPad);
			break;
		}
		turn = true;

		if(SceneManager.GetActiveScene ().name == "Pong_Breaker"){
			BlokeGroup = GameObject.Find ("BlokeGroup").transform;
			InitialBlokes();
		}
		else if(SceneManager.GetActiveScene ().name == "Pong_Breaker_Endless"){
			BlokeGroup = GameObject.Find ("BlokeGroup").transform;
			InitialBlokes();
			InvokeRepeating ("BlokeSpawner", SPAWN_RATE, SPAWN_RATE);
		}
		GetComponent<GameSetter> ().enabled = true;

	}


//Initial creation of blokes
	void InitialBlokes(){
		for (int i = 0; i < CORD_X_MAX; i++) {
			for (int j = 0; j < CORD_Z_MAX; j++) {
				for(int k= 0;k < TYPE_OF_BLOKE;k++){
					GameObject tempObject=null;
					string name="";
				
					switch (k) {
					case 0:
						tempObject = Bloke1;
						name = "Bloke1-";
						break;
					case 1:
						tempObject = Bloke2;
						name = "Bloke2-";
						break;
					case 2:
						tempObject = Bloke3;
						name = "Bloke3-";
						break;
					case 3:
						tempObject = BlockRock;
						name = "Bloke4-";
						break;
					case 4:
						tempObject = BlockToggle;
						name = "Bloke5-";
						break;
					case 5:
						tempObject = BlockBlink;
						name = "Bloke6-";
						break;
					case 6:
						tempObject = BlokeBlast;
						//print ("ho ja bhai");
						name = "Bloke7-";
						break;
					}
					var newBloke =Instantiate (tempObject,new Vector3 (BLOKE_MIN_SPAWNX+i*BLOKE_WIDTH+0.025f, BLOKE_HEIGHT_FROM_GROUND ,BLOKE_MIN_SPAWNZ+j*BLOKE_HEIGHT+0.025f ), Quaternion.identity);
					newBloke.transform.parent = BlokeGroup.transform;
					newBloke.gameObject.SetActive (false);
					newBloke.gameObject.name = name + i.ToString () + "X-" + j.ToString ()+"Z";
					if (k == 4) {
						newBloke.GetComponent<BlockToggle>().invisibleMat=invisiMat;
					}
				}
			}

		}

	}
	GameObject BlokeCd(int type,int i,int j){

		return	BlokeGroup.Find ("Bloke" + type + "-" + i.ToString () + "X-" + j.ToString () + "Z").gameObject;

	}
	void BlokeSpawner() {
		int CordX, CordZ,CordType;
		CordX = Random.Range (0, CORD_X_MAX);
		CordZ = Random.Range (0, CORD_Z_MAX);
		CordType = Random.Range (1, 4);
		if (!BlokeCd(1,CordX,CordZ).activeSelf && !BlokeCd (2, CordX, CordZ).activeSelf && !BlokeCd (3, CordX, CordZ).activeSelf) {
			BlokeGroup.Find ("Bloke" + CordType.ToString () + "-" + CordX.ToString () + "X-" + CordZ.ToString ()+"Z").gameObject.SetActive (true);
		}

	}

	public void SetTurnForBloke(bool turn){
		this.turn=turn;

	}

	// make coins at the bloke places
	public void makeCoin(GameObject tempBloke){
		int chance = Random.Range(0,100);//Random.Range (0f, 1f);

		if (chance < COIN_PROB*100 ) {
			Instantiate (coin, tempBloke.transform.position, Quaternion.Euler (0, 0, 90));
		}
	}

	bool nahiKarSakteCreate (int choice){
		if (Time.time-padLongPurchasedTime<PURCHASED_DURATION&&choice==1) {
			return true;
		}
		if (Time.time-VIPBallPurchasedTime<PURCHASED_DURATION && choice == 5) {
			return true;
		}
		if (Time.time-flareBallPurchasedTime<PURCHASED_DURATION && choice == 9) {
			return true;
		}
		if (Time.time-gunPadPurchasedTime<PURCHASED_DURATION && choice == 8) {
			return true;
		}
		if (Time.time-magnetPadPurchasedTime<PURCHASED_DURATION && choice == 7) {
			return true;
		}


		return false;

	}
	void Update(){

		if (padLongPurchased) {
			padLongPurchasedTime = Time.time;
			padLongPurchased = false;
		}
		if (flareBallPurchased) {
			flareBallPurchasedTime = Time.time;
			flareBallPurchased=  false;;
		}
		if (VIPBallPurchased) {
			VIPBallPurchasedTime = Time.time;
			VIPBallPurchased = false;
		}
		if (magnetPadPurchased) {
			magnetPadPurchasedTime= Time.time;
			magnetPadPurchased = false;
		}
		if (bigBallPurchased) {
			bigBallPurchasedTime = Time.time;
			bigBallPurchased = false;
		}
		if (multiBallPurchased) {
			multiBallPurchasedTime = Time.time;
			multiBallPurchased = false;
		}
	}
	// power up created at bloke places	
	public void makePowerUp(GameObject tempBloke){
		if (youdidthistoher.Instance.MCDActive == 1) {
			return;
		}
		float chance = Random.Range (0f, 1f);
		if (chance < POWER_UP_PROB) {

			int powerChoice = Random.Range (0, 33);
			if (nahiKarSakteCreate (powerChoice)) {
				return;
			}
			if (powerChoice < 4) 		Instantiate (padLong, tempBloke.transform.position, Quaternion.identity);
			else if(powerChoice < 8)	Instantiate (padShort, tempBloke.transform.position, Quaternion.identity);	
			else if(powerChoice < 12)	Instantiate (bigBall, tempBloke.transform.position, Quaternion.identity);
			else if(powerChoice < 16)	Instantiate (speedUp, tempBloke.transform.position, Quaternion.identity);	
			else if(powerChoice < 20)	Instantiate (speedDown, tempBloke.transform.position, Quaternion.identity);	
			else if(powerChoice < 23)	Instantiate (flareBall, tempBloke.transform.position, Quaternion.identity);	
			else if(powerChoice < 26) 	Instantiate (multiBall, tempBloke.transform.position, Quaternion.identity);	
			else if(powerChoice < 29) 	Instantiate (VIPBall, tempBloke.transform.position, Quaternion.identity);
			else if(powerChoice < 31)	Instantiate (gunPad, tempBloke.transform.position, Quaternion.identity);	
			else if(powerChoice < 33)	Instantiate (magnetPad, tempBloke.transform.position, Quaternion.identity);	

		}
	}
}
