using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfaces with spawners to give them free lane locations
/// Regulates spawning to ensure things don't spawn on top of one another
/// </summary>
public class LaneManager : MonoBehaviour {

	/// <summary>
	/// y-value of each lane
	/// </summary>
	public List<float> laneLocations;

	/// <summary>
	/// Seconds before lane opens again
	/// </summary>
	public float spawnDelay;

	public List<bool> laneIsOpen = new List<bool> {true, true, true, true};
	public float xThreshold = -17;
	public static LaneManager instance;
	
	void Awake() {
		instance = this;
	}
		
	/// <summary>
	/// Closes a lane for spawnDelay seconds after a spawn
	/// This ensures things don't spawn on top of each other
	/// </summary>
	/// <param name="location">y-value of lane</param>
	/// <param name="tank">Set to true if spawning a tank so we can close two lanes</param>
	public IEnumerator DisableSpawningCoroutine(float location, bool tank) {
		int laneIndex = laneLocations.IndexOf (location);
		laneIsOpen [laneIndex] = false;
		if (tank) {
			laneIsOpen [laneIndex + 1] = false;
		}
		yield return new WaitForSeconds (spawnDelay);
		laneIsOpen [laneIndex] = true;
		if (tank) {
			laneIsOpen [laneIndex + 1] = true;
		}
	}

	public bool IsFreeLane() {
		return laneIsOpen.Contains (true) ? true : false;
	}

	/// <summary>
	/// For tank spawning purposes
	/// </summary>
	/// <returns>True if two adjacent lanes are free, false otherwise</returns>
	public bool TwoAdjacentLanesFree() {
		for (int i = 0; i <= laneIsOpen.Count - 2; i++) {
			if (laneIsOpen[i] && laneIsOpen[i + 1]) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Searches randomly for a free lane
	/// If spawning a tank, ensures above lane is free too
	/// </summary>
	/// <returns>The location of the free lane</returns>
	/// <param name="tank">Set to true if spawning a tank</param>
	public float GetFreeLane(bool tank) {
		System.Random random = new System.Random ();
		int laneIndex;
		while (true) {
			if (tank) {
				laneIndex = random.Next (laneLocations.Count - 1);
			} else {
				laneIndex = random.Next (laneLocations.Count);
			}
			if (laneIsOpen[laneIndex]) {
				if (tank && laneIsOpen[laneIndex + 1]) {
					return laneLocations [laneIndex];
				} else if (!tank) {
					return laneLocations[laneIndex];
				}
			}
		}
	}
}
