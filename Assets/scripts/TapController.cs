using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class TapController : MonoBehaviour {


	public delegate void PlayerDelegate();
	public static event PlayerDelegate OnPlayerDied;
	public static event PlayerDelegate OnPlayerScored;


	public float tapForce = 10;
	public float tiltSmooth = 5;

	public Vector3 startPos;
	Rigidbody2D rigidbody;
	Quaternion downRotation;
	Quaternion forwardRotation;

	//public AudioSource themeAudio;
	//public AudioSource tapAudio;
	public AudioSource dieAudio;
	public AudioSource scoreAudio;
	public AudioSource scoretwoAudio;
	public AudioSource scorethreeAudio;


	GameManager game;
	int score = 0;

	void OnEnable(){

		GameManager.OnGameStarted += OnGameStarted;
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;

	}

	void OnDisable(){
		GameManager.OnGameStarted -= OnGameStarted;
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	void OnGameStarted(){
		rigidbody.velocity = Vector3.zero;
		rigidbody.simulated = true;
	
	}
	void OnGameOverConfirmed(){
		transform.localPosition = startPos;
		transform.rotation = Quaternion.identity;
	}

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		downRotation = Quaternion.Euler (0,0,-90);
		forwardRotation = Quaternion.Euler (0,0,30);
		game = GameManager.Instance;
		rigidbody.simulated = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (game.GameOver)
			return;
		

		if (Input.GetMouseButtonDown (0)) {



			transform.rotation = forwardRotation;
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce (Vector2.up * tapForce, ForceMode2D.Force);
		}
		transform.rotation = Quaternion.Lerp (transform.rotation, downRotation, tiltSmooth	* Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "ScoreZoneTag"){
			//register score event
			OnPlayerScored(); //GameManager
			//play a sound
			score = game.giveScore; 
			if (score > 0 && score % 5 == 0) scoreAudio.Play ();
			if (score > 0 && score % 10 == 0) scorethreeAudio.Play ();
			if (score > 0 && score % 20 == 0) scoretwoAudio.Play (); 

		}
		if(col.gameObject.tag == "DeadZoneTag"){
			rigidbody.simulated = false;
			//register dead event
			OnPlayerDied(); //GameManager
			//play a sound
			dieAudio.Play();	

		}

	}
}
