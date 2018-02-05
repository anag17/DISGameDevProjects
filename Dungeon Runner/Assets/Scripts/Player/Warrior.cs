using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Warrior player class: melees grunts and archers and boulders, upgrade to melee tanks
/// </summary>
public class Warrior : PlayerClass {

	public override string title {get; protected set;}

	void Start() {
		title = "Warrior";
		canAbility = true;
	}

	override public void Ability () {
		if (canAbility) {
			StartCoroutine (SwipeSoundCoroutine());
			AnimatorController.instance.UseAbility ();
			Vector2 firePosition = PlayerController.instance.GetPlayerPosition();
			firePosition.x += 0.5f;
			firePosition.y += 0.5f;
			RaycastHit2D hit = Physics2D.Raycast (firePosition, Vector3.right, 3);
			if (hit && hit.collider.gameObject.tag == "Enemy") {
				if (upgraded) {
					hit.collider.GetComponent<Enemy> ().Damage (8);
				} else {
					hit.collider.GetComponent<Enemy> ().Damage (4);
				}
			} else if (hit && hit.collider.gameObject.tag == "Boulder") {
				Destroy (hit.collider.gameObject);
			}
			canAbility = false;
			StartCoroutine(CooldownCoroutine ());
		}
	}

	IEnumerator	SwipeSoundCoroutine() {
		if (!upgraded) {
			yield return new WaitForSeconds (.25f);
		}
		SoundManager.instance.swipe.Play ();
	}
}
