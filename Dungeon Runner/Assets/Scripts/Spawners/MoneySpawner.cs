using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns coins and chests
/// </summary>
public class MoneySpawner : Spawner {

	public float firstSpawn;
	public float spawnEvery;
	public float chanceOfChest;

	public GameObject coinPrefab;
	public GameObject chestPrefab;

	void Start () {
		StartCoroutine(SpawnCoroutine());
	}
	
	IEnumerator SpawnCoroutine() {
		yield return new WaitForSeconds (firstSpawn);
		SpawnPrefab(coinPrefab);
		while (true) {
			yield return new WaitForSeconds (spawnEvery);
			if (Random.value < chanceOfChest) {
				SpawnPrefab (chestPrefab);
			}
			else {
				SpawnPrefab (coinPrefab);
			}
		}
	}
}
