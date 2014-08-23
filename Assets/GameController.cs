using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Transform Player;
	public static GameController Instance;
	public GUIText scoreTxt;
	public GUIText bonusTxt;
	public GUIText livesTxt;
	public GUIText gameOverTxt;
	public GUIText restartTxt;
	public int lives = 3;
	private int score = 0;
	private int bonus = 0;
	private Transform playerRef = null;


	// Use this for initialization
	void Start () {
		Instance = this;
		StartGame();
	}

	void StartGame(){
		gameOverTxt.guiText.enabled = false;
		restartTxt.guiText.enabled = false;
		gameOverTxt.GetComponent<Animator>().SetBool("GameOver", false);
		PlayerStart();
		bonus = 100;
		bonusTxt.text = bonus.ToString();
		score = 0;
		lives = 3;
		scoreTxt.text = score.ToString();
		InvokeRepeating("UpdateBonus", 1, 1);
	}

	// Update is called once per frame
	void Update () {
		scoreTxt.text = score.ToString();
		bonusTxt.text = bonus.ToString();
		livesTxt.text = lives.ToString();

		if (Input.GetButtonDown("Reset")) {
			if (lives > 0)
				NextLife();
			else
				StartGame();
		}
	}

	public void ReachedGoal(){
		score += 100 + bonus;
		CancelInvoke("UpdateBonus");
	}

	public void NextLife(){
		if (lives > 1){
			lives--;
			PlayerStart();
		} else {
			bonus = 0;
			lives = 0;
			gameOverTxt.guiText.enabled = true;
			restartTxt.guiText.enabled = true;
			gameOverTxt.GetComponent<Animator>().SetBool("GameOver", true);
			CancelInvoke("UpdateBonus");
		}
	}

	void UpdateBonus(){
		if (bonus > 0)
			bonus--;
	}

	void PlayerStart(){
		if (playerRef != null)
			Destroy(playerRef.gameObject);
		playerRef = (Transform)Instantiate(Player, transform.position, transform.rotation);
	}
}
