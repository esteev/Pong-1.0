using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class so : MonoBehaviour {

	public AudioSource a;

	void Start () {
	
		if (youdidthistoher.Instance.soundOn == 1) {
			AudioListener.volume = 1f;
		} else {
			AudioListener.volume = 0f;
		}

		if (youdidthistoher.Instance.inGameSond == 1) {
			a.enabled = true;
		} else {
			a.enabled = false;
		}
	}
	
	void Update () {
		
	}
}
