using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupContainer : Pickup {
    [SerializeField] private Text[] counters = new Text[4];
    [SerializeField] private MeshRenderer pickupSpinner;
    [SerializeField] private Material zeroRemaining;


    private Material ogMaterial;

    void Start() {
        if (counters != null) {
            SetCounters(pickupsRemaining);
        }
        if (pickupSpinner != null) {
            ogMaterial = pickupSpinner.sharedMaterial;
        }
    }

    void Update() {
        // Leave this update statement as an override
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Money") && other.gameObject.GetComponent<Rigidbody>().isKinematic == false) {
            if (pickupsRemaining == 0 && pickupSpinner != null) {
                pickupSpinner.material = ogMaterial;
            }
            pickupsRemaining++;
            SetCounters(pickupsRemaining);
        }
    }

    override public GameObject GrabPickup() {
        if (pickupsRemaining > 0 || prefabName == "Clean") {
            pickupsRemaining -= 1;
            if (pickupsRemaining == 0 && pickupSpinner != null) {
                pickupSpinner.material = zeroRemaining;
            }
            if (counters != null) {
                SetCounters(pickupsRemaining);
            }
            return pickupPrefab;
        } else {
            return null;
        }
    }

    override public int PickupsRemaining() {
        return pickupsRemaining;
    }

    private void SetCounters(int count) {
        string countStr = count.ToString();
        foreach(Text counter in counters) {
            counter.text = countStr;
        }
    }
}
