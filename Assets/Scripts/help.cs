using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class help : MonoBehaviour {

	private const int no_of_powerups = 10;
	private const int no_of_blokes=5;
	public GameObject GamePlay, PowerUps, Blokes, inputs, camerad;
	public Text pUp,blo;
	private TextAsset txt,txt1;
	private string[] parsed,parsed1;

	void Start()
	{
		txt = (TextAsset)Resources.Load ("TextAssets/powerups",typeof(TextAsset));
		txt1 = (TextAsset)Resources.Load ("TextAssets/blokes",typeof(TextAsset));
		parsed = new string[no_of_powerups];
		parsed1 = new string[no_of_blokes];
		parsed=txt.text.Split ("\n"[0]);
		parsed1=txt1.text.Split ("\n"[0]);
	}

	public void gameplay()
	{
		reset ();
		GamePlay.SetActive (true);
	}

	public void powerups()
	{
		reset ();
		PowerUps.SetActive (true);
	}

	public void blokes()
	{
		reset ();
		Blokes.SetActive (true);
	}

	public void input()
	{
		reset ();
		inputs.SetActive (true);
	}

	public void cameras()
	{
		reset ();
		camerad.SetActive (true);
	}

	public void powerupexplain(int i)
	{
		pUp.text = parsed [i];
	}

	public void blokeexplain(int i)
	{
		blo.text = parsed1 [i];
	}

	private void reset()
	{
		GamePlay.SetActive (false);
		PowerUps.SetActive (false);
		Blokes.SetActive (false);
		inputs.SetActive (false);
		camerad.SetActive (false);
	}
}
