using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pushes player towards their demise
/// Provides player cover from enemy fire
/// </summary>
public class Boulder : Obstacle {

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "EnemyArrow") {
			Destroy (other.gameObject);
		}
	}
}
