/// <summary>
/// Pickup.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, System.IEquatable<Pickup>, System.IComparable<Pickup> {
    [SerializeField] protected GameObject pickupPrefab;
    [SerializeField] protected int pickupsRemaining = 1;

    public bool isMeal;
    public string prefabName;

    protected Collider col;
    protected Rigidbody rb;
    protected Transform playerTransform;
    protected bool hitGround = false;

    private Vector3 initForward;
    private bool deleteCoins;

    void Start() {
        pickupPrefab = Resources.Load(prefabName) as GameObject;
        while (playerTransform == null) {
            playerTransform = GameController.GetPlayer().transform;
        }

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        initForward = rb.transform.forward;
        deleteCoins = true;
        StartCoroutine(DeleteCoinCoroutine());
    }

    void Update() {
        if (isMeal == true && hitGround == true) {
            float distance = Mathf.Abs(Vector3.Distance(transform.position, playerTransform.position));
            if (distance < 5) {
                rb.isKinematic = true;
                col.isTrigger = true;
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, (7 / distance) * Time.deltaTime);
            } else {
                rb.isKinematic = false;
                col.isTrigger = false;
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == 9) {
            hitGround = true;
            if (prefabName == "coin") {
                rb.isKinematic = true;
            }
        }

        // Workaround for a bug in which a coin would spawn along with thrown objects, affecting their trajectory.
        // Deletes all colliding coins within 0.1 seconds of instantiation and the corrects the trajectory by re-instantiating the thrown object.
        if (other.gameObject.CompareTag("Money") && deleteCoins) {
            Destroy(other.gameObject);
            GameObject obj = GameObject.Instantiate(pickupPrefab, GetComponent<Transform>());
            obj.GetComponent<Pickup>().SetPickups(1);
            obj.transform.parent = null;
            obj.GetComponent<Collider>().enabled = true;
            Rigidbody heldRB = obj.GetComponent<Rigidbody>();
            heldRB.isKinematic = false;

            Vector3 throwDir = new Vector3(initForward.x, initForward.y + 0.2f, initForward.z);
            heldRB.AddForce(throwDir * GameObject.FindGameObjectWithTag("Player").GetComponent<GrabThrow>().throwSpeed);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            playerTransform.GetComponent<GrabThrow>().AddAmmo(this);
            Destroy(gameObject);
        }
    }

    public virtual GameObject GrabPickup() {
        Destroy(gameObject);
        return pickupPrefab;
    }

    public virtual int PickupsRemaining() {
        return pickupsRemaining;
    }

    public virtual void SetPickups(int pickups) {
        pickupsRemaining = pickups;
    }

    public GameObject GetPrefab() {
        return pickupPrefab;
    }

    public bool Equals(Pickup other) {
        if (other.prefabName == this.prefabName) {
            return true;
        } else {
            return false;
        }
    }

    override public int GetHashCode() {
        return prefabName.GetHashCode();
    }

    public int CompareTo(Pickup other) {
        return this.prefabName.CompareTo(other.prefabName);
    }

    public Pickup Copy() {
        return (Pickup)this.MemberwiseClone();
    }

    IEnumerator DeleteCoinCoroutine() {
        yield return new WaitForSeconds(0.1f);
        deleteCoins = false;
    }
}
