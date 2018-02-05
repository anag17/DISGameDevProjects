using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bloom : MonoBehaviour {
    [SerializeField] Image largeReticule;

    void Update() {
        RaycastHit hitInfo;
        // Raycast from the center of the screen two units forward
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 2)
                && hitInfo.transform.GetComponentInParent<Pickup>() != null
                && hitInfo.transform.GetComponentInParent<Pickup>().PickupsRemaining() > 0) {
            largeReticule.gameObject.SetActive(true);
        } else {
            largeReticule.gameObject.SetActive(false);
        }
    }
}
