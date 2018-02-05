using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fireball for Mage: just horizontal movement
/// </summary>
public class Fireball : MonoBehaviour {

	public float speed;

	void FixedUpdate () {
		GetComponent<Rigidbody2D>().velocity = new Vector2 (speed, 0);
		// Destroys itself after passing a threshold
		if (transform.position.x > -1 * LaneManager.instance.xThreshold) {
			Destroy (gameObject);
		}
	}
}
