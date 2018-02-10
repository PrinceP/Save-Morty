using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {




	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;


	public static GameManager Instance;

	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countDownPage;
	public Text scoreText;
	//public AudioSource scoreAudio;

	enum PageStage{
		None,
		Start,
		GameOver,
		Countdown
	}

	int score = 0;
	bool gameOver = true;
	public bool GameOver { get {  return gameOver; } }
	public int giveScore {  get {  return score; } }


	void Awake(){
		Instance = this;
	}

	void OnEnable(){
		CountdownText.OnCountdownFinished += OnCountdownFinished;
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;

	}
	void OnDisable(){
		CountdownText.OnCountdownFinished -= OnCountdownFinished;
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
	}

	void OnPlayerDied(){
		gameOver = true;
		int savedScored = PlayerPrefs.GetInt ("HighScore");
		if (score > savedScored) {
			PlayerPrefs.SetInt ("HighScore", score);
		}
		SetPageState (PageStage.GameOver);
	
	}
	void OnPlayerScored(){
		score++;
		scoreText.text = score.ToString ();

		//if (score > 0 && score % 10 == 0) scoreAudio.Play ();
	
	}


	void OnCountdownFinished(){
	
		SetPageState (PageStage.None);
		OnGameStarted (); //TapController
		score = 0;
		gameOver = false;
	
	}


	void SetPageState(PageStage state){
		switch (state) {
		case PageStage.None:
			startPage.SetActive (false);
			gameOverPage.SetActive (false);
			countDownPage.SetActive (false);
			break;
		
		case PageStage.Start:
			startPage.SetActive (true);
			gameOverPage.SetActive (false);
			countDownPage.SetActive (false);
			break;
		
		case PageStage.GameOver:
			startPage.SetActive (false);
			gameOverPage.SetActive (true);
			countDownPage.SetActive (false);
			break;

		case PageStage.Countdown:
			startPage.SetActive (false);
			gameOverPage.SetActive (false);
			countDownPage.SetActive (true);
			break;
		}
	}

	public void ConfirmGameOver(){
	
		//activated when replay button is hit
		OnGameOverConfirmed(); // TapController
		scoreText.text = "0";
		SetPageState (PageStage.Start);

	}

	public void StartGame(){

		//activated when start button is hit
		SetPageState(PageStage.Countdown);
	}

}
