// Modified code from https://unity3d.com/learn/tutorials/projects/survival-shooter/enemy-one?playlist=17144
/// <summary>
/// The attributes and functionality of the customers
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public class Customer : MonoBehaviour {

	Transform player;               // Reference to the player's position.
	NavMeshAgent nav;               // Reference to the nav mesh agent.
	AudioSource walksound;
	public float customerhealth = 10f;
	private Transform childobj;
	private IEnumerator waitDeath;
	protected bool triggered = false;
	protected bool isDrunk = false;

	// reference to the coin
	Rigidbody coinrb;
	Collider coincol;
	[SerializeField] GameObject coin;

    private NavMeshHit hit;

	void Awake ()
	{
		// Set up the references.
		coincol = coin.GetComponent<Collider> ();
		coinrb = coin.GetComponent<Rigidbody> ();
        //		waitDeath = waitForDeath ();
		walksound = this.GetComponent<AudioSource>();
	}

	void Start (){
		player = GameObject.FindGameObjectWithTag ("Player").transform;
        childobj = gameObject.transform.GetChild(0);

        if (NavMesh.SamplePosition(transform.position, out hit, 500, NavMesh.AllAreas)) {
            transform.position = hit.position;
            nav = gameObject.AddComponent<NavMeshAgent>();
            nav.speed = 2;
            if (name.Contains("BigGuy")) {
                nav.speed = 1.5f;
            }
        }
    }
		
	void Update ()
	{
        if (nav != null && nav.isOnNavMesh) {
            nav.SetDestination(player.position);
        } else {
            Debug.LogError("Failed to place agent on NavMesh");
        }

		if (GameController.GetPaused ())
			walksound.Pause ();
    } 


	// when a custoemr gets hit with food
	public void TakeDamage(int damage)
	{
		EventManager.TriggerEvent (Events.enemyHit);
		customerhealth = customerhealth - damage;
		if (customerhealth <= 0) {

			Fed();

		//	nomnom.Play ();
		}
	}

	// condition that makes customers move faster
	protected void Angry()
	{
		nav.speed = nav.speed * 1.5f;
		childobj.gameObject.SetActive(true);
		triggered = true;
	}

	// condition that slows down custoemrs
	public void Drunk() {
		nav.speed = nav.speed * 0.5f;
		isDrunk = true;
	}

	// Customer "death"
	protected void Fed() {
		//GetComponent<Rigidbody>().velocity = Vector3.zero;
		nav.enabled = false;
		GetComponent<Animation>().Play("death");

		EventManager.TriggerEvent (Events.enemyKill);
		this.gameObject.SetActive(false);
		// drop coin at their death position
		Instantiate (coin, new Vector3(transform.position.x, transform.position.y + .75f, transform.position.z), Quaternion.AngleAxis(90, Vector3.right));
		coinrb.isKinematic = false;
		coincol.enabled = true;
		coincol.isTrigger = false;

		//Debug.Log ("NOW");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().GiveXP(5);

//		StartCoroutine (waitDeath);
    }

//	protected IEnumerator waitForDeath () {
//		
//		yield return new WaitForSeconds (1);
//		this.gameObject.SetActive (false);
//		//Destroy(gameObject);
//	}

    public virtual void OnCollisionEnter(Collision hit)
	{
		
	}

}
