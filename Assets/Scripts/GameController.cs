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
	public GUIText highTxt;
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
		scoreTxt.text = PersistentValues.score.ToString();
		InvokeRepeating("UpdateBonus", 1, 1);
	}

	// Update is called once per frame
	void Update () {
		scoreTxt.text = PersistentValues.score.ToString();
		highTxt.text = PersistentValues.highScore.ToString();
		bonusTxt.text = bonus.ToString();
		livesTxt.text = PersistentValues.lives.ToString();

		if (Input.GetButtonDown("Reset")) {
			if (PersistentValues.lives > 0)
				NextLife();
			else {
				PersistentValues.score = 0;
				PersistentValues.lives = 3;
				Application.LoadLevel(0); // restart game
			}
		}
	}

	public void ReachedGoal(){
		PersistentValues.score += 100 + bonus;
		CancelInvoke("UpdateBonus");
		int nextScene = Application.loadedLevel + 1;
		if (nextScene >= Application.levelCount)
			nextScene = 0;
		Application.LoadLevel(nextScene);
	}

	public void NextLife(){
		if (PersistentValues.lives > 1){
			PersistentValues.lives--;
			PlayerStart();
		} else {
			bonus = 0;
			if (PersistentValues.score > PersistentValues.highScore)
				PersistentValues.highScore = PersistentValues.score;
			PersistentValues.lives = 0;
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
