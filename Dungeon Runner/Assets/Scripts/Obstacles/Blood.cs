using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permanence juice for enemy death
/// </summary>
public class Blood : Obstacle {

	[SerializeField] Sprite[] sprites;

	void Start () {
		GetComponent<SpriteRenderer> ().sprite = sprites [Random.Range (0, sprites.Length)];
	}
}
