/// <summary>
/// Background music for the game
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

	[SerializeField] AudioClip electroPop;
	AudioSource backgroundMusic;
	// Use this for initialization
	void Start () {
		backgroundMusic = GetComponent<AudioSource> ();
		backgroundMusic.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		

		if (GameController.GetPaused())
			backgroundMusic.Pause ();
		if (!GameController.GetPaused ())
			backgroundMusic.UnPause ();
}

}