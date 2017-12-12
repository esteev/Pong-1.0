using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GOBABY : MonoBehaviour {

	private const int noOfWinText=20;
	private const int noOfLoseText=20;
	public GameObject pausemenu;
	private string[] parsed;
	private TextAsset txt;

	private int coinsEarned;
	public int state;

	public GameObject ground;
	public Text winText;
	public Text loseText;
	public Text totalCoin;
	public GameObject doubleCoin;
	public GameObject coins;  //for coin animation
	public static int coinMoveCount; //for coin animation no of coins
	void Awake()
	{
		parsed = new string[noOfLoseText];
	}

	void Start () {
		totalCoin.text = youdidthistoher.Instance.currency.ToString ();
		pausemenu.SetActive (false);
		switch (state) 
		{
		case 0:
			txt = (TextAsset)Resources.Load ("TextAssets/win", typeof(TextAsset));
			parsed = txt.text.Split ("\n" [0]);
			winText.text = parsed [Random.Range (0, noOfWinText)];
			break;
		case 1:
			txt = (TextAsset)Resources.Load ("TextAssets/lose",typeof(TextAsset));
			parsed=txt.text.Split ("\n"[0]);
			loseText.text = parsed [Random.Range (0, noOfLoseText)];
			break;
		}
		coinsEarned = ground.GetComponent<ScoreManager> ().coinCount;
		youdidthistoher.Instance.currency+=coinsEarned;
		youdidthistoher.Instance.Save ();
		InvokeRepeating ("goCoins", 0.2f, 0.1f);
	}
	private void goCoins(){
		if (coinMoveCount++ < 5) {
			var coin = Instantiate (coins, this.transform.GetChild (0));
			coin.transform.localScale = new Vector3 (0.823f, 0.823f, 0.823f);
		}

	}

	public void back()
	{	/////
		AdManager.Instance.HideBanner ();
		/////
	//	Time.timeScale = 1.0f;
		SceneManager.LoadScene ("Intermediate");
		//PlayerPrefs.SetInt ("currentPlayingLevel", PlayerPrefs.GetInt ("currentPlayingLevel") + 1);
		youdidthistoher.Instance.currentPlayingLevel++;
		youdidthistoher.Instance.Save ();
	}

	public void restart()
	{
	//	Time.timeScale = 1.0f;
		/////
		AdManager.Instance.HideBanner ();
		/////

		if (SceneManager.GetActiveScene ().name == "Pong_Breaker")
			SceneManager.LoadScene ("Pong_Breaker");
		else
			SceneManager.LoadScene ("Pong_Breaker_Endless");

		Time.timeScale = 1.0f;
	}

	public void next()
	{	/////
		AdManager.Instance.HideBanner ();
		/////
		//	
	//	Time.timeScale = 1.0f;
		youdidthistoher.Instance.currentPlayingLevel++;
		youdidthistoher.Instance.Save ();
		SceneManager.LoadScene ("Pong_Breaker");
	
	}

	public void WatchAdToMultiply()
	{	///
		AdManager.Instance.ShowRewardedVideo(0);
		///
		doubleCoin.SetActive(false);
		//ANIMATION HERE
		pausemenu.SetActive (false);
	}

}
