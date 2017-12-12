using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerS : MonoBehaviour {

	public float SPEED;
	public float SMOOTH_MOVEMENT;
	private const float PADDLE_BACK_ANIM_DUR = 0.12f;
	private float MIN_X, MAX_X, MAX_Z, MIN_Z;

	private float ORIGINAL_FORCE_MODIFIER = 0.3f;
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
	private bool picheHo;

	private float JOYSTICK_MULTIPLIER = 1.2f;
	private float GYRO_MULTIPLIER = 2.2f;


	void Start () {
		MIN_X =	-11.5f + transform.localScale.x/2;
		MAX_X = -5.5f - transform.localScale.x/2;
		MAX_Z=	5.5f-transform.localScale.z/2;						
		MIN_Z=	-5.5f+transform.localScale.z/2;

		setter = GameObject.Find ("Ground").GetComponent<GameSetter> ();

		if (gameObject.name != "MCD") {
			this.gameObject.GetComponent<Renderer> ().material = youdidthistoher.Instance.materials [youdidthistoher.Instance.currentSkinIndexPad];
		}

		Input.multiTouchEnabled = false;
		rb = GetComponent<Rigidbody> ();
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;				// heavy physics but ball doesnt go through objects at high SPEED anymore
		// temporary solution is to reduce the max force applied on ball
		inputController=PlayerPrefs.GetInt("InputType");
		if (inputController==1||inputController==2) 
		{
			js.SetActiveRecursively (false);
		}
		maximumZ = 1.25f;
		InvokeRepeating ("setGame", 0.5f, 10f);
		InvokeRepeating ("CalculateMaximumZ", 0.1f, 1f);
	}

	void FixedUpdate () {
		oldPostion = this.transform.position;
		if (!picheHo) {
			if (inputController == 1) {
				TouchControl ();
				MouseControl ();

			} else if (inputController == 0) {
				JoystickControl ();
			} else if (inputController == 2) {
				GyroControl ();
			}
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

	void OnCollisionEnter(Collision col)
	{	if (col.gameObject.CompareTag ("Ball")) {
			picheHo = true;
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
				if (contact.point.x < transform.position.x) {
					xForce = -xForce;
				}
				contact.otherCollider.GetComponent<Rigidbody> ().AddForce (xForce, 0, zForce);
			}
			if(oldPostion.x<this.transform.position.x){
				col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(xForce,0,zForce)*ORIGINAL_FORCE_MODIFIER);
			}
			Invoke ("padBack", PADDLE_BACK_ANIM_DUR);
		}
	}


	void padBack()	
	{	picheHo = false;
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	void setGame(){
		SPEED = setter.player_speed;
		SMOOTH_MOVEMENT = setter.playerSmoothMovement;
		ORIGINAL_FORCE_MODIFIER = setter.player_force_modifier;
		MAX_FORCE = setter.player_force;
	}


	void TouchControl(){
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

				transform.position = Vector3.MoveTowards (transform.position, toMove, SMOOTH_MOVEMENT);

			}

		} 

	}
	void MouseControl(){
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
				, SMOOTH_MOVEMENT);

		} else {
			dirTouchX = 0;
			dirTouchZ = 0;
		}
	}
	void JoystickControl(){
		Vector3 direction=Vector3.zero;

		if (camera3.activeSelf) {
			direction.x = joystick.Vertical () * Time.deltaTime * SPEED;
			direction.z = joystick.Horizontal () * Time.deltaTime * SPEED * NEGATIVE_CORRECT; 

		} else {
			direction.x = joystick.Horizontal () * Time.deltaTime * SPEED * NEGATIVE_CORRECT; 
			direction.z = joystick.Vertical () * Time.deltaTime * SPEED * NEGATIVE_CORRECT;
		}

		direction = direction *JOYSTICK_MULTIPLIER;

		temp = transform.position;
		temp.x = Mathf.Clamp (temp.x +direction.x, MIN_X, MAX_X);												//Field Play Constraint
		temp.z = Mathf.Clamp (temp.z +direction.z, MIN_Z, MAX_Z);												//Field Play Constraint
		transform.position = temp;


		
	}
	void GyroControl(){
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

		dir *= Time.deltaTime*GYRO_MULTIPLIER;

		// Move object
		transform.Translate (dir * SPEED);
	}
	void CalculateMaximumZ(){
		// maximumZ is value that is used in calculation for hitting ball in z direction
		if (transform.localScale.z > 2.5) {
			maximumZ = MAST_VALUE_PAD_LONG;
		} else if (transform.localScale.z < 2.5) {
			maximumZ=MAST_VALUE_PAD_SHORT;
		} else {
			maximumZ = MAST_VALUE_PAD_NORMAL;
		}
	}
}
