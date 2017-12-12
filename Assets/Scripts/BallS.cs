using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallS : MonoBehaviour {
	private const float NORMAL_BALL_SIZE = 0.75f;
	private const float BLOKE_MIN_SPAWNX = -3f;
	private const float BLOKE_MAX_SPAWNX = 3f;
	private const float	BLOKE_MIN_SPAWNZ = -5f;
	private const float BLOKE_MAX_SPAWNZ = 5f;
	private const float BLOKE_HEIGHT = 1f;
	private const float BLOKE_WIDTH=0.5f;
	private const float ballForce = 100f;

	private const float SPEED_CONTROL_THRUST=50f;
	private const float POWER_UP_DURATION =5f;
	private const float BLOKE_RESPAWN_TIME=0.00f;

	private const int TYPE_OF_BLOKE=7;
	private const int CORD_X_MAX = (int)((BLOKE_MAX_SPAWNX - BLOKE_MIN_SPAWNX) / BLOKE_WIDTH) ;
	private const int CORD_Z_MAX=(int)((BLOKE_MAX_SPAWNZ-BLOKE_MIN_SPAWNZ)/BLOKE_HEIGHT)+1;

	public float MIN_SPEED=8f;
	public float MAX_SPEED=20f;

	private const float CORRECTION_ANGLE=5f;
	private const float CORRECTION_RATIO=4f;
	private const float BRING_POINTX = -7f;
	public float velocityStrength,maxValue=0;

	public GameObject ground;
	public GameObject tempBloke;

	private Rigidbody ballRig;
	private Vector3 direction;
	private ScoreManager SM ;

	public Transform BlokeGroup;
	public Transform HitBloke;					//for passing bloke transform
	public Transform tempHitBloke;//			//for reference temp bloke
	public Transform lastPad,lastWall;

	public bool notMoving;
	public bool turn,BlokeHit;  //bool
	private bool sameBloke=false;
	private List  <GameObject>  BlastList; 
	public GameObject BlastAnim;

	public AudioSource a;
	public AudioClip a1,a2,a3,a4;

	public int HitBlokeX,HitBlokeZ,HitBlokeType;

	public Anim animScript;
	private GameManager GMScript;
	private bool done = false;
	void Start()
	{	ground = GameObject.Find ("Ground");
		tempHitBloke = GameObject.Find ("BlokeTemp").transform;
		HitBloke =  GameObject.Find ("BlokeTemp").transform;
		ballRig = GetComponent<Rigidbody> ();
		BlokeGroup = GameObject.Find ("BlokeGroup").transform;
		GMScript = ground.GetComponent<GameManager>();
		animScript = ground.GetComponent<Anim> ();
	//	direction = Vector3.left*ballForce;
	//	ballRig.AddForce(direction);
		turn = true;
		ballRig.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;// heavy duty physics
		SM = ground.GetComponent<ScoreManager> ();
		BlastList = new List<GameObject> ();
		lastWall = GameObject.Find("EastWall").transform;

		//

	}

	void Update(){
		
		if(ground.GetComponent<GameSetter> ().ballSpeedNormalMin!=0&&!done){
			done = true;
		//	print ("chlo");
			MIN_SPEED = ground.GetComponent<GameSetter> ().ballSpeedNormalMin;
			MAX_SPEED = ground.GetComponent<GameSetter> ().ballSpeedNormalMax;

		}
//		print (MIN_SPEED + "   " + MAX_SPEED);

		HitChecker ();

		EmptyBlastList ();
	}

	void HitChecker(){
		if (BlokeHit) {
			BlockIsHit ();
		}
	}

	void FixedUpdate()
	{
		velocityStrength = ballRig.velocity.magnitude;
		if (velocityStrength < MIN_SPEED) {
			ballRig.AddForce (ballRig.velocity.normalized * SPEED_CONTROL_THRUST);
		}
		if (velocityStrength > MAX_SPEED) {
			ballRig.AddForce (ballRig.velocity.normalized * SPEED_CONTROL_THRUST*-1);

		}
		if (velocityStrength > maxValue)
			maxValue = velocityStrength;


	}


	void OnTriggerEnter(Collider col){
		if (col.gameObject.name.Contains ("Bloke"))
			a.PlayOneShot (a1,0.2f);
		if (col.gameObject.CompareTag ("Bloke")) {
			

			HitBloke = col.gameObject.transform;
			//BlokeHit = true;
			//			BlockIsHit ();
			GMScript.SetTurnForBloke(turn);
			GMScript.makePowerUp (HitBloke.gameObject);
			if (turn) {
				GMScript.makeCoin (HitBloke.gameObject);
			}
			SM.BlokePoint (turn);
			if (col.gameObject.name.Contains ("Bloke7")) {
				Blast (col.gameObject);
			} 
			col.gameObject.SetActive (false);

		}
	}


	void OnCollisionEnter(Collision col)
	{	
		if (col.gameObject.CompareTag ("Bloke"))
			a.PlayOneShot (a1,0.2f);
		else if (col.gameObject.CompareTag ("Block"))
			a.PlayOneShot (a2,0.2f);
		
		if ((col.gameObject.name == "NorthWall" || col.gameObject.name == "SouthWall")) {
			
			foreach (ContactPoint contact in col.contacts) {
				if (Mathf.Abs (Vector3.Angle (ballRig.velocity.normalized, Vector3.forward)) < CORRECTION_ANGLE) {
				//print ("abhi bhi hota hai");
					if (transform.position.x > BRING_POINTX) {
						ballRig.velocity = new Vector3 (-ballRig.velocity.z / CORRECTION_RATIO, 0, ballRig.velocity.z);
					} else {
						ballRig.velocity = new Vector3 (ballRig.velocity.z / CORRECTION_RATIO, 0, ballRig.velocity.z);

					}
				}

			}
		}
		if (col.gameObject.CompareTag ("player")) {
			turn = true;

		} else if (col.gameObject.CompareTag ("AI")) {
			turn = false;

		}
		if ((col.gameObject.name == "EastWall")) {			//wall behind AI
			SM.AI_WallPoint++;
			lastWall = col.transform;
			a.PlayOneShot (a3,0.5f);

		} else if (col.gameObject.name == "WestWall") {
			//wall behind player
			SM.player_WallPoint++;
			lastWall = col.transform;
			a.PlayOneShot (a3,0.5f);
		} else if (col.gameObject.CompareTag ("player")) {
			a.PlayOneShot (a4,0.6f);
			if (SceneManager.GetActiveScene ().name == "Pong_Breaker_Practice") {
				if (lastWall == GameObject.Find ("EastWall").transform && lastPad != col.gameObject.transform) {
					SM.player_PadPoint++;
				} else if (lastPad != col.gameObject.transform) {
					SM.player_PadPoint++;
				}
			} else {
			//	print (lastWall);
				if (lastWall == GameObject.Find ("EastWall").transform) {
				//	print (lastWall+"2");

					SM.player_PadPoint++;
				}
			}
			lastPad = col.gameObject.transform;
		} else if (col.gameObject.CompareTag ("AI")) {
			a.PlayOneShot (a4,0.6f);
			if (SceneManager.GetActiveScene ().name == "Pong_Breaker_Practice") {
				if (lastWall == GameObject.Find ("WestWall").transform && lastPad != col.gameObject.transform) {
					SM.AI_PadPoint++;
				} else if (lastPad != col.gameObject.transform) {
					SM.AI_PadPoint++;
				}
			} else {
				if (lastWall == GameObject.Find ("WestWall").transform) {
					SM.AI_PadPoint++;					
				}
			}
			lastPad = col.gameObject.transform;

		} else if (col.gameObject.CompareTag ("Bloke")) {//
			BlokeHit = true;
			HitBloke = col.gameObject.transform;
		}



	}

	GameObject BlokeCd(int type,int i,int j){
		return BlokeGroup.Find ("Bloke" + type + "-" + i.ToString () + "X-" + j.ToString () + "Z").gameObject;

	}

	void Blast(GameObject blokeTemp){
		//print ("shayad");
		int CordX = (int)((blokeTemp.transform.position.x - BLOKE_MIN_SPAWNX) / BLOKE_WIDTH);
		int CordZ = (int)((blokeTemp.transform.position.z - BLOKE_MIN_SPAWNZ) / BLOKE_HEIGHT);
		int CordType;
		string name = blokeTemp.gameObject.name;
		CordType = int.Parse (name [5]+"");


		if (CordX != 0) {
			addBlokeToBlast(CordX-1,CordZ);
		}
		if (CordX != CORD_X_MAX) {    
			addBlokeToBlast (CordX + 1, CordZ);

		}
		if (CordZ != 0) {
			addBlokeToBlast (CordX, CordZ - 1);
		}
		if (CordZ != CORD_Z_MAX) {    
			addBlokeToBlast (CordX, CordZ + 1);
		}
		if (CordX != 0 && CordZ != 0) {
			addBlokeToBlast (CordX-1, CordZ - 1);
		}
		if (CordX != 0 && CordZ != CORD_Z_MAX) {
			addBlokeToBlast (CordX-1, CordZ + 1);
		}
		if (CordX != CORD_X_MAX && CordZ != 0) {
			addBlokeToBlast (CordX+1, CordZ - 1);
		}
		if (CordX != CORD_X_MAX && CordZ != CORD_Z_MAX) {
			addBlokeToBlast (CordX+1, CordZ + 1);
		}
		//		print (BlastList.Count);
	}
	void addBlokeToBlast(int CordX,int CordZ){
		int CordType=FindCordType (CordX, CordZ);
		if (CordType == 0)
			return;
		GameObject temp=BlokeCd(CordType,CordX,CordZ);
		if (!BlastList.Contains (temp)) {
			BlastList.Add (temp);
		}
	} 

	void EmptyBlastList(){
		if (BlastList.Count > 0) {
			//	print ("to");
			GameObject temp = BlastList [0];
			if(BlastList.Remove (BlastList [0])){
				//temp.setActive (false);

				int CordX = (int)((temp.transform.position.x - BLOKE_MIN_SPAWNX) / BLOKE_WIDTH);
				int CordZ = (int)((temp.transform.position.z - BLOKE_MIN_SPAWNZ) / BLOKE_HEIGHT);
				int CordType;
				string name = temp.name;
				CordType = int.Parse (name [5]+"");
				removeBloke (CordX, CordZ, CordType, turn); //turn check

				//				temp.SetActive(false);
				if (temp.name [5] == '7')
					Blast (temp);

			}

		}
	}

	int FindCordType(int CordX,int CordZ){
		if (CordX == CORD_X_MAX || CordZ == CORD_Z_MAX) {
			return 0;

		}
		int CordType=0 ;
		for (int i = 1; i <= TYPE_OF_BLOKE; i++) {
			if (BlokeCd(i, CordX, CordZ).activeSelf) {
				CordType = i;
				break;
			}
		}
		return CordType;
	}

	void BlockIsHit(){
		int CordX = (int)((HitBloke.position.x - BLOKE_MIN_SPAWNX) / BLOKE_WIDTH);
		int CordZ = (int)((HitBloke.position.z - BLOKE_MIN_SPAWNZ) / BLOKE_HEIGHT);
		int CordType;
		string name = HitBloke.name;
		CordType = int.Parse (name [5]+"");
		if (this.transform.localScale.z == NORMAL_BALL_SIZE) {
			removeBloke (CordX, CordZ, CordType, turn);
		} 
		else {

			removeADJBloke (CordX, CordZ, CordType, turn);
		}

		////////////////////
		BlokeHit = false;
		HitBloke = null;


	}

	void removeADJBloke(int CordX,int CordZ,int CordType,bool turn){
		removeBloke (CordX, CordZ, CordType, turn);

		if (CordX != 0) {
			removeBloke (CordX - 1, CordZ, FindCordType (CordX - 1, CordZ), turn);
		}
		if (CordX != CORD_X_MAX) {
			removeBloke (CordX + 1, CordZ, FindCordType (CordX + 1, CordZ), turn);
		}
		if (CordZ != 0) {
			removeBloke (CordX, CordZ - 1, FindCordType (CordX, CordZ - 1), turn);
		}
		if (CordZ != CORD_Z_MAX) {    
			removeBloke (CordX, CordZ + 1, FindCordType (CordX, CordZ + 1), turn);
		}
	}


	void removeBloke(int CordX,int CordZ,int CordType,bool turn){
		if (CordType == 0)
			return;
		GameObject HitBloke = BlokeCd (CordType, CordX, CordZ);

		if (turn) {
			//	print ("player balle balle");
			//	playerBlokeScore++;
			//  length	//		playerTransform.localScale = new Vector3(playerTransform.localScale.x,playerTransform.localScale.y,playerTransform.localScale.z + 0.2f);

		} else {
			//	AIBlokeScore++;
			//	print ("AI balle balle");
		}
		if (CordType == 3) {
			BlokeGroup.Find ("Bloke" + 2 + "-" + CordX.ToString () + "X-" + CordZ.ToString () + "Z").gameObject.SetActive (true);

		} else if (CordType == 2) {
			BlokeGroup.Find ("Bloke" + 1 + "-" + CordX.ToString () + "X-" + CordZ.ToString () + "Z").gameObject.SetActive (true);
		} else if(CordType==1 || CordType ==7) {
			
			SM.BlokePoint (turn);
		}
		if (CordType <= 3) {
			GMScript.SetTurnForBloke(turn);
			GMScript.makePowerUp (HitBloke.gameObject);
//			print ("sikka"+turn);

			if (turn) {
				GMScript.makeCoin (HitBloke.gameObject);
			}
			HitBloke.SetActive (false);
		}
		if (CordType == 7) {
			HitBloke.SetActive (false);
			Instantiate (BlastAnim, HitBloke.transform.position, Quaternion.identity);
			Blast (HitBloke.gameObject);
		}
		if (CordType ==1) {
			ground.GetComponent<Anim>().disapperBloke (HitBloke.transform);			
		}


	}
	/*
	void onClick(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}*/	
}