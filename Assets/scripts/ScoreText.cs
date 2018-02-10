using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour {
	Text score;

	void OnEnable(){
		score = GetComponent<Text> ();
		score.text = "Score: " + GameManager.Instance.scoreText;

	}

}
