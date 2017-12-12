using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class padUdaDe : MonoBehaviour {

	private const float CPU_CONFUSION_FORCE = 3.0f;
	private GameObject ground;
	private const float FORCE_MULTIPLIER = 300.0f;
	private GameManager GMScript;
	void Start(){
		ground = GameObject.Find ("Ground");
		GMScript = ground.GetComponent<GameManager> ();
		Invoke ("destroy", 3.0f);
	}

	void Update()
	{
		if (transform.position.y < 0)
			Destroy (this.gameObject);
		if (transform.position.y > 5)
		{
			this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range (0, 1), -Random.Range (1, 5), Random.Range (0, 1)) * FORCE_MULTIPLIER);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		//	print (col.gameObject.name);
		if (col.gameObject.CompareTag ("Bloke")) {
			Destroy (this.gameObject);
			GMScript.SetTurnForBloke(true);
			GMScript.makeCoin (col.gameObject);
			if (col.gameObject.name.Contains ("Bloke7")) {
				GameObject.FindGameObjectWithTag("Ball").SendMessage ("Blast", col.gameObject, SendMessageOptions.DontRequireReceiver);
			}
			ground.SendMessage ("BlokePoint", true, SendMessageOptions.DontRequireReceiver);
			col.gameObject.SetActive (false);

		} 
		//else if (col.gameObject.name.Contains ("CPU")) {
		//	col.GetComponent<Rigidbody> ().AddForce (CPU_CONFUSION_FORCE, 0, 0);
		//	Destroy (this.gameObject);
		//}
		/*else if (col.gameObject.name.Contains ("EastWall")||col.gameObject.CompareTag("AI")||col.gameObject.CompareTag("player")) {
			Destroy (this.gameObject);
		}
*/
		//else if (col.gameObject.name.Contains ("CPU")) {
		//	col.GetComponent<Rigidbody> ().AddForce (CPU_CONFUSION_FORCE, 0, 0);
		//	Destroy (this.gameObject);
		//}
	//	else if (col.gameObject.name.Contains ("Wall")||col.gameObject.name.Contains("MCD")) {
	//		Invoke ("destroy", 5.0f);
	//	}
	}

	void destroy()
	{
		Destroy (this.gameObject);
	}
}
