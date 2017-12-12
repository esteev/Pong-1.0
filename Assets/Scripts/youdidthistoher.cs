using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class youdidthistoher : MonoBehaviour {

	private const int NO_OF_MATERIALS = 10;
	private const int NO_OF_MATERIALS_BLOKE = 15;
	private const int NO_OF_EXTRAS = 10;

	private static youdidthistoher instance;
	public static youdidthistoher Instance{get{ return instance; }}

	public Material[] materials; 
	public Material[] blokeMaterials;
	public Material[] extraMaterials;
	public Sprite[] pads;
	public Sprite[] blokes;
	public Sprite[] extras;
	public Sprite[] powerUps;

	public int currentSkinIndexPad=0;
	public int currentSkinIndexBloke = 0;
	public int currentGround = 0;
	public int currentWall = 4;
	public int currency=0;
	public int p_bigBall = 0;
	public int p_flareBall = 0;
	public int p_gun = 0;
	public int p_padLong = 0;
	public int p_magnet = 0;
	public int p_VIPBall = 0;
	public int p_multiBall = 0;
	public int skinAvailabilityPad=1;
	public int skinAvailabilityBloke=1;
	public int skinAvailabilityExtra = 17;
	public int MCDActive=0;
	public int DrunkActive=0;
	public int skinAvailabilityMCD = 0;
	public int skinAvailabilityDrunk = 0;
	public int hasRatedGame = 0;
	public int gameOpenCount = 1;
	public int currentPlayingLevel = 1;
	public float HighScore = 0;
	public int LevelLooseCount=1;
	public int soundOn = 1;
	public int inGameSond =1;

	public int padPriceDisplay = 100;
	public int blokePriceDisplay = 50;
	public int extraPriceDisplay = 150;
	public int powerUpPriceDisplay = 30;

	public int currentCameraMode=0;		//0 for dynamic, 2 for first person
	public int[] powerUpArray;
	public int campaignLevelReached=1;
	public string[] level;
	public Sprite pressedSound, unpressedSound, pressedoSound, unpressedoSound;

	public GameObject g1, g2, w1, w2, w3, w4, w5, w6, w7, w8;


	void Awake () {
		instance = this;
		DontDestroyOnLoad(gameObject);
		materials = new Material[NO_OF_MATERIALS]; 
		blokeMaterials = new Material[NO_OF_MATERIALS_BLOKE];
		extraMaterials = new Material[NO_OF_EXTRAS];
		powerUpArray = new int[7];
		materials = Resources.LoadAll<Material> ("Material/Pad_Materials");
		blokeMaterials = Resources.LoadAll<Material> ("Material/Bloke_Materials");
		extraMaterials = Resources.LoadAll<Material> ("Material/Extra_Materials");
		pads = Resources.LoadAll<Sprite> ("Pads");
		blokes = Resources.LoadAll<Sprite> ("Blokes");
		extras = Resources.LoadAll<Sprite> ("Extras");
		powerUps = Resources.LoadAll<Sprite> ("Power");
		TextAsset txt = (TextAsset)Resources.Load ("LevelStore", typeof(TextAsset));
		string content = txt.text;
		level = content.Split ('\n');


		if (PlayerPrefs.HasKey ("Currency")) {
			//We had a previous session
			currentSkinIndexPad = PlayerPrefs.GetInt("CurrentSkinPad");
			currentCameraMode = PlayerPrefs.GetInt ("currentCameraMode");
			currentSkinIndexBloke = PlayerPrefs.GetInt("CurrentSkinBloke");
			currency = PlayerPrefs.GetInt ("Currency");
			currentGround = PlayerPrefs.GetInt ("currentGround");
			currentWall = PlayerPrefs.GetInt ("currentWall");
			skinAvailabilityPad = PlayerPrefs.GetInt ("SkinAvailabilityPad");
			skinAvailabilityBloke = PlayerPrefs.GetInt ("SkinAvailabilityBloke");
			skinAvailabilityExtra = PlayerPrefs.GetInt ("SkinAvailabilityExtra");
			skinAvailabilityMCD = PlayerPrefs.GetInt ("SkinAvailabilityMCD");
			skinAvailabilityDrunk = PlayerPrefs.GetInt ("SkinAvailabilityDrunk");
			MCDActive = PlayerPrefs.GetInt ("MCDActive");
			DrunkActive = PlayerPrefs.GetInt ("DrunkActive");
			soundOn = PlayerPrefs.GetInt ("soundOn");
			inGameSond = PlayerPrefs.GetInt ("inGameSond");
			hasRatedGame = PlayerPrefs.GetInt ("hasRatedGame");
			gameOpenCount = PlayerPrefs.GetInt ("gameOpenCount");
			gameOpenCount++;
			LevelLooseCount = PlayerPrefs.GetInt ("levelLooseCount");
			powerUpArray [0] = PlayerPrefs.GetInt ("p_bigBall");
			powerUpArray [1] = PlayerPrefs.GetInt ("p_flareBall");
			powerUpArray [2] = PlayerPrefs.GetInt ("p_gun");
			powerUpArray [3] = PlayerPrefs.GetInt ("p_padLong");
			powerUpArray [4] = PlayerPrefs.GetInt ("p_magnet");
			powerUpArray [5] = PlayerPrefs.GetInt ("p_VIPBall");
			powerUpArray [6] = PlayerPrefs.GetInt ("p_multiBall");
			currentPlayingLevel = PlayerPrefs.GetInt ("currentPlayingLevel");
			campaignLevelReached = PlayerPrefs.GetInt ("campaignLevelReached");


		} else{
			//pehli baar chalega bencho
			Save();
			PlayerPrefs.SetInt ("currentPlayingLevel", currentPlayingLevel);
			PlayerPrefs.SetInt ("campaignLevelReached", campaignLevelReached);
			for (int i = 1; i<=level.Length; i++) {
				PlayerPrefs.SetString ("level" + i, level [i - 1]);
			}
			PlayerPrefs.SetFloat ("HighScore", HighScore);
		}

		if (soundOn == 1)
			AudioListener.volume = 1f;
		else
			AudioListener.volume = 0f;

		if (!(PlayerPrefs.HasKey ("cameraChanging"))) 
		{
			PlayerPrefs.SetInt ("cameraChanging", 0);
			PlayerPrefs.SetInt ("currentCameraMode",2);
			currentCameraMode = 2;
		}
	}
		
	public void Save()
	{
		PlayerPrefs.SetInt ("CurrentSkinPad", currentSkinIndexPad);
		PlayerPrefs.SetInt ("CurrentSkinBloke", currentSkinIndexBloke);
		PlayerPrefs.SetInt ("Currency", currency);
		PlayerPrefs.SetInt ("currentGround", currentGround);
		PlayerPrefs.SetInt ("currentWall", currentWall);
		PlayerPrefs.SetInt ("currentCameraMode", currentCameraMode);
		PlayerPrefs.SetInt ("SkinAvailabilityPad", skinAvailabilityPad);
		PlayerPrefs.SetInt ("SkinAvailabilityBloke", skinAvailabilityBloke);
		PlayerPrefs.SetInt ("SkinAvailabilityExtra", skinAvailabilityExtra);
		PlayerPrefs.SetInt ("SkinAvailabilityMCD", skinAvailabilityMCD);
		PlayerPrefs.SetInt ("SkinAvailabilityDrunk", skinAvailabilityDrunk);
		PlayerPrefs.SetInt ("p_bigBall", powerUpArray [0]);
		PlayerPrefs.SetInt ("p_flareBall", powerUpArray [1]);
		PlayerPrefs.SetInt ("p_gun", powerUpArray [2]);
		PlayerPrefs.SetInt ("p_padLong", powerUpArray [3]);
		PlayerPrefs.SetInt ("p_magnet", powerUpArray [4]);
		PlayerPrefs.SetInt ("p_VIPBall", powerUpArray [5]);
		PlayerPrefs.SetInt ("p_multiBall", powerUpArray [6]);
		PlayerPrefs.SetInt ("MCDActive", MCDActive);
		PlayerPrefs.SetInt ("DrunkActive", DrunkActive);
		PlayerPrefs.SetInt ("soundOn", soundOn);
		PlayerPrefs.SetInt ("inGameSond", inGameSond);
		PlayerPrefs.SetFloat ("HighScore", HighScore);
		PlayerPrefs.SetInt ("hasRatedGame", hasRatedGame);
		PlayerPrefs.SetInt ("gameOpenCount", gameOpenCount);
		PlayerPrefs.SetInt ("levelLooseCount", LevelLooseCount);
		PlayerPrefs.SetInt ("currentPlayingLevel", currentPlayingLevel);
		PlayerPrefs.SetInt ("campaignLevelReached", campaignLevelReached);	
		PlayerPrefs.SetFloat ("HighScore", HighScore);


		//	print (skinAvailabilityPad);
	//	print (skinAvailabilityExtra);
	}

	public void MenuSound()
	{
		if (youdidthistoher.Instance.soundOn == 1) {
			AudioListener.volume = 0f;
			soundOn = 0;
		} else {
			AudioListener.volume = 1f;
			soundOn = 1;
		}
		Save ();
	}
	/*
	public void loader()
	{
		w1.GetComponent<Renderer> ().material = extraMaterials [currentWall];
		w2.GetComponent<Renderer> ().material = extraMaterials [currentWall];
		w3.GetComponent<Renderer> ().material = extraMaterials [currentWall];
		w4.GetComponent<Renderer> ().material = extraMaterials [currentWall];
		w5.GetComponent<Renderer> ().material = extraMaterials [currentWall];
		w6.GetComponent<Renderer> ().material = extraMaterials [currentWall];
		w7.GetComponent<Renderer> ().material = extraMaterials [currentWall];
		w8.GetComponent<Renderer> ().material = extraMaterials [currentWall];
		g1.GetComponent<Renderer> ().material = extraMaterials [currentGround];
		g2.GetComponent<Renderer> ().material = extraMaterials [currentGround];

	}*/
}