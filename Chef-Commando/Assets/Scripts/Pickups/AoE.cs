using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE : Pickup {

	List<Customer> triggerList = new List<Customer>();
	[SerializeField] private ParticleSystem wineSplash;
	[SerializeField] private int radius = 3;

	void OnCollisionEnter (Collision other) {

		Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);
		foreach (Collider col in hitColliders) {
			if (col.GetComponent<Customer> ()) {
				triggerList.Add (col.GetComponent<Customer> ());
			}
		}

		wineSplash.transform.parent = null;
		wineSplash.transform.localScale = Vector3.one;
		wineSplash.Play ();

		foreach (Customer thing in triggerList) {
			thing.Drunk ();
		}

		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
