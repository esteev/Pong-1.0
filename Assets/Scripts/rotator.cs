using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class rotator : MonoBehaviour {
	private GameObject ball;
	private float dirX;
	public bool turn,collect;
	private GameObject player;
	private GameObject AI;
	private int playerPad;
	private int AIPad;
	private GameObject ground;
	private PowerUp PUScript;
	void Start () {
		//collect = false;
		ground = GameObject.Find ("Ground");
			GameManager GMscript = ground.GetComponent<GameManager> ();
			turn = GMscript.turn;
		PUScript = ground.GetComponent<PowerUp> ();
		if (turn) {
			dirX = -1f;
		} else {
			dirX = 1f;
		};
	
	}
	void BulletTurnSetter(GameObject tempBloke){
		if (tempBloke.name.Contains ("player")) {
			dirX = -1f;
		} else if (tempBloke.name.Contains ("AI")) {
			dirX = 1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
		transform.position=new Vector3(transform.position.x+Time.deltaTime*dirX,transform.position.y,transform.position.z);
	
	}
	void OnTriggerEnter(Collider other){
		 
		if ((other.gameObject.CompareTag ("player") && turn) || (other.gameObject.CompareTag ("AI") && !turn)) {

			if (this.gameObject.name.Contains ("PadLong")) {
				PUScript.PUpadLong (turn);
			} else if (this.gameObject.name.Contains ("PadShort")) {
				PUScript.PUpadShort (turn);
			} else if (this.gameObject.name.Contains ("BigBall")) {
				PUScript.PUbigBall ();
			} else if (this.gameObject.name.Contains ("SpeedUp")) {
				PUScript.PUspeedUp();
			} else if (this.gameObject.name.Contains ("SpeedDown")) {
				PUScript.PUspeedDown ();
			} else if (this.gameObject.name.Contains ("FlareBall")) {
				PUScript.PUflareBall ();
			} else if (this.gameObject.name.Contains ("MultiBall")) {
				PUScript.PUmultiBall ();
			} else if (this.gameObject.name.Contains ("GunPad")) {
				PUScript.PUgunPad (turn);
			} else if (this.gameObject.name.Contains ("MagnetPad")) {
				PUScript.PUmagnetPad (turn);
			} else if (this.gameObject.name.Contains ("VIPBall")) {
				PUScript.PUVIPBall ();
			}
		Destroy (this.gameObject);
		}
		if (other.gameObject.name == "EastWall"||other.gameObject.name == "WestWall") {
			Destroy (this.gameObject);

		}
	}
}
