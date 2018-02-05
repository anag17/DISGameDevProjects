using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player movement, collisions, class status, death
/// </summary>
public class PlayerController : MonoBehaviour {

	public float speed = 3;
	public float classDuration;
	public int startLane = 2;
	public float xInitial = -8;
	public int coinMultiplier = 1;
	bool switchingLanes = false;
	int lane;
	Rigidbody2D body;
	float posXLimit;
	public PlayerClass currentClass;
	public static PlayerController instance;

	void Start () {
		instance = this;
		lane = startLane;
		body = GetComponent<Rigidbody2D> ();
		transform.position = new Vector2 (xInitial, LaneManager.instance.laneLocations [lane]);
		currentClass = GetComponent<NoClass> ();
		HudManager.instance.cooldownBarBack.gameObject.SetActive (false);
		HudManager.instance.cooldownBarFront.gameObject.SetActive (false);
	}

	// Using Update here instead of FixedUpdate because it makes for more responsive lane switching
	void Update () {
		AnimatorController.instance.UpdateSpeed (body.velocity.x);
		// To limit player movement rightward
		posXLimit = Camera.main.ViewportToWorldPoint(new Vector3 (1, 0, 0)).x - 0.5f;
		if (GameManager.gameState != GameManager.GameState.Paused) {
			MoveLeftRight ();
			SwitchLanes ();
			// Work-around for a bug where the mage fireballs were pushing the player around
			if (!switchingLanes) {
				transform.position = new Vector2(transform.position.x, LaneManager.instance.laneLocations[lane]);
			}
		}
		if ((Input.GetKeyDown(KeyCode.J)
			|| Input.GetKeyDown(KeyCode.K)
			|| Input.GetKeyDown(KeyCode.L)
			|| Input.GetKeyDown(KeyCode.Space))
			&& GameManager.gameState != GameManager.GameState.Paused) {
			currentClass.Ability();
		}
	}

	public Vector2 GetPlayerPosition() {
		return transform.position;
	}

	void MoveLeftRight () {
		Vector2 moveVel = body.velocity;
		moveVel.x = Input.GetAxisRaw ("Horizontal") * speed * Time.deltaTime;
		// Set x-velocity to 0 if you've gone too far right
		if (moveVel.x > 0 && transform.position.x >= posXLimit ) {
			moveVel.x = 0;
		}
		// Set x-velocity to "standing still" speed if you're hitting the left arrow
		if (moveVel.x < 0) {
			moveVel.x = -2;
		}
		body.velocity = moveVel;
	}

	void SwitchLanes () {
		if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			&& lane < LaneManager.instance.laneLocations.Count - 1) {
			float nextLocation = LaneManager.instance.laneLocations [lane + 1];
			StartCoroutine (LaneSwitchCoroutine (transform.position.y, nextLocation));
			lane++;
		} else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			&& lane > 0) {
			float nextLocation = LaneManager.instance.laneLocations [lane - 1];
			StartCoroutine (LaneSwitchCoroutine (transform.position.y, nextLocation));
			lane--;
		}
	}

	IEnumerator LaneSwitchCoroutine(float currentLocation, float nextLocation) {
		switchingLanes = true;
		float time = 0;
		float distance = nextLocation - currentLocation;
		while (time < .1f) {
			time += Time.deltaTime;
			// The floating point calculation here can sometimes miscalculate and send you a greater
			// distance than you'd anticipate. Adding .02 to the time denominator prevents this overshooting.
			float location = currentLocation + (time / .12f) * distance;
			transform.position = new Vector2(transform.position.x, location);
			yield return null;
		}
		transform.position = new Vector2(transform.position.x, nextLocation);
		switchingLanes = false;
	}

	void NewClass(Collision2D other) {
		Destroy (other.gameObject);
		HudManager.instance.hourglass.gameObject.SetActive (false);
		coinMultiplier = 1;
		SoundManager.instance.drums.Play ();
		StopCoroutine ("ClassTimerCoroutine");
		StartCoroutine ("ClassTimerCoroutine");
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Enemy" || (other.gameObject.tag == "EnemyArrow" && other.gameObject.GetComponent<Arrow> ().speed < 0)) {
			Die ();
		} else if (other.gameObject.tag == "Coin") {
			Destroy (other.gameObject);
			ScoreManager.instance.AddCoins(coinMultiplier, false);
		} else if (other.gameObject.tag == "Chest") {
			Destroy (other.gameObject);
			ScoreManager.instance.AddCoins (3 * coinMultiplier, true);
		} else if (other.gameObject.tag == "Warrior") {
			NewClass (other);
			currentClass = gameObject.GetComponent<Warrior> ();
			if (gameObject.GetComponent<Warrior> ().upgraded) {
				AnimatorController.instance.UpdateClass (5);
			} else {
				AnimatorController.instance.UpdateClass (4);
			}
		} else if (other.gameObject.tag == "Ranger") {
			NewClass (other);
			currentClass = gameObject.GetComponent<Ranger> ();
			AnimatorController.instance.UpdateClass (2);
		} else if (other.gameObject.tag == "Mage") {
			NewClass (other);
			currentClass = gameObject.GetComponent<Mage> ();
			AnimatorController.instance.UpdateClass (1);
		} else if (other.gameObject.tag == "Thief") {
			NewClass (other);
			currentClass = gameObject.GetComponent<Thief> ();
			if (currentClass.upgraded) {
				coinMultiplier = 4;
			} else {
				coinMultiplier = 2;
			}
			AnimatorController.instance.UpdateClass (3);
		}
	}

	IEnumerator ClassTimerCoroutine () {

		// Class begins
		HudManager.instance.cooldownBarBack.gameObject.SetActive (true);
		HudManager.instance.cooldownBarFront.gameObject.SetActive (true);

		// Flash an hourglass image when class is going to time out
		yield return new WaitForSeconds (classDuration - 2);
		for (int i = 0; i < 4; i++) {
			HudManager.instance.hourglass.gameObject.SetActive (true);
			yield return new WaitForSeconds (.25f);
			HudManager.instance.hourglass.gameObject.SetActive (false);
			yield return new WaitForSeconds (.25f);
		}

		// Class ends
		currentClass = gameObject.GetComponent<NoClass> ();
		AnimatorController.instance.UpdateClass (0);
		HudManager.instance.cooldownBarFront.GetComponent<CooldownBar> ().Reset ();
		HudManager.instance.cooldownBarBack.gameObject.SetActive (false);
		HudManager.instance.cooldownBarFront.gameObject.SetActive (false);
		coinMultiplier = 1;
		gameObject.GetComponent<Ranger> ().arrowPrefab.GetComponent<Arrow> ().speed = 10;
		gameObject.GetComponent<Ranger> ().cooldown = 1;
	}

	void Die () {
		SoundManager.instance.track1.Stop ();
		SoundManager.instance.track2.Stop ();
		SoundManager.instance.track3.Stop ();
		DeathReport.instance.LoadDeathReport();
	}
}
