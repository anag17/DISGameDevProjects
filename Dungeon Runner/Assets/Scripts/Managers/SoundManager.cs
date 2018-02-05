using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton that provides access to all sounds for other scripts
/// </summary>
public class SoundManager : MonoBehaviour {

	public AudioSource fireball, uiInteraction, swordClash, arrow, coin, swipe, drums,
		enemyDeath1, enemyDeath2, enemyDeath3, tankDeath, playerDeath, hit, track1,
		track2, track3;

	public static SoundManager instance;

	// Use this for initialization
	void Start () {

		instance = this;
		AudioSource[] sounds = GetComponents<AudioSource> ();

		fireball = sounds [0];
		uiInteraction = sounds [1];
		swordClash = sounds [2];
		arrow = sounds [3];
		coin = sounds [4];
		swipe = sounds [5];
		drums = sounds [6];
		enemyDeath1 = sounds [7];
		enemyDeath2 = sounds [8];
		enemyDeath3 = sounds [9];
		tankDeath = sounds [10];
		playerDeath = sounds [11];
		hit = sounds [12];
		track1 = sounds [13];
		track2 = sounds [14];
		track3 = sounds [15];

		if (SceneManager.GetActiveScene().name == "Game") {
			// Loops a random soundtrack
			float r = Random.value;
			if (r < .33) {
				track1.Play ();
				track1.loop = true;
			} else if (r < .66) {
				track2.Play ();
				track2.loop = true;
			} else {
				track3.Play ();
				track3.loop = true;
			}	
		}
	}
}
