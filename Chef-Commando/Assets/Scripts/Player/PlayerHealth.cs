/// <summary>
/// Class taht holds all the player health and xp logic
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int xp = 0;
    public int skillPoints = 0;
    public int xpForNextLevel = 10;
	public int DamageReceived = 1;
    public int playerHP = 5;
	public int playerMoney = 0;
	//public int moneyEarned = 3;

	private bool isColliding = false;

    AudioSource sound;
	public AudioClip gruntSound; 


	public Text moneyCount;
    private GameObject mainCamera;
	private UIHealthPanel healthpanel;
    private UIXPSlider xpSlider;

	private bool invincibility = false;

    
	void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		healthpanel = GameObject.FindObjectOfType<UIHealthPanel> ();
        xpSlider = GameObject.FindObjectOfType<UIXPSlider>();
		healthpanel.SetHealth (playerHP);
		//moneyCount.text = "$" + playerMoney;
		sound = GetComponent<AudioSource> ();
	}

	void OnCollisionEnter(Collision hit) {
        // Take damage if player collides with a customer while not invincible.
		if (hit.gameObject.CompareTag("Customer") && !invincibility) {
			isColliding = true;
			takeDamage(DamageReceived);
		} 
	}

    /*void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Customer") && !invincibility) {
            isColliding = true;
            takeDamage(DamageReceived);
        }
    }*/

    /*
	void OnCollisionExit(Collision hit)
	{
		if (hit.gameObject.CompareTag ("Customer")) {
			Debug.Log ("No longer colliding");
			isColliding = false;
		}
	}


	void OnTriggerEnter(Collider hit){
		if (hit.gameObject.CompareTag("Money")) {
			Debug.Log ("Coin picked up");
			hit.gameObject.SetActive (false);
		//	EarnMoney (moneyEarned);
		}
	}
*/
    // Reduces player HP after touching an enemy. Kills the player when HP is 0.
    void takeDamage(int hp) {
        mainCamera.GetComponent<CameraShake>().ShakeCamera(0.1f, 0.1f);
        playerHP =	playerHP - hp;	
		healthpanel.SetHealth (playerHP);
		invincibility = true;
		StartCoroutine (Invincible (2));
		sound.PlayOneShot (gruntSound, 1);
		if (playerHP <= 0) 
			Dead ();
	}

	public void Dead() {
		EventManager.TriggerEvent (Events.playerDead);
	}

    // Awards the player the specified amount of XP and a skill point if the player "levels up". Updates XP slider.
    public void GiveXP(int exp) {
        xp += exp;
         // Checks if enough XP has been gained for the next skill point.
        if (xp >= xpForNextLevel) {
            skillPoints++;
            xp -= xpForNextLevel;
            xpForNextLevel += 5;
        }

        xpSlider.UpdateSlider(xpForNextLevel, xp);
    }
	/*
    // Awards the player the specified amount of coins. Updates money display.
	public void EarnMoney(int amount){
		playerMoney += amount;
		Debug.Log ("Money: " + playerMoney);
		moneyCount.text = "$" + playerMoney;
	}
	*/

	// Time of player invincibility after being hit
	IEnumerator Invincible(float time) {
		yield return new WaitForSeconds(time);
		invincibility = false;
	}

}
