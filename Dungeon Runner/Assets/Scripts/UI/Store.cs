// <copyright file="PauseMenu.cs" company="DIS Copenhagen">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Benno Lueders</author>
// <date>14/08/2017</date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Pause menu script. Provides functionality to Pause and Unpause the game and Go Back to the Main Menu. Controlled by buttons from the Unity UI.
/// Pressing ESC or P will open and close the menu.
/// Menu contains store in which player purchases permanent class upgrades.
/// </summary>
public class Store : MonoBehaviour {

	public GameObject storeCanvas, storePanel;

	/// <summary>
	/// Duration of store animation
	/// </summary>
	[SerializeField] float inOutTime;

	void Start() {
		GameManager.gameState = GameManager.GameState.Running;
		storeCanvas.SetActive (false);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
			if (GameManager.gameState == GameManager.GameState.Running) {
				StartCoroutine (PauseCoroutine ());
			}
			else if (GameManager.gameState == GameManager.GameState.Paused && !DeathReport.instance.displayed) {
				StartCoroutine (UnpauseCoroutine ());
			}
		}
	}

	public void UpgradeMage() {
		if (!PlayerController.instance.GetComponent<Mage> ().upgraded && ScoreManager.instance.PurchaseSuccess ()) {
			PlayerController.instance.GetComponent<Mage> ().upgraded = true;
			SoundManager.instance.swordClash.Play ();
			Button[] buttons = GameObject.FindObjectsOfType<Button> ();
			foreach (Button button in buttons) {
				if (button.tag == "MageButton") {
					button.interactable = false;
					break;
				}
			}
		}
	}

	public void UpgradeRanger() {
		if (!PlayerController.instance.GetComponent<Ranger> ().upgraded && ScoreManager.instance.PurchaseSuccess ()) {
			PlayerController.instance.GetComponent<Ranger> ().upgraded = true;
			SoundManager.instance.swordClash.Play ();
			Button[] buttons = GameObject.FindObjectsOfType<Button> ();
			foreach (Button button in buttons) {
				if (button.tag == "RangerButton") {
					button.interactable = false;
					break;
				}
			}
		}
	}

	public void UpgradeThief() {
		if (!PlayerController.instance.GetComponent<Thief> ().upgraded && ScoreManager.instance.PurchaseSuccess ()) {
			PlayerController.instance.GetComponent<Thief> ().upgraded = true;
			SoundManager.instance.swordClash.Play ();
			Button[] buttons = GameObject.FindObjectsOfType<Button> ();
			foreach (Button button in buttons) {
				if (button.tag == "ThiefButton") {
					button.interactable = false;
					break;
				}
			}
			if (PlayerController.instance.currentClass.title == "Thief") {
				PlayerController.instance.coinMultiplier = 4;
			}
		}
	}

	public void UpgradeWarrior() {
		if (!PlayerController.instance.GetComponent<Warrior> ().upgraded && ScoreManager.instance.PurchaseSuccess ()) {
			PlayerController.instance.GetComponent<Warrior> ().upgraded = true;
			SoundManager.instance.swordClash.Play ();
			Button[] buttons = GameObject.FindObjectsOfType<Button> ();
			foreach (Button button in buttons) {
				if (button.tag == "WarriorButton") {
					button.interactable = false;
					break;
				}
			}
			if (PlayerController.instance.currentClass.title == "Warrior") {
				AnimatorController.instance.UpdateClass (5);
			}
		}
	}

	public void Unpause() {
		StartCoroutine (UnpauseCoroutine ());
	}

	/// <summary>
	/// Store menu animation
	/// </summary>
	IEnumerator PauseCoroutine() {
		storeCanvas.SetActive (true);
		GameManager.instance.Pause ();
		float timer = 0;
		while (timer < inOutTime) {
			timer += Time.unscaledDeltaTime;
			float normalizedTime = timer / inOutTime;
			storePanel.transform.localScale = new Vector3 (Easing.Circular.In (normalizedTime), 1, 1);
			yield return null;
		}
		storePanel.transform.localScale = new Vector3 (1, 1, 1);
	}

	/// <summary>
	/// Reverse store menu animation
	/// </summary>
	IEnumerator UnpauseCoroutine() {
		GameManager.instance.Unpause ();
		float timer = 0;
		while (timer < inOutTime) {
			timer += Time.unscaledDeltaTime;
			float normalizedTime = timer / inOutTime;
			storePanel.transform.localScale = new Vector3 (Easing.Circular.In (1 - normalizedTime), 1, 1);
			yield return null;
		}
		storePanel.transform.localScale = new Vector3 (0, 1, 1);
		storeCanvas.SetActive (false);
	}
}
