/// <summary>
/// Script for the hitmarker effect
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitmarker : MonoBehaviour {

	AudioSource hitnoise;
	private	Transform marker;

	// Start lisitening for a hit
	void OnEnable ()
	{
		EventManager.StartListening (Events.enemyHit, Mark);
	}

	void OnDisable ()
	{
		EventManager.StopListening (Events.enemyHit, Mark);
	}

	// Use this for initialization
	void Start () {
		marker = gameObject.transform.GetChild(0);
		marker.gameObject.SetActive (false);
		hitnoise = GetComponent<AudioSource> ();

	}

	// when a hit is detected
	void Mark(){
		hitnoise.Play ();
		marker.gameObject.SetActive (true);
		StartCoroutine (HitDisplay(.3f));
	}

	// corouitine for how long to display hitmarker
	IEnumerator HitDisplay(float time){

		yield return new WaitForSeconds(time);
		marker.gameObject.SetActive (false);
	}

}
