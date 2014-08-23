using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatroller : MonoBehaviour {

	public float enemySpeed = 1.0f;

	private List<Vector2> waypoints = new List<Vector2>();
	private int targetWaypoint = 0;

	// Use this for initialization
	void Start () {
		waypoints.Add(transform.position); // always have at least one waypoint, our starting point
		Component[] childWaypoints = GetComponentsInChildren<Transform>();
		foreach(Transform tfm in childWaypoints){
			waypoints.Add (tfm.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// lerp to waypoint
	}
}
