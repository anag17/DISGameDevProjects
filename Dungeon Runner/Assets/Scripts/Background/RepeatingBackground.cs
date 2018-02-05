using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {

	private float backgroundHorizontalLength;

	void Awake () {
		backgroundHorizontalLength = 17.8f;
	}

	void FixedUpdate () {
		//checks if the background is completely left of the main camera.
		if (transform.position.x < -(backgroundHorizontalLength)) {
			RepositionBackground ();
		}
	}

	/// <summary>
	/// Repositions the background to the right of the next background sprite.
	/// </summary>
	private void RepositionBackground() {
		Vector2 backgroundOffSet = new Vector2(backgroundHorizontalLength, 0);
		transform.position = (Vector2)transform.position+ backgroundOffSet*2f;
	}

}
