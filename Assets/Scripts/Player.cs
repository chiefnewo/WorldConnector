using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameConstants gameConstants;

	public float rotationSpeed = 180f;
	public float maxVelocity = 2.0f;
	private float currentVelocity = 0.0f;
	public float acceleration = .1f;
	public float borderSpeedPenalty = .1f;

	private bool exploded = false;
	protected Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();

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
			currentVelocity *= 0.001f;

		if (currentVelocity > maxVelocity)
			currentVelocity = maxVelocity;

		if (currentVelocity < -maxVelocity)
			currentVelocity = -maxVelocity;

		transform.Translate(0, currentVelocity, 0);
		transform.Rotate(Vector3.forward, -rotation);

		if (transform.position.x < gameConstants.gameBoundL){
			transform.position = new Vector3(gameConstants.gameBoundL, transform.position.y);
			currentVelocity *= borderSpeedPenalty;
		}

		if (transform.position.x > gameConstants.gameBoundR){
			transform.position = new Vector3(gameConstants.gameBoundR, transform.position.y);
			currentVelocity *= borderSpeedPenalty;
		}

		if (transform.position.y > gameConstants.gameBoundT){
			transform.position = new Vector3(transform.position.x, gameConstants.gameBoundT);
			currentVelocity *= borderSpeedPenalty;
		}

		if (transform.position.y < gameConstants.gameBoundB){
			transform.position = new Vector3(transform.position.x, gameConstants.gameBoundB);
			currentVelocity *= borderSpeedPenalty;
		}
	}

	void Update () {
		if (Input.GetButton("Fire1"))
			currentVelocity = 0;
	}

	void Explode(){
		// play explosion
		exploded = true;
		animator.SetBool("Boosting", false);
		animator.SetTrigger("Explode");
	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log("here!");
		if (col.transform.tag == "Deadly")
			Explode();
	}
}
