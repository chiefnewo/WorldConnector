using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameConstants gameConstants;

	public float rotationSpeed = 180f;
	public float maxVelocity = .1f;
	private float currentVelocity = 0.0f;
	public float acceleration = .1f;
	public float borderSpeedPenalty = 50f;
	public float wallBounce = .1f; // how far off the walls to bounce when you hit them

	public float explosionSlowdown = 48f;
	public float goalSlowdown = 40f;
	private float slowDown = 1f;

	private bool exploded = false;
	protected Animator animator;

	// sounds
	public AudioClip explosion;
	public AudioClip levelComplete;
	public AudioClip levelStart;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		gameConstants = (GameConstants)GameObject.Find("GameConstants").GetComponent("GameConstants");
		AudioSource.PlayClipAtPoint(levelStart, Vector2.zero);
	}
	
	void FixedUpdate () {
		float rotation = 0f;

		if (!exploded){
			currentVelocity += Input.GetAxisRaw("Vertical") * acceleration * Time.fixedDeltaTime;
			if (Input.GetAxisRaw("Vertical") > 0)
				animator.SetBool("Boosting", true);
			else
				animator.SetBool("Boosting", false);
			rotation = Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
		} else
			currentVelocity *= slowDown * Time.fixedDeltaTime;

		if (currentVelocity > maxVelocity)
			currentVelocity = maxVelocity;

		if (currentVelocity < -maxVelocity)
			currentVelocity = -maxVelocity;

		transform.Translate(0, currentVelocity, 0);
		transform.Rotate(Vector3.forward, -rotation);

		if (transform.position.x < gameConstants.gameBoundL){
			transform.position = new Vector3(gameConstants.gameBoundL + wallBounce, transform.position.y);
			currentVelocity *= borderSpeedPenalty * Time.fixedDeltaTime;
		}

		if (transform.position.x > gameConstants.gameBoundR){
			transform.position = new Vector3(gameConstants.gameBoundR - wallBounce, transform.position.y);
			currentVelocity *= borderSpeedPenalty * Time.fixedDeltaTime;
		}

		if (transform.position.y > gameConstants.gameBoundT){
			transform.position = new Vector3(transform.position.x, gameConstants.gameBoundT - wallBounce);
			currentVelocity *= borderSpeedPenalty * Time.fixedDeltaTime;
		}

		if (transform.position.y < gameConstants.gameBoundB){
			transform.position = new Vector3(transform.position.x, gameConstants.gameBoundB + wallBounce);
			currentVelocity *= borderSpeedPenalty * Time.fixedDeltaTime;
		}
	}

	void Update () {
		if (Input.GetButton("Fire1"))
			currentVelocity = 0;
	}

	void Explode(){
		// play explosion
		exploded = true;
		slowDown = explosionSlowdown;
		animator.SetBool("Boosting", false);
		animator.SetTrigger("Explode");
		AudioSource.PlayClipAtPoint(explosion, Vector2.zero);
	}

	void OnTriggerEnter2D(Collider2D col) {
		//Debug.Log("here!");
		if (col.transform.tag == "Deadly")
			Explode();

		if (col.transform.tag == "Goal"){
			AudioSource.PlayClipAtPoint(levelComplete, Vector2.zero);
			GameController.Instance.ReachedGoal();
			exploded = true;
			slowDown = goalSlowdown;
		}

		if (col.transform.tag == "Wormhole"){
			transform.position = col.transform.GetComponent<WormholeSpin>().exit.position;
			transform.rotation = col.transform.GetComponent<WormholeSpin>().exit.rotation;
		}
	}

	void NextLife(){
		GameController.Instance.NextLife();
	}

	void PlayRocketSound(){
		if (!audio.isPlaying){
			audio.Play();
		}
	}

	void StopRocketSound(){
		audio.Stop();
	}
}
