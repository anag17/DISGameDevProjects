using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows us to access parameters in player animator
/// </summary>
public class AnimatorController : MonoBehaviour {

	public static AnimatorController instance;
	Animator anim;

	void Start () {
		instance = this;
		anim = this.gameObject.GetComponent<Animator> ();
	}

	public void UpdateSpeed (float currentSpeed) {
		anim.SetFloat ("Speed", currentSpeed);
	}

	public void UpdateClass (int newClass) {
		anim.SetInteger ("Class", newClass);
	}

	public void UseAbility () {
		anim.SetTrigger ("Ability");
	}
}
