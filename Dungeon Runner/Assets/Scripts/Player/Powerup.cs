using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class power-up: basically just horizontal movement
/// </summary>
public class Powerup : MonoBehaviour {

	public float speed = 3;

	Rigidbody2D body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		body.velocity = new Vector2 (-speed, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.x < LaneManager.instance.xThreshold) {
			Destroy (gameObject);
		}
	}
}