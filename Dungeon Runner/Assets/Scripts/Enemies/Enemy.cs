using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy superclass: base movement, health, projectile damage, death, juice for hitting and killing enemies
/// </summary>
public class Enemy : MonoBehaviour {

	public float baseSpeed = 2;
	public float runSpeed;
	public bool defeatable;
	public int health;
	public int pointValue;
	protected Rigidbody2D body;
	public GameObject bloodPrefab;
	Coroutine hurtRoutine;
	SpriteRenderer[] sr;
	[SerializeField] float hurtTimer = 0.1f;

	void Awake () {
		body = GetComponent<Rigidbody2D> ();
		body.velocity = new Vector2 (-1 * (baseSpeed + runSpeed), 0);
		sr = GetComponentsInChildren<SpriteRenderer> ();
	}
	
	protected void Update () {
		if (transform.position.x < LaneManager.instance.xThreshold) {
			Destroy (gameObject);
		}
		if (health <= 0) {
			Die ();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Arrow" && other.gameObject.GetComponent<Arrow> ().speed > 0) {
			//health -= 1;
			Damage(1);
			if (health > 0) {
				SoundManager.instance.hit.Play ();
			}
			if (!other.gameObject.GetComponent<Arrow> ().upgraded) {
				Destroy (other.gameObject);
			}
		} else if (other.gameObject.tag == "Fireball") {
			//health -= 2;
			Damage(2);
			if (health > 0) {
				SoundManager.instance.hit.Play ();
			}
			Destroy (other.gameObject);
		}
	}

	protected virtual void Die() {
		ScoreManager.instance.IncrementScore(pointValue);
		float r = Random.value;
		if (r <= .33) {
			SoundManager.instance.enemyDeath1.Play ();
		} else if (r <= .66) {
			SoundManager.instance.enemyDeath2.Play ();
		} else {
			SoundManager.instance.enemyDeath3.Play ();
		}
		Instantiate (bloodPrefab, gameObject.transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	public void Damage (int damage) {
		health -= damage;
		if (hurtRoutine != null) {
			StopCoroutine (hurtRoutine);
		}
		hurtRoutine = StartCoroutine (HurtCoroutine ());
	}

	/// <summary>
	/// Enemy flashes red when damaged
	/// </summary>
	IEnumerator HurtCoroutine () {
		float timer = 0;
		bool blink = false;
		while (timer < hurtTimer) {
			blink = !blink;
			timer += Time.deltaTime;
			if (blink) {
				foreach (SpriteRenderer rend in sr) {
					rend.color = Color.white;
				}
			} else {
				foreach (SpriteRenderer rend in sr) {
					rend.color = Color.red;
				}
			}
			yield return new WaitForSeconds (0.05f);
		}
		foreach (SpriteRenderer rend in sr) {
			rend.color = Color.white;
		}
	}

}
