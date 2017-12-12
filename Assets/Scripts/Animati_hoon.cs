using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animati_hoon : MonoBehaviour {

	public GameObject empty;
	public GameObject ground;
	private Animator anim;

	void Start () {
		GameObject emp = Instantiate (empty, ground.GetComponent<PowerUp> ().currentPlayerPad.transform);
		ground.GetComponent<PowerUp> ().transform.SetParent (emp.transform);
		anim = empty.GetComponent<Animator> ();
	}

	void Update () {
		
	}

	public void backFlip()
	{
		anim.SetInteger ("Trick", 1);
	}
}
