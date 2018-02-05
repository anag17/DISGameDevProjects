using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arrow for Ranger: horizontal movement, drops over time, turns blue when upgraded
/// </summary>
public class Arrow : MonoBehaviour {

	public float speed;
	public float gravity;
	public bool upgraded;
	public Sprite upgradedArrow;

	Rigidbody2D body;
	float startY;

	void Start() {
		body = GetComponent<Rigidbody2D> ();
		startY = transform.position.y;
		if (upgraded) {
			GetComponent<SpriteRenderer> ().sprite = upgradedArrow;
		}
	}

	void FixedUpdate () {
		body.velocity = new Vector2 (speed, -gravity);
		// Destroys itself after passing a threshold
		if (transform.position.x > -1 * LaneManager.instance.xThreshold || (startY - transform.position.y) > 1.5f) {
			Destroy (gameObject);
		}
	}
}
