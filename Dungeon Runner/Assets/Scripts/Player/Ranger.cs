using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ranger player class: shoots arrows, upgrade shoots piercing arrows and improves with each shot
/// </summary>
public class Ranger : PlayerClass {

	public GameObject arrowPrefab;
	public override string title {get; protected set;}

	void Start() {
		title = "Ranger";
		canAbility = true;
	}

	override public void Ability () {
		if (canAbility) {
			AnimatorController.instance.UseAbility ();
			StartCoroutine (ShotCoroutine ());
			canAbility = false;
			if (upgraded) {
				cooldown -= .08f;
			}
			StartCoroutine (CooldownCoroutine ());
		}
	}

	IEnumerator ShotCoroutine() {
		yield return new WaitForSeconds (.3f);
		SoundManager.instance.arrow.Play ();
		if (upgraded) {
			arrowPrefab.GetComponent<Arrow> ().speed += 5;
		}
		// Upgraded arrows have a piercing effect implemented in enemy colliders
		arrowPrefab.GetComponent<Arrow> ().upgraded = upgraded;
		Vector2 firePosition = PlayerController.instance.GetPlayerPosition ();
		firePosition.y += 1.5f;
		Instantiate (arrowPrefab, firePosition, Quaternion.AngleAxis (90, Vector3.back));
	}
}
