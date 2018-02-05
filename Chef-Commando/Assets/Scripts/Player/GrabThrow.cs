using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GrabThrow : MonoBehaviour {
    public float throwSpeed = 500;

    [SerializeField] private List<AmmoType> ammoTypes;
    [SerializeField] private Transform pickupSpawn;
    [SerializeField] private Pickup nullablePickup;

    private AudioSource throwsound;
    private GameObject heldObject;
    private Dictionary<Pickup, int> ammoRemaining;
    private Dictionary<Pickup, Text> ammoDisplay;
    private List<Pickup> ammoOrder;
    private RigidbodyFirstPersonController rbfpc;
    private bool holdingItem = false;
    private int ammoSelected = 0;
    private float forwardSpeed;
    private float backwardSpeed;
    private float strafeSpeed;
    private float jumpForce;
    private float health;

    void Awake() {
        ammoRemaining = new Dictionary<Pickup, int>();
        ammoOrder = new List<Pickup> {
            nullablePickup        // Save space for a held item
        };

        ammoRemaining.Add(ammoOrder[0], 0);

        ammoDisplay = new Dictionary<Pickup, Text>();
        foreach (AmmoType ammo in ammoTypes) {
            ammoDisplay.Add(ammo.ammo, ammo.text);
        }
    }

    void Start() {
        throwsound = GetComponent<AudioSource>();
    }

    void Update() {
        if (!GameController.GetPaused()) {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
                if (!holdingItem) {
                    PickUp();
                } else {
                    Throw();
                    if (ammoRemaining[ammoOrder[ammoSelected]] > 0) {
                        holdingItem = true;
                    } else {
                        holdingItem = false;
                    }
                }
            }

            if (Input.mouseScrollDelta.magnitude > 0 || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) {
                Destroy(heldObject);

                if (ammoSelected != 0) {
                    ammoDisplay[ammoOrder[ammoSelected]].color = Color.white;
                }

                if (Input.mouseScrollDelta.magnitude > 0) {
                    ammoSelected = ammoSelected + (int)Input.mouseScrollDelta.y;
                } else if (Input.GetKeyDown(KeyCode.Q)) {
                    ammoSelected--;
                } else if (Input.GetKeyDown(KeyCode.E)) {
                    ammoSelected++;
                }

                if (ammoSelected < 0) {
                    ammoSelected = ammoOrder.Count - 1;
                } else {
                    ammoSelected = ammoSelected % ammoOrder.Count;
                }

                if ((ammoSelected == 0 && ammoRemaining[ammoOrder[0]] > 0) || ammoSelected > 0) {
                    heldObject = GameObject.Instantiate(ammoOrder[ammoSelected].GetPrefab(), pickupSpawn);
                    heldObject.GetComponent<Collider>().enabled = false;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    holdingItem = true;
                }

                if (ammoSelected != 0) {
                    ammoDisplay[ammoOrder[ammoSelected]].color = Color.yellow;
                }

                if (ammoRemaining[ammoOrder[ammoSelected]] > 0) {
                    holdingItem = true;
                } else {
                    holdingItem = false;
                }
            }
        }
    }

    private void PickUp() {
        RaycastHit hitInfo;
        // Raycast from the center of the screen two units forward
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 2)) {
            Pickup pickup = hitInfo.transform.GetComponentInParent<Pickup>();
            if (pickup != null && !pickup.isMeal) {
                if (!new List<Pickup>(ammoRemaining.Keys).Contains(pickup)) {
                    ammoRemaining.Add(pickup.Copy(), 1);   // pickup.Copy?
                }
                ammoRemaining[pickup.Copy()] = 1;      // pickup.Copy?
                ammoOrder[0] = pickup.Copy();      // pickup.Copy?
                GameObject pickupObj = pickup.GrabPickup();
                if (pickupObj != null) {
                    heldObject = GameObject.Instantiate(pickupObj, pickupSpawn);
                    holdingItem = true;
                }
            }
        }
    }

    private void Throw() {
        if (AmmoCheck() && heldObject != null) {
            GameObject objPrefab = null;
            if (ammoRemaining[heldObject.GetComponent<Pickup>()] > 0) {
                objPrefab = heldObject.GetComponent<Pickup>().GetPrefab();
            } else {
                objPrefab = heldObject.GetComponent<Pickup>().GrabPickup();
            }
            GameObject obj = GameObject.Instantiate(objPrefab, pickupSpawn);

            obj.GetComponent<Pickup>().SetPickups(1);

            obj.transform.parent = null;
            obj.GetComponent<Collider>().enabled = true;

            Rigidbody heldRB = obj.GetComponent<Rigidbody>();
            heldRB.isKinematic = false;

            Vector3 forward = heldRB.transform.forward;
            Vector3 throwDir = new Vector3(forward.x, forward.y + 0.2f, forward.z);

            if (Input.GetButtonDown("Fire1")) {
                heldRB.AddForce(throwDir * throwSpeed);
                throwsound.Play();
            } else if (Input.GetButtonDown("Fire2")) {
                heldRB.AddForce(throwDir * 150);
                throwsound.Play();
            }

            if (ammoRemaining[heldObject.GetComponent<Pickup>()] == 0) {
                if (ammoRemaining[ammoOrder[ammoSelected]] != 0) {
                    heldObject = GameObject.Instantiate(ammoOrder[ammoSelected].GetPrefab(), pickupSpawn);
                }
            }
        }
    }

    private bool AmmoCheck() {
        Pickup ammo = ammoOrder[ammoSelected];
        if (ammoSelected > 0) {
            ammoRemaining[ammo]--;
            if (ammoRemaining[ammo] == 0) {
                ammoDisplay[ammoOrder[ammoSelected]].color = Color.white;
                ammoOrder.RemoveAt(ammoSelected);
                ammoSelected--;
                if (ammo.isMeal) {
                    ammoDisplay[ammo].text = ammoRemaining[ammo].ToString();
                }
                return true;
            } else if (ammoRemaining[ammo] > 0) {
                if (ammo.isMeal) {
                    ammoDisplay[ammo].text = ammoRemaining[ammo].ToString();
                }
                return true;
            } else {
                Debug.LogError("Unexpected ammo remaining value");
                return false;
            }
        } else if (ammoSelected == 0) {
            if (ammoRemaining[ammo] > 0) {
                ammoRemaining[ammo] = 0;
                if (ammo.isMeal) {
                    ammoDisplay[ammo].text = ammoRemaining[ammo].ToString();
                }
                return true;
            } else {
                return false;
            }
        } else {
            Debug.LogError("Unexpected ammoSelected value: " + ammoSelected + ". Value should never be negative.");
            return false;
        }
    }

    public void AddAmmo(Pickup ammo) {
        int roundsLeft = 0;
        int ammoToAdd = ammo.PickupsRemaining();

        if (ammoRemaining.TryGetValue(ammo, out roundsLeft)) {
            ammoRemaining[ammo] = roundsLeft + ammoToAdd;
        } else {
            ammoRemaining.Add(ammo, ammoToAdd);
        }

        if (!ammoOrder.Contains(ammo)) {
            ammoOrder.Add(ammo);
        }

        ammoDisplay[ammo].text = ammoRemaining[ammo].ToString();
    }
}

[System.Serializable]
class AmmoType {
    public Pickup ammo = null;
    public Text text = null;
}
