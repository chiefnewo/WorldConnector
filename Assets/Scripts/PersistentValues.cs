using UnityEngine;
using System.Collections;

public class PersistentValues : MonoBehaviour {

	public static int score = 0;
	public static int lives = 3;
	public static int highScore = 500;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
