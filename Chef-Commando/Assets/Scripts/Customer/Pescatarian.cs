/// <summary>
/// Pescatarian subclass
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pescatarian : Customer {

	public override void OnCollisionEnter(Collision hit)
	{
        // deal damage if hit with fish item
        if (hit.gameObject.CompareTag("Fish")) {
            hit.gameObject.SetActive(false);
            TakeDamage(10);
        }
        else if ((hit.gameObject.CompareTag("Meat") || hit.gameObject.CompareTag("Veggy")) && !triggered) {
            Angry();
        }
	}

}
