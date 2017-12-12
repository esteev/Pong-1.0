using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

	private const int MAX_BALLS = 3;
	private const float PAD_COL_MARGIN_Z=0.25f;

	public GameObject padMagnet;
	public GameObject ballPrefab;

	private Rigidbody[] allBalls = new Rigidbody[MAX_BALLS];
	private Vector3[] forceLines = new Vector3[MAX_BALLS];
	private Vector3[]  saveLocalScale = new Vector3[MAX_BALLS];
	private Vector3[] savePosition= new Vector3[MAX_BALLS];

	private bool isParented = false;
	private int index=0;
	private float dirX=0;
	private float dirZ=0;
	private bool turn;

	void Start()
	{	
	//	dirX = Vector3.left.x;
		Physics.IgnoreCollision (ballPrefab.GetComponent<Collider>(),padMagnet.GetComponent<Collider>(),true);
		//Physics.IgnoreCollision (ballPrefab.GetComponent<Collider>(),this.gameObject.GetComponent<Collider>(),true);
		if (this.transform.parent.name.Contains ("player")) {
			turn = true;
		} else {
			turn = false;
		}
	}


	void FixedUpdate()
	{	dirX = (transform.parent.position.x > 0) ? Vector3.left.x : Vector3.right.x;
		if (isParented) {
			if (Input.touchCount >= 2) {
				release ();
			}
			if (gameObject.transform.parent.name.Contains ("AI")) {
				Invoke ("release", 2f);
			}
			if (Input.GetMouseButtonDown (0)) {
				release();
			}
		}
	}
	public void release(){
		for (int i = 0; i < index; i++) 
		{	
			
			allBalls[i].isKinematic = false;
			if (allBalls[i].gameObject.activeSelf&&allBalls [i].transform.parent.name.Contains ("MagContainer")) {
				GameObject temp;
				temp = allBalls [i].transform.parent.gameObject;
				allBalls [i].transform.SetParent (null);

				Destroy (temp);
			} else {
				allBalls [i].transform.SetParent (null);

			}

			//	Destroy (temp);
			allBalls[i].transform.localScale = saveLocalScale[i];
			allBalls [i].AddForce (forceLines[i]);
		}
		padMagnet.GetComponent<Rigidbody> ().isKinematic = false;
		index = 0;
	}
	void OnTriggerEnter(Collider col)
	{	
		//	print (col.gameObject.name);
		if (col.gameObject.CompareTag ("Ball"))
		{	if ((turn && transform.position.x < col.transform.position.x) || (!turn && transform.position.x < col.transform.position.x)) {
				saveLocalScale [index] = col.transform.localScale;
				savePosition [index] = col.transform.position;
				print (index);
				col.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				padMagnet.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				col.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
				col.transform.SetParent (padMagnet.transform);

				GameObject temp = new GameObject ();
				temp.name="MagContainer";
				temp.transform.SetParent (padMagnet.transform);
			
				temp.transform.localScale = new Vector3 (1/padMagnet.transform.localScale.x, 1/padMagnet.transform.localScale.y, 1/padMagnet.transform.localScale.z);

				col.transform.SetParent (temp.transform);
			

				print (padMagnet.transform.localScale);

				padMagnet.GetComponent<Rigidbody> ().isKinematic = true;

				if (transform.position.z - col.transform.position.z < -PAD_COL_MARGIN_Z) {
					dirZ = 1;																							
				} else if (transform.position.z - col.transform.position.z > PAD_COL_MARGIN_Z) {
					dirZ = -1;																							
				} else {
					dirZ = 0;
				}

				forceLines [index].x = dirX;
				forceLines [index].z = dirZ;
				allBalls [index++] = col.gameObject.GetComponent<Rigidbody> ();
				isParented = true;
			}
		}
	}

	void OnDestroy()
	{
		////////////        THIS WILL HAVE TO BE SHIFTED WHERE THE MAGET POWER UP ENDS SO THAT REGULAR COLLISIONS CAN TAKE PLACE
		Physics.IgnoreCollision (ballPrefab.GetComponent<Collider>(),padMagnet.GetComponent<Collider>(),false);
		//Physics.IgnoreCollision (ballPrefab.GetComponent<Collider>(),this.gameObject.GetComponent<Collider>(),false);
		//Physics.IgnoreCollision (allBalls[i].GetComponent<Collider> (), this.gameObject.GetComponent<Collider> (), false);
		index = 0;
	}
}
