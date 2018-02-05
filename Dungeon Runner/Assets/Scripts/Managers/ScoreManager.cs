using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Keeps track of player's scores and communicates them to HUD and death report
/// </summary>
public class ScoreManager : MonoBehaviour {

	public static ScoreManager instance;
	public int coinValue;
	public int upgradePrice;
	public int coins;
	public int coinsCollected;
	public int gruntsDefeated;
	public int archersDefeated;
	public int tanksDefeated;
	public int score;

	void Start () {
		instance = this;
		coinsCollected = 0;
		gruntsDefeated = 0;
		archersDefeated = 0;
		tanksDefeated = 0;
		score = 0;
		StartCoroutine (SurvivalCoroutine ());
		HudManager.instance.coinsText.text = coins.ToString ();
	}

	void Update() {
		HudManager.instance.scoreText.text = "Score: " + score.ToString ();
		HudManager.instance.coinsText.text = coins.ToString();
		// Displays store alert if player has enough coins to upgrade and not all classes are upgraded
		if (coins >= upgradePrice &&
			!(PlayerController.instance.GetComponent<Mage>().upgraded
			&& PlayerController.instance.GetComponent<Ranger>().upgraded
			&& PlayerController.instance.GetComponent<Thief>().upgraded
			&& PlayerController.instance.GetComponent<Warrior>().upgraded)) {
			HudManager.instance.storeAlertText.gameObject.SetActive (true);
		} else {
			HudManager.instance.storeAlertText.gameObject.SetActive (false);
		}
	}

	public void AddCoins (int numCoins, bool chest) {
		if (chest) {
			IncrementScore(3 * coinValue);
		} else {
			IncrementScore(coinValue);
		}
		StartCoroutine (AddCoinsCoroutine (numCoins));
	}

	IEnumerator AddCoinsCoroutine(int numCoins) {
		for (int i = 0; i < numCoins; i++) {
			coins++;
			coinsCollected++;
			SoundManager.instance.coin.Play ();
			yield return new WaitForSeconds (.15f);

		}
	}

	IEnumerator SurvivalCoroutine() {
		while (true) {
			yield return new WaitForSeconds (.1f);
			score++;
		}
	}

	public bool PurchaseSuccess() {
		if (coins >= upgradePrice) {
			coins -= upgradePrice;
			HudManager.instance.coinsText.text = coins.ToString();
			return true;
		}
		return false;
	}

	public void IncrementScore(int value) {
		StopCoroutine("ScoreIncrementCoroutine");
		StartCoroutine ("ScoreIncrementCoroutine", value);
	}

	public IEnumerator ScoreIncrementCoroutine(int value) {
		score += value;
		HudManager.instance.scoreIncrementText.text = "+ " + value.ToString ();
		yield return new WaitForSeconds (2);
		HudManager.instance.scoreIncrementText.text = "";
	}
}
