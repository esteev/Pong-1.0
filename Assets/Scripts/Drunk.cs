using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drunk : MonoBehaviour {

	private float SPEED=15.0f;
	private float SMOOTH_MOVEMENT=0.4f;
	private const float PADDLE_BACK_ANIM_DUR = 0.1f;
	private float MIN_X, MAX_X, MAX_Z, MIN_Z;

	private float ORIGINAL_FORCE_MODIFIER = 0.4f;
	private float MAX_FORCE = 500.0f;

	private const float PAD_BOUND = 1.25f;

	private const int NEGATIVE_CORRECT = -1;
	private Vector3 FIELD_CENTRE = new Vector3 (0.0f,0.4f,0.0f);

	private const float MAST_VALUE_PAD_NORMAL = 1.5f;
	private const float MAST_VALUE_PAD_SHORT=1f;
	private const float MAST_VALUE_PAD_LONG=2.8f;

	public Joystick joystick;
	//public GameObject ball;
	public GameObject js;							//joystick enable hone pe bhi touch controls kaam karenge

	private float dirZ=0;
	private int dirX=0;
	private int inputController;
	private float zForce;
	private float xForce;

	private Vector3 temp;
	private Rigidbody rb;
	public float maximumZ,minimum;
	private Ray ray;
	private RaycastHit hit;
	private int dirTouchX,dirTouchZ;
	private Vector3 oldPostion;
	private GameSetter setter;
	public GameObject camera3;

	private float JOYSTICK_MULTIPLIER = 1.2f;
	private float GYRO_MULTIPLIER = 2f;

	private Vector3 camData;
	private Transform cam;
	public Camera topDown,fP;


	void Start () {
		MIN_X =	-11.5f + transform.localScale.x/2;
		MAX_X = -5.5f - transform.localScale.x/2;
		MAX_Z=	5.5f-transform.localScale.z/2;						
		MIN_Z=	-5.5f+transform.localScale.z/2;		
		setter = GameObject.Find ("Ground").GetComponent<GameSetter> ();

		Input.multiTouchEnabled = false;
		rb = GetComponent<Rigidbody> ();
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;				// heavy physics but ball doesnt go through objects at high SPEED anymore
		// temporary solution is to reduce the max force applied on ball
		inputController=PlayerPrefs.GetInt("InputType");
		if (inputController==1||inputController==2) 
		{
			js.SetActiveRecursively (false);
		}
		maximumZ = 1.25f;

		switch (youdidthistoher.Instance.currentCameraMode) {

		case 0:
			cam = Camera.main.transform;
			break;
		case 1:
			cam = topDown.transform;
			break;
		case 2:
			cam = fP.transform;
			break;
		}

		camData = cam.rotation.eulerAngles;

	}
	void setGame(){
		SPEED = setter.player_speed;
		SMOOTH_MOVEMENT = setter.playerSmoothMovement;
		ORIGINAL_FORCE_MODIFIER = setter.player_force_modifier;
		MAX_FORCE = setter.player_force;
	}
	void FixedUpdate () {
		setGame ();
		cam.transform.rotation = Quaternion.Slerp (cam.rotation, Random.rotation, 0.1f*Time.deltaTime);
		int margin = 10;
		if (cam.transform.rotation.eulerAngles.x > camData.x+margin || cam.transform.rotation.eulerAngles.x < camData.x-margin)
			cam.transform.rotation = Quaternion.Euler (camData.x, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);

		if (cam.transform.rotation.eulerAngles.y > camData.y+margin || cam.transform.rotation.eulerAngles.y < camData.y-margin)
			cam.transform.rotation = Quaternion.Euler (cam.transform.rotation.eulerAngles.x, camData.y, cam.transform.rotation.eulerAngles.z);

		if (cam.transform.rotation.eulerAngles.z > camData.z+margin || cam.transform.rotation.eulerAngles.z < camData.z-margin)
			cam.transform.rotation = Quaternion.Euler (cam.transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, camData.z);


//		print (camData.x);
//		print (camData.y);
//		print (camData.z);


		float RandMultiplier = 0.6f;		//speed slow
		if (Random.Range (0, 15) <= 1)
			RandMultiplier = 4f;		//speed up

		if (transform.localScale.z > 2.5) {
			maximumZ = MAST_VALUE_PAD_LONG;
		} else if (transform.localScale.z < 2.5) {
			maximumZ = MAST_VALUE_PAD_SHORT;
		} else {
			maximumZ = MAST_VALUE_PAD_NORMAL;
		}
		transform.rotation = Quaternion.identity;																//Rotates to zero
		if (inputController == 1) {
			//finger touch control
			//			print ("1");

			if (Input.touchCount > 0) {
				// The screen has been touched so store the touch
				Touch touch = Input.GetTouch (0);
				if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
					// If the finger is on the screen, move the object smoothly to the touch position

					ray = Camera.main.ScreenPointToRay (touch.position);
					Physics.Raycast (ray, out hit, Mathf.Infinity);
					print (hit.point);

					Vector3 toMove = new Vector3 (Mathf.Clamp (hit.point.x, hit.point.x - 0.05f, hit.point.x + 0.05f), 
						transform.position.y, Mathf.Clamp (hit.point.z, hit.point.z - 0.05f, hit.point.z + 0.05f));

					transform.position = Vector3.MoveTowards (transform.position, toMove, SMOOTH_MOVEMENT*RandMultiplier);

				}

			} else {
				dirTouchX = 0;
				dirTouchZ = 0;

			}
			if (Input.GetButton ("Fire1")) {
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Physics.Raycast (ray, out hit, Mathf.Infinity);
				if (hit.point.x < transform.position.x) {
					dirTouchX = -1;
				} else {
					dirTouchX = 1;
				}

				if (hit.point.z < transform.position.z) {
					dirTouchZ = -1;
				} else {
					dirTouchZ = 1;
				}
				//By mouse
				transform.position = Vector3.MoveTowards (transform.position,
					new Vector3 (Mathf.Clamp (hit.point.x, hit.point.x - 0.05f, hit.point.x + 0.05f), transform.position.y, Mathf.Clamp (hit.point.z, hit.point.z - 0.05f, hit.point.z + 0.05f))
					, SMOOTH_MOVEMENT*RandMultiplier);

			} else {
				dirTouchX = 0;
				dirTouchZ = 0;
			}

		} else if (inputController == 0) {
			//joystick control

			//			print ("2");
			Vector3 direction=Vector3.zero;

			if (camera3.activeSelf) {
				direction.x = joystick.Vertical () * Time.deltaTime * SPEED;
				direction.z = joystick.Horizontal () * Time.deltaTime * SPEED * NEGATIVE_CORRECT; 

			} else {
				direction.x = joystick.Horizontal () * Time.deltaTime * SPEED * NEGATIVE_CORRECT; 
				direction.z = joystick.Vertical () * Time.deltaTime * SPEED * NEGATIVE_CORRECT;
			}

			direction = direction *JOYSTICK_MULTIPLIER*RandMultiplier;


			temp = transform.position;
			temp.x = Mathf.Clamp (temp.x +direction.x, MIN_X, MAX_X);												//Field Play Constraint
			temp.z = Mathf.Clamp (temp.z +direction.z, MIN_Z, MAX_Z);												//Field Play Constraint
			transform.position = temp;



		} /*else if (inputController == 0) {
			//joystick control

			//			print ("2");

			Vector3 direction=Vector3.zero;
			if (camera3.activeSelf) {
				direction.x = joystick.Vertical () ;
				direction.z = joystick.Horizontal () * NEGATIVE_CORRECT; 

			} else {
				direction.x = joystick.Horizontal ()  * NEGATIVE_CORRECT; 
				direction.z = joystick.Vertical () * NEGATIVE_CORRECT;

			}
			direction.Normalize ();

			temp = transform.position;

			direction.x = temp.x + direction.x;
			direction.y = temp.y;
			direction.z = temp.z + direction.z;

			transform.position = Vector3.MoveTowards (transform.position, direction, SMOOTH_MOVEMENT/0.7f);

		} */else if (inputController == 2) {
			Vector3 dir = Vector3.zero;
			if (camera3.activeSelf) {
				dir.x = Input.acceleration.y;
				dir.z = -Input.acceleration.x;
			} else {
				dir.x = -Input.acceleration.x;
				dir.z = -Input.acceleration.y;
			}
			//if (dir.sqrMagnitude > 1)
			//	dir.Normalize();

			dir *= Time.deltaTime*GYRO_MULTIPLIER*RandMultiplier;

			// Move object
			transform.Translate (dir * SPEED);


		}/*else if (inputController == 2) {
			Vector3 direction = Vector3.zero;
			if (camera3.activeSelf) {
				direction.x = Input.acceleration.y;
				direction.z = -Input.acceleration.x;
			} else {
				direction.x = -Input.acceleration.x;
				direction.z = -Input.acceleration.y;
			}
			print (Input.acceleration.y);
			//direction.x = Mathf.Clamp (direction.x, direction.x - 0.25f, direction.x + 0.25f);
			//direction.z = Mathf.Clamp (direction.z, direction.z - 0.25f, direction.z + 0.25f);




			//if (dir.sqrMagnitude > 1)
			//	dir.Normalize();
			direction=direction.normalized*Time.deltaTime*25;
			temp = transform.position;

			direction.x = temp.x + direction.x;
			direction.y = temp.y;
			direction.z = temp.z + direction.z;

			transform.position = Vector3.MoveTowards (transform.position, direction, SMOOTH_MOVEMENT*GYRO_MULTIPLIER);


		}*/
		if (Random.Range (0f, 1f) <= 0.05f)
		{
			Vector3 daruChal = new Vector3 (Random.Range (-1f,1f), 0.0f, Random.Range (-1f, 1f));
			daruChal /= 1.5f;
			transform.Translate (daruChal);
		}


		temp = transform.position;
		temp.x = Mathf.Clamp (temp.x , MIN_X, MAX_X);												//Field Play Constraint
		temp.z = Mathf.Clamp (temp.z , MIN_Z, MAX_Z);												//Field Play Constraint
		transform.position = temp;

		//CHECKING IF PAD GOT OUT OF PLAY ZONE DUE TO EXTERNAL FACTORS


		if (transform.position.x > MAX_X || transform.position.x < MIN_X || transform.position.z > MAX_Z || transform.position.z < MIN_Z) {
			temp.x = (MAX_X + MIN_X) / 2;
			temp.z = (MAX_Z + MIN_Z) / 2;
			transform.position = Vector3.MoveTowards (transform.position, temp, SMOOTH_MOVEMENT * 4);
		}



	}
	void Update(){
		oldPostion = this.transform.position;

	}
	void OnCollisionEnter(Collision col)
	{	
		//		float calcx, calcz;
		if (col.gameObject.CompareTag ("Ball")) {

			foreach (ContactPoint contact in col.contacts) {
				contact.otherCollider.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				dirZ = contact.point.z - transform.position.z;
				if (Mathf.Abs (dirZ) > maximumZ) {
					maximumZ = Mathf.Abs (dirZ);
				}
				dirZ = dirZ / maximumZ;
				dirZ = dirZ * Mathf.PI / 2;
				xForce = MAX_FORCE * Mathf.Cos (dirZ);
				zForce = MAX_FORCE * Mathf.Sin (dirZ);
				if (contact.point.x < transform.position.x)
					xForce = -xForce;
				contact.otherCollider.GetComponent<Rigidbody> ().AddForce (xForce, 0, zForce);


			}
			if(oldPostion.x<this.transform.position.x){
				//				print ("chal jaa bhai");
				col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(xForce,0,zForce)*ORIGINAL_FORCE_MODIFIER);

			}

			Invoke ("padBack", PADDLE_BACK_ANIM_DUR);

		}



	}


	void padBack()	
	{
		//PADDLE stop TRANSITION HERE

		rb.velocity = Vector3.zero;
		dirZ = dirX = 0;
	}

}