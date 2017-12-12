using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goliUdaDe : MonoBehaviour {

	private const float CPU_CONFUSION_FORCE = 3.0f;
	private GameObject ground,ball;
	private Transform BlokeGroup;
	private bool turn;

	private const float BLOKE_MIN_SPAWNX = -3f;
	private const float BLOKE_MAX_SPAWNX = 3f;
	private const float	BLOKE_MIN_SPAWNZ = -5f;
	private const float BLOKE_MAX_SPAWNZ = 5f;
	private const float BLOKE_HEIGHT = 1f;
	private const float BLOKE_WIDTH=0.5f;

	private const int TYPE_OF_BLOKE=7;
	private const int CORD_X_MAX = (int)((BLOKE_MAX_SPAWNX - BLOKE_MIN_SPAWNX) / BLOKE_WIDTH) ;
	private const int CORD_Z_MAX=(int)((BLOKE_MAX_SPAWNZ-BLOKE_MIN_SPAWNZ)/BLOKE_HEIGHT)+1;

	private GameManager GMScript;
	void Start(){
		ground = GameObject.Find ("Ground");
		GMScript = ground.GetComponent<GameManager> ();
		BlokeGroup = GameObject.Find ("BlokeGroup").transform;
		ball = GameObject.Find ("Ball");

		if (gameObject.name.Contains ("player")) {
			turn = true;
		} else {
			turn = false;
		}
	}
	void OnTriggerEnter(Collider col)
	{
	//	print (col.gameObject.name);
		if (col.gameObject.CompareTag ("Bloke")) {
			Destroy (this.gameObject);
			GMScript.SetTurnForBloke(turn);
			GMScript.makePowerUp (col.gameObject);
			if (turn) {
				GMScript.makeCoin (col.gameObject);
			}
			if (col.gameObject.name.Contains ("Bloke7")) {
				ball.SendMessage ("Blast", col.gameObject, SendMessageOptions.DontRequireReceiver);
			}
			ground.SendMessage ("BlokePoint", turn, SendMessageOptions.DontRequireReceiver);
			col.gameObject.SetActive (false);

		} 
		//else if (col.gameObject.name.Contains ("CPU")) {
		//	col.GetComponent<Rigidbody> ().AddForce (CPU_CONFUSION_FORCE, 0, 0);
		//	Destroy (this.gameObject);
		//}
		else if (col.gameObject.name.Contains ("Wall")||col.gameObject.CompareTag("AI")||col.gameObject.CompareTag("player")) {
			Destroy (this.gameObject);
		}

	}

}
