/// <summary>
/// Big subclass
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big : Customer {
	//customerhealth = 3f;

	 
	public override void OnCollisionEnter(Collision food)
	{
		// deal damage no matter the food
		if (food.gameObject.CompareTag ("Meat") || food.gameObject.CompareTag ("Veggy") || food.gameObject.CompareTag ("Fish")) {
			food.gameObject.SetActive (false);
			TakeDamage (5);
		} 

	}
}
