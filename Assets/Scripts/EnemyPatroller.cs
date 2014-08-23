using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatroller : MonoBehaviour {

	public float enemySpeed = 0.001f;
	public float chooseDistance = .1f; // how close to waypoint before choosing next one

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
		Vector2 curPos = transform.position;
		if (Vector2.Distance(curPos, waypoints[targetWaypoint]) <= chooseDistance)
			NextWaypoint();

		Vector2 trans = Vector2.MoveTowards(curPos, waypoints[targetWaypoint], enemySpeed * Time.deltaTime);
		transform.position = trans;
	}

	void NextWaypoint(){
		targetWaypoint++;

		if (targetWaypoint >= waypoints.Count)
			targetWaypoint = 0;
	}
}