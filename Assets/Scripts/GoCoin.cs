using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoCoin : MonoBehaviour {

	// Use this for initialization
	public Vector3 toMove;
	public Vector3 originate;
	RectTransform RectTrans;
	void Start () {
		Transform go = this.transform.parent;
		originate = go.GetChild (5).GetComponent<RectTransform>().anchoredPosition;
		toMove = go.GetChild (6).GetComponent<RectTransform>().anchoredPosition;
		Randi();
		RectTrans = GetComponent<RectTransform> ();
		RectTrans.anchoredPosition = originate;
	}
	void Randi(){
		originate.x += Random.Range (-1f, 1f) * 30f +50f;
		originate.y += Random.Range (-1f, 1f) * 30f;
	}
	// Update is called once per frame
	void Update () {
		RectTrans.Rotate (new Vector3 (0, 30, 0) * Time.deltaTime*5f);
		RectTrans.anchoredPosition = Vector3.MoveTowards (RectTrans.anchoredPosition, toMove, 3f);
		if (RectTrans.anchoredPosition3D == toMove) {
			GameObject.Find("GameoverCanvas").GetComponent<GOBABY>().totalCoin.text = youdidthistoher.Instance.currency.ToString ();
			Destroy (this.gameObject);
		}
	}

}
