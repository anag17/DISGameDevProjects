using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies and obstacles using a probability-based leveling system that progresses over time
/// </summary>
public class EnemySpawner : Spawner {

	/// <summary>
	/// All the enemy and obstacle prefabs
	/// </summary>
	List<GameObject> prefabs;
	const int tankIndex = 0;
	public GameObject tankPrefab;
	public GameObject archerPrefab;
	public GameObject gruntPrefab;
	public GameObject firePrefab;
	public GameObject boulderPrefab;

	/// <summary>
	/// Time at which each level begins
	/// </summary>
	[SerializeField] List<float> levelStarts = new List<float>();

	/// <summary>
	/// Seconds between spawns for each level
	/// </summary>
	[SerializeField] List<float> spawnEvery = new List<float>();

	// Example: { .1, .2, .4, .1, .2 } 
	// Means: During this level, each spawn has a 10% chance of being a tank, a 20% chance of being an archer, etc.
	// Probabilities in a given level should sum to one
	[SerializeField] List<float> level0TankArcherGruntFireBoulder = new List<float>();
	[SerializeField] List<float> level1TankArcherGruntFireBoulder = new List<float>();
	[SerializeField] List<float> level2TankArcherGruntFireBoulder = new List<float>();
	[SerializeField] List<float> level3TankArcherGruntFireBoulder = new List<float>();
	[SerializeField] List<float> level4TankArcherGruntFireBoulder = new List<float>();
	[SerializeField] List<float> level5TankArcherGruntFireBoulder = new List<float>();
	[SerializeField] List<float> level6TankArcherGruntFireBoulder = new List<float>();
	List<List<float>> levels;

	void Start () {
		prefabs = new List<GameObject> { tankPrefab, archerPrefab, gruntPrefab, firePrefab, boulderPrefab };
		levels = new List<List<float>> {
			level0TankArcherGruntFireBoulder,
			level1TankArcherGruntFireBoulder,
			level2TankArcherGruntFireBoulder,
			level3TankArcherGruntFireBoulder,
			level4TankArcherGruntFireBoulder,
			level5TankArcherGruntFireBoulder,
			level6TankArcherGruntFireBoulder
		};
		StartCoroutine(SpawnCoroutine());
	}

	IEnumerator SpawnCoroutine() {

		// For each level
		for (int level = 0; level < levels.Count; level++) {

			// For the duration of the level
			while (level == levels.Count - 1 || Time.timeSinceLevelLoad < levelStarts[level + 1]) {

				// Spawn something every "spawnEvery" seconds, plus or minus .75 so things don't look too orderly
				yield return new WaitForSeconds (spawnEvery [level] + (Random.value * 1.5f) - .75f);

				// Spawn a prefab from the level
				// Chance of each spawn is determined by the level array
				float r = Random.value;
				float probabilitySum = 0;
				for (int prefabIndex = 0; prefabIndex < prefabs.Count; prefabIndex++) {
					float prefabChance = levels[level][prefabIndex];
					if (probabilitySum < r && r < probabilitySum + prefabChance) {
						if (prefabIndex == tankIndex) {
							SpawnTank (prefabs [tankIndex]);
						} else {
							SpawnPrefab (prefabs [prefabIndex]);
						}
					}
					probabilitySum += prefabChance;
				}
			}
		}
	}
}
