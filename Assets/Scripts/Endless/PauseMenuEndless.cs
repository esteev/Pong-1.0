using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuEndless : MonoBehaviour {

	private bool paused = false;

	public GameObject pauseMenu;
	public GameObject panel;
	public GameObject joystick;
	public GameObject confirmScreen;
	public Button soundButt, osoundButt;
	public GameObject a;
	void Start()
	{
		if (youdidthistoher.Instance.soundOn == 1) {
			soundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.pressedSound;
		} else {
			soundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.unpressedSound;
		}

		if (youdidthistoher.Instance.inGameSond == 1) {
			osoundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.pressedoSound;
		} else {
			osoundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.unpressedoSound;
		}
	}

	public void unpause()
	{	Time.timeScale = 1.0f;
		//////
		AdManager.Instance.HideBanner();
		//////

		paused = false;
		pauseMenu.SetActive (false);
		panel.SetActive (true);
		if (PlayerPrefs.GetInt ("InputType")==1||PlayerPrefs.GetInt ("InputType")==2)
			joystick.SetActive (false);
	}

	public void pause()
	{	//////
		AdManager.Instance.ShowBanner();
		/////
		//print ("Adsa");
		Time.timeScale = 0.0f;
		paused = true;
		pauseMenu.SetActive (true);
		pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
		pauseMenu.transform.GetChild(1).gameObject.SetActive(false);
		panel.SetActive (false);
	}

	public void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (paused == false) {
				pause ();
			} else {
				if (confirmScreen.activeInHierarchy)
					actionConfirmed ();
				else
					Back ();
			}
		}
	}
	public void Back()
	{
	//	print ("Adsa");
		Time.timeScale = 0.0f;
		//are you sure
		pauseMenu.SetActive(true);
		pauseMenu.transform.GetChild(0).gameObject.SetActive(false);
		confirmScreen.SetActive(true);
		panel.SetActive (true);
	}

	public void actionConfirmed()
	{
		Time.timeScale = 1.0f;
		confirmScreen.SetActive(false);
		panel.SetActive (false);
		joystick.SetActive (false);
		SceneManager.LoadScene ("Intermediate");
	}

	public void actionDenied()
	{
		Time.timeScale = 1.0f;
		confirmScreen.SetActive (false);
		panel.SetActive (true);
	//	pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
		pauseMenu.transform.GetChild(1).gameObject.SetActive(true);
		pauseMenu.SetActive (false);
	}
	public void sond()
	{
		youdidthistoher.Instance.MenuSound ();
		if(youdidthistoher.Instance.soundOn==1)
			soundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.pressedSound;
		else
			soundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.unpressedSound;
	}

	public void oSond()
	{
		if (youdidthistoher.Instance.inGameSond == 1) {
			youdidthistoher.Instance.inGameSond = 0;
			osoundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.unpressedoSound;
			a.SetActive (false);
		}else
		{	youdidthistoher.Instance.inGameSond = 1;
			osoundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.pressedoSound;
			a.SetActive (true);

		}
		youdidthistoher.Instance.Save ();
	}
}
