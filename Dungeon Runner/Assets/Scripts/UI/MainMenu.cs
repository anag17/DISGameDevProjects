// <copyright file="MainMenu.cs" company="DIS Copenhagen">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Benno Lueders</author>
// <date>14/08/2017</date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main menu script. Provides functionality to start the game controlled by buttons from the Unity UI.
/// </summary>
public class MainMenu : MonoBehaviour {

	void Start() {
		GameManager.gameState = GameManager.GameState.MainMenu;
	}

	public void Instructions() {
		StartCoroutine (InstructionsCoroutine ());
	}

	IEnumerator InstructionsCoroutine() {
		SoundManager.instance.uiInteraction.Play ();
		// Pauses time long enough for sound to play
		Time.timeScale = .01f;
		yield return new WaitForSeconds (.001f);
		Time.timeScale = 1;
		GameManager.instance.LoadScene ("Instructions");
	}

	public void StartGame() {
		StartCoroutine (StartGameCoroutine ());
	}

	IEnumerator StartGameCoroutine() {
		SoundManager.instance.swordClash.Play ();
		yield return new WaitForSeconds (.9f);
		GameManager.gameState = GameManager.GameState.Running;
		GameManager.instance.LoadScene ("Game");
	}
}
