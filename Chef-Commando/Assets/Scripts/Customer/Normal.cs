/// <summary>
/// Normal Subclass from Customer 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Customer {

	public override void OnCollisionEnter(Collision hit)
	{
		// deal damage if hit with burger
		if (hit.gameObject.CompareTag ("Meat")) {
			hit.gameObject.SetActive (false);
			TakeDamage (10);

		} else if ((hit.gameObject.CompareTag("Veggy") || hit.gameObject.CompareTag("Fish")) && !triggered) {
			Angry ();
		}
	}
}
