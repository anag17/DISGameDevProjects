//
// Modified from https://unity3d.com/learn/tutorials/projects/survival-shooter/more-enemies
// and forum.unity.com/threads/enemy-spawn-wave-system.195730/
/// <summary>
/// Manages the wave system 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour {

	public List<GameObject> enemyprefabs = new List<GameObject>(); // A list of enemy prefab to be spawned.
		
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

	AudioSource nomnom;


	//floats and ints
	public float timeBetweenWaves = 10f; // time to prepare for next wave
	public float spawnTime = 3f;            // How long between each spawn.
	public int wave = 1;
	public int startCustcount = 3;
	public int addPerwave = 2;

	private int custCount = 0; //number of enemies that are alive
	private int custSpawned = 0;
	[SerializeField] private AudioClip beep;

	public Text waveText;




	void Start () {
		nomnom = GetComponent<AudioSource> ();
		waveText.text = "Wave: 0";
		Invoke ("StartWave", timeBetweenWaves);
	}


	void OnEnable () {
		EventManager.StartListening (Events.enemyKill, MinusCount);
	}

	void OnDisable () {
		EventManager.StopListening (Events.enemyKill, MinusCount);
	}

	// Starts the wave
	void StartWave(){
		waveText.text = "Wave: " + wave;
		InvokeRepeating ("SpawnCustomers", spawnTime, spawnTime);
		nomnom.PlayOneShot (beep);
	}




	void EndWave() {
		wave++;
		custCount = 0;
		custSpawned = 0;
		// Start new wave
		Invoke ("StartWave", timeBetweenWaves);
	}

	void CheckCount(){
	    if (custSpawned >= (startCustcount + wave * addPerwave)) {
			CancelInvoke ("SpawnCustomers");
		}

		// need the enemycounter to decrease when customers are fed
		if (custCount < 1)
			EndWave ();

	}

	// subtracts from the wavecount
	void MinusCount()
	{
		nomnom.Play ();
		custCount--;

		//StartCoroutine (CountCheckDelay (2f));
		CheckCount();
	}


		void SpawnCustomers ()
		{
		int spawnPointIndex;
		int rand;
		if (wave < 2) {
			spawnPointIndex = Random.Range (0, spawnPoints.Length / 2);
			rand = Random.Range (0, 1);
		}
		else {
			// Find a random index between zero and one less than the number of spawn points.
			spawnPointIndex = Random.Range (0, spawnPoints.Length);
			rand = Random.Range (0, enemyprefabs.Count);
		}
		// picks a random enemy to spawn
		 
			// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		//while (custSpawned < startCustcount + wave * addPerwave) {
		Instantiate (enemyprefabs [rand], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
		
			// increment counts
			custSpawned++;
			custCount++;
	//	}
		CheckCount (); // check on the count of customers
		}

	IEnumerator CountCheckDelay(float time)
	{
		yield return new WaitForSeconds(time);
		CheckCount ();
	}

	IEnumerator WaveDelay(float time)
	{
		yield return new WaitForSeconds (time);
		EndWave ();
	}
}

