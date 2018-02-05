using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clean : PickupContainer {

    void Start() {
        // Leave this start statement as an override
    }

    void Update() {
        // Leave this update statement as an override
    }

    void OnTriggerEnter(Collider other) {
        // Leave this onTrigger statement as an override
    }

    override public GameObject GrabPickup() {
        return pickupPrefab;
    }

    override public int PickupsRemaining() {
        return 1;
    }
}
