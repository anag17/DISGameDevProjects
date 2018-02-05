using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates screen shake on tank death
/// </summary>
public class CameraController : MonoBehaviour {

	static float shakeTimer, shakeStrength;
	Vector3 originalPosition = new Vector3(0, 0, -10);
	
	void Update () {
		Vector3 newPosition = transform.position;
		if (shakeTimer > 0) {
			newPosition += Random.onUnitSphere * shakeStrength;
			shakeTimer -= Time.deltaTime;
		} else {
			newPosition = originalPosition;
		}
		newPosition.z = -10;
		transform.position = newPosition;
	}

	public static void ScreenShake(float strength, float shakeTime) {
		shakeStrength = strength;
		shakeTimer = shakeTime;
	}
}