using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public float speed = 50f;

	void Update () {
		transform.Rotate (Vector3.right, speed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Interactable") && GetComponent<Rigidbody>().isKinematic == false) {
            Destroy(gameObject);
        }
    }
}
