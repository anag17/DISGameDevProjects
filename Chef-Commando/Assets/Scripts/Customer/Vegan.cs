/// <summary>
/// Vegan subclass
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegan : Customer {


	public override void OnCollisionEnter(Collision hit)
	{
        // deal damage if hit with veggy
        if (hit.gameObject.CompareTag("Veggy")) {
            hit.gameObject.SetActive(false);
            TakeDamage(10);
        }
        else if ((hit.gameObject.CompareTag("Meat") || hit.gameObject.CompareTag("Fish")) && !triggered) {
            Angry();
        }
	}
}
