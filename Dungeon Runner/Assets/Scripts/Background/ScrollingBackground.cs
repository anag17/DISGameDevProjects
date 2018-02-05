using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Causes background image of dungeon to scroll sideways
/// </summary>
public class ScrollingBackground : MonoBehaviour {

	private Rigidbody2D body;
	public float scrollSpeed;

	void Start () {
		body = GetComponent<Rigidbody2D>();
		body.velocity = new Vector2 (-scrollSpeed, 0);
	}
}
