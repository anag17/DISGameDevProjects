using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reports game achievements upon death
/// </summary>
public class DeathReport : MonoBehaviour {

	public GameObject deathReportCanvas, deathReportPanel;
	public static DeathReport instance;
	public Text totalScore, timeSurvived, coinsCollected, gruntsDefeated, archersDefeated, tanksDefeated;
	public bool displayed;

	/// <summary>
	/// Duration of death report animation
	/// </summary>
	[SerializeField] float inTime;

	void Start () {
		deathReportCanvas.SetActive (false);
		displayed = false;
		instance = this;
	}
	
	public void LoadDeathReport() {

		deathReportCanvas.SetActive (true);
		GameManager.instance.EndRun ();
		totalScore.text = "Total Score: " + ScoreManager.instance.score.ToString();
		gruntsDefeated.text = "Grunts defeated: " + ScoreManager.instance.gruntsDefeated.ToString();
		archersDefeated.text = "Archers defeated: " + ScoreManager.instance.archersDefeated.ToString ();
		tanksDefeated.text = "Tanks defeated: " + ScoreManager.instance.tanksDefeated.ToString ();
		coinsCollected.text = "Coins collected: " + ScoreManager.instance.coinsCollected.ToString ();

		// Calculates time survived in minutes and seconds
		int secondsSurvived = (int)System.Math.Round (Time.timeSinceLevelLoad, 0);
		string minutesSurvived = (secondsSurvived / 60).ToString();
		string extraSeconds = (secondsSurvived % 60).ToString();
		if (extraSeconds.Length == 1) {
			extraSeconds = "0" + extraSeconds;
		}
		timeSurvived.text = "Time survived: " + minutesSurvived + ":" + extraSeconds;

		displayed = true;
		SoundManager.instance.playerDeath.Play ();
		StartCoroutine (DeathReportCoroutine ());
	}

	/// <summary>
	/// Death report animation
	/// </summary>
	IEnumerator DeathReportCoroutine() {
		float timer = 0;
		while (timer < inTime) {
			timer += Time.unscaledDeltaTime;
			float normalizedTime = timer / inTime;
			deathReportPanel.transform.localScale = new Vector3 (Easing.Circular.In (normalizedTime), 1, 1);
			yield return null;
		}
		deathReportPanel.transform.localScale = new Vector3 (1, 1, 1);	
	}
}
