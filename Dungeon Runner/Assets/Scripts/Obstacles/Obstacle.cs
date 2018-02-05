using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obstacle superclass: just base movement
/// </summary>
public class Obstacle : MonoBehaviour {

	public float baseSpeed = 2;
	public bool defeatable;

	void Update () {
		transform.position = new Vector2(transform.position.x - baseSpeed * Time.deltaTime, transform.position.y);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.x < LaneManager.instance.xThreshold) {
			Destroy (gameObject);
		}
	}
}
