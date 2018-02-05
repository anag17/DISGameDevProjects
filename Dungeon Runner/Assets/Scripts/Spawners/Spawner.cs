using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawner superclass: methods for spawning random, specific, and tank prefabs
/// </summary>
public class Spawner : MonoBehaviour {

	protected System.Random random = new System.Random();
		
	protected void SpawnRandomPrefab(params GameObject[] prefabs) {
		if (LaneManager.instance.IsFreeLane()) {
			GameObject prefab = prefabs [random.Next (prefabs.Length)];
			float location = LaneManager.instance.GetFreeLane (false);
			Instantiate (prefab, new Vector2 (10, location), Quaternion.identity);
			StartCoroutine (LaneManager.instance.DisableSpawningCoroutine(location, false));
		}
	}

	protected void SpawnPrefab(GameObject prefab) {
		if (LaneManager.instance.IsFreeLane()) {
			float location = LaneManager.instance.GetFreeLane(false);
			Instantiate (prefab, new Vector2 (10, location), Quaternion.identity);
			StartCoroutine (LaneManager.instance.DisableSpawningCoroutine(location, false));
		}
	}

	protected void SpawnTank(GameObject prefab) {
		if (LaneManager.instance.TwoAdjacentLanesFree()) {
			float location = LaneManager.instance.GetFreeLane (true);
			Instantiate (prefab, new Vector2 (12, location), Quaternion.identity);
			StartCoroutine (LaneManager.instance.DisableSpawningCoroutine(location, true));
		}
	}
}