using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Superclass for player classes (e.g. mage, warrior): mostly just an ability cooldown they all use
/// </summary>
public class PlayerClass : MonoBehaviour {

	public float cooldown = 1;
	public virtual string title {get; protected set;}

	protected bool canAbility;
	public bool upgraded;

	public virtual void Ability () {
	}

	protected IEnumerator CooldownCoroutine () {
		HudManager.instance.cooldownBarFront.GetComponent<CooldownBar>().Cooldown(cooldown);
		yield return new WaitForSeconds (cooldown);
		canAbility = true;
	}
}
