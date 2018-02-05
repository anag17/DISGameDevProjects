// <copyright file="GameManager.cs" company="DIS Copenhagen">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Benno Lueders</author>
// <date>14/08/2017</date>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// In control of the game flow. Use this to transition between levels, request the current game state and pause/unpause the game.
/// </summary>
public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public enum GameState
	{
		MainMenu,
		Running,
		Paused
	}

	public static GameState gameState;

	void Awake() {
		instance = this;
	}

	public void Pause() {
		SoundManager.instance.uiInteraction.Play ();
		Time.timeScale = 0;
		gameState = GameState.Paused;
	}

	public void Unpause() {
		SoundManager.instance.uiInteraction.Play ();
		Time.timeScale = 1;
		gameState = GameState.Running;
	}
		
	IEnumerator MainMenuCoroutine() {
		SoundManager.instance.uiInteraction.Play ();
		// Pauses for sufficient time to play the UI interaction sound effect
		Time.timeScale = .01f;
		yield return new WaitForSeconds (.001f);
		Time.timeScale = 1;
		gameState = GameState.Running;
		gameState = GameState.MainMenu;
		SceneManager.LoadScene ("MainMenu");
	}

	IEnumerator PlayAgainCoroutine() {
		SoundManager.instance.uiInteraction.Play ();
		// Pauses for sufficient time to play the UI interaction sound effect
		Time.timeScale = .01f;
		yield return new WaitForSeconds (.001f);
		Time.timeScale = 1;
		gameState = GameState.Running;
		SceneManager.LoadScene ("Game");
	}

	public void PlayAgain() {
		StartCoroutine(PlayAgainCoroutine());
	}

	public void EndRun() {
		Time.timeScale = 0;
		gameState = GameState.Paused;
	}

	public void LoadScene(string scene) {
		SceneManager.LoadScene (scene);
		Time.timeScale = 1;
		gameState = GameState.Running;
	}

	public void MainMenu() {
		StartCoroutine (MainMenuCoroutine());
	}
}
