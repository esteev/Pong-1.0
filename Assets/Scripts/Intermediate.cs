using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intermediate : MonoBehaviour {


	// rowsXcolumns = total levels;
	private const int NO_OF_ROWS=25;
	private const int NO_OF_LEVELS_IN_A_ROW = 4;
	private const int LEVEL_ROW_WIDTH = 60;

	public GameObject levelButton;
	public GameObject levelButtonContainer;
	//public GameObject levelSelectorCanvas;
	public GameObject worksGoodPanel;
	public GameObject buttonsCanvas;
	public GameObject practicePane;
	public Sprite currentLevelImage;
	public Text HS, LEV, COI;
	private int currentPlayingLevel;
	private int campaignLevelReached;
	public ScrollRect scrollyRecty;

	void Start () {
		////////
		AdManager.Instance.ShowBanner ();
		////////
		HS.text = "HighScore: "+PlayerPrefs.GetInt("HighScore").ToString();
		LEV.text = "Current Level: "+PlayerPrefs.GetInt ("campaignLevelReached").ToString();
		COI.text = PlayerPrefs.GetInt ("Currency").ToString ();
		worksGoodPanel.SetActive(false);
		buttonsCanvas.SetActive(true);
		currentPlayingLevel = PlayerPrefs.GetInt("currentPlayingLevel");
		campaignLevelReached =PlayerPrefs.GetInt ("campaignLevelReached");
		loadLevels ();
		if (youdidthistoher.Instance.soundOn == 1)
			youdidthistoher.Instance.gameObject.GetComponent<AudioSource>().enabled = true;
		else
			youdidthistoher.Instance.gameObject.GetComponent<AudioSource>().enabled = false;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Back ();
		}

		if (worksGoodPanel.transform.localPosition.y > NO_OF_ROWS * LEVEL_ROW_WIDTH) {
			worksGoodPanel.transform.localPosition = new Vector3 (worksGoodPanel.transform.localPosition.x, NO_OF_ROWS * LEVEL_ROW_WIDTH, worksGoodPanel.transform.localPosition.z);
			scrollyRecty.StopMovement ();
		} else if (worksGoodPanel.transform.localPosition.y < -120) {
			worksGoodPanel.transform.localPosition = new Vector3 (worksGoodPanel.transform.localPosition.x, 0f, worksGoodPanel.transform.localPosition.z);
			scrollyRecty.StopMovement ();
		}
	}

	private void LoadMenu(int LevelNo)
	{
		print (LevelNo);

		// LOAD LEVELs HERE
//		int levelNo=int.Parse(LevelName);
	/*	if (LevelNo > campaignLevelReached) {
			return ;
		}
		PlayerPrefs.SetInt("currentPlayingLevel",LevelNo);
		youdidthistoher.Instance.currentPlayingLevel=LevelNo;
		youdidthistoher.Instance.Save ();
		print (LevelNo);
		youdidthistoher.Instance.gameObject.GetComponent<AudioSource> ().enabled = false;
		SceneManager.LoadScene ("Pong_Breaker");
*/
	}

	public void Campaign()
	{
		worksGoodPanel.SetActive (true);
		buttonsCanvas.SetActive (false);
	}

	public void Endless()
	{
		youdidthistoher.Instance.gameObject.GetComponent<AudioSource> ().enabled = false;
		SceneManager.LoadScene ("Pong_Breaker_Endless");
	}

	public void Practice()
	{
		worksGoodPanel.SetActive (false);
		buttonsCanvas.SetActive (false);
		practicePane.SetActive (true);
	}

	public void vsWall()
	{
		youdidthistoher.Instance.gameObject.GetComponent<AudioSource> ().enabled = false;
		SceneManager.LoadScene ("Pong_Breaker_Practice_Wall");
	}

	public void vsAI()
	{
		youdidthistoher.Instance.gameObject.GetComponent<AudioSource> ().enabled = false;
		SceneManager.LoadScene ("Pong_Breaker_Practice");
	}

	public void Back()
	{
		if (practicePane.activeInHierarchy == true||worksGoodPanel.activeInHierarchy == true) 
		{
			worksGoodPanel.SetActive (false);
			buttonsCanvas.SetActive (true);
			practicePane.SetActive (false);
		}
		else
		{
			SceneManager.LoadScene ("MainMenu");	
		}
	}

	public void yay()
	{
		print ("mkc");
	}

	private void loadLevels()
	{
		int total = 0;
		for (int i = 0; i < NO_OF_ROWS; i++) 
		{
			total++;
			GameObject levelContainer = Instantiate (levelButtonContainer, worksGoodPanel.transform);
			levelContainer.transform.localPosition = Vector3.zero;
			for (int j = 1; j <= NO_OF_LEVELS_IN_A_ROW; j++) 
			{
				GameObject level = Instantiate (levelButton, levelContainer.transform);
				level.transform.localPosition = Vector3.zero;
				int levelNo = (i*(NO_OF_LEVELS_IN_A_ROW))+ j;
				level.transform.GetChild (0).GetComponent<Text> ().text = levelNo.ToString();
				level.GetComponent<Button> ().onClick.AddListener (() => LoadMenu(levelNo));
			//	level.transform.GetChild (0).gameObject.SetActive (false);
				if (levelNo == campaignLevelReached)
					level.GetComponent<Image> ().sprite = currentLevelImage;

				if (levelNo <= campaignLevelReached) 
				{
				//	level.transform.GetChild (0).gameObject.SetActive (true);
					level.transform.GetChild (1).gameObject.SetActive (false);
				}
			}
		}
//		print (total);
		/*
		foreach (Sprite thumbnail in thumbnails) 
		{
			GameObject container = Instantiate (levelButtonPrefab) as GameObject;
			container.GetComponent<Image> ().sprite = thumbnail;
			container.transform.SetParent (levelButtonContainer.transform,false);

			string LevelName = thumbnail.name;
			container.GetComponent<Button> ().onClick.AddListener (() => LoadMenu(LevelName));
		}
		*/
	}
}
