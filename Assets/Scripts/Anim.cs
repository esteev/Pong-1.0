using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Anim : MonoBehaviour {
	
	private PowerUp PUScript;
	private bool animBloke;
	private List<Transform> BlokeList;
	public Transform AnimBloke;
	void Start () {
		PUScript = GameObject.Find ("Ground").GetComponent<PowerUp> ();
		BlokeList = new List<Transform> ();
	}

	void Update () {

		if(PUScript.animPlayerPad){
			changeSize (PUScript.currentPlayerPad, PUScript.playerPadScale,0.01f);
			if (PUScript.playerPadScale == PUScript.currentPlayerPad.localScale) {
				PUScript.animPlayerPad = false;
			}
		}
		if (PUScript.animAIPad) {
			changeSize (PUScript.currentAIPad, PUScript.AIPadScale,0.01f);
			if (PUScript.AIPadScale == PUScript.currentAIPad.localScale) {
				PUScript.animAIPad = false;
			}
		}

		if (PUScript.animBall) {
			bool check=true;

			foreach (GameObject ball in PUScript.ballList) {
				
				changeSize (ball.transform, PUScript.BallScale,0.01f);
				/*if (ball.transform.parent.name.Contains ("MagContainer")) {
					float size = ball.transform.localScale.x;
					Vector3 temp=new Vector3(1/size,1/size,1/size);
					changeSize (ball.transform.parent, temp,0.01f);

				}*/
				if (ball.transform.localScale != PUScript.BallScale)
					check = false;
			}
			if (check) {
				PUScript.animBall = false;
			}
		}
		if (PUScript.animMultiBall) {
			bool check=true;
			for (int i=1;i< PUScript.ballList.Length;i++) {
				changeSize (PUScript.ballList[i].transform, Vector3.zero,0.02f);

				if (PUScript.ballList [i].transform.localScale != Vector3.zero) {
					check = false;
				}
			}
			if (check) {
				PUScript.animMultiBall = false;
				PUScript.ballList [1].SetActive (false);
				PUScript.ballList [2].SetActive (false);
			}
		}

	}

	void changeSize(Transform pad, Vector3 padLength,float smooth){

		pad.transform.localScale = Vector3.MoveTowards (pad.localScale, padLength,smooth);
		
	}

	public void disapperBloke(Transform bloke){
		var temp = Instantiate (AnimBloke,bloke.transform.position,Quaternion.identity);
		temp.GetComponent<AnimBloke> ().enabled = true;
	
	}

}
