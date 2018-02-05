using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mage player class: shoots fireballs, shoots more with longer cooldown when upgraded
/// </summary>
public class Mage : PlayerClass {

	public GameObject fireballPrefab;
	public override string title {get; protected set;}

	void Start() {
		title = "Mage";
		canAbility = true;
	}

	override public void Ability () {
		if (canAbility) {
			SoundManager.instance.fireball.Play ();
			AnimatorController.instance.UseAbility ();
			StartCoroutine (ShotCoroutine ());

			// Disallow attacking for the duration of the cooldown
			canAbility = false;
			if (upgraded) {
				cooldown = 3;
			}
			StartCoroutine(CooldownCoroutine ());
		}
	}

	void Fire() {
		Vector2 firePosition = PlayerController.instance.GetPlayerPosition();
		firePosition.y += .5f;
		Instantiate(fireballPrefab, firePosition, Quaternion.identity);
	}

	IEnumerator ShotCoroutine() {
		if (upgraded) {
			Fire ();
		}
		yield return new WaitForSeconds (.1f);
		if (upgraded) {
			Fire ();
		}
		yield return new WaitForSeconds (.1f);
		Fire ();
	}
}
