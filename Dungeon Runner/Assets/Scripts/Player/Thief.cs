using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Thief player class: melees grunts and archers, picks up 2x coins, upgrade for 4x coins
/// (coin multiplier implemented in PlayerController)
/// </summary>
public class Thief : PlayerClass {

	public override string title {get; protected set;}

	void Start() {
		title = "Thief";
		canAbility = true;
	}

	override public void Ability () {
		if (canAbility) {
			StartCoroutine (SwipeSoundCoroutine ());
			AnimatorController.instance.UseAbility ();
			Vector2 firePosition = PlayerController.instance.GetPlayerPosition();
			firePosition.x += 0.5f;
			firePosition.y += 0.5f;
			RaycastHit2D hit = Physics2D.Raycast (firePosition, Vector2.right, 1.5f);
			if (hit && hit.collider.gameObject.tag == "Enemy") {
				hit.collider.gameObject.GetComponent<Enemy> ().Damage (4);
			}
			canAbility = false;
			StartCoroutine(CooldownCoroutine ());
		}
	}

	IEnumerator SwipeSoundCoroutine() {
		yield return new WaitForSeconds (.1f);
		SoundManager.instance.swipe.Play ();
	}
}
