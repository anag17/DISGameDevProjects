using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton that provides access to all HUD UI for other scripts
/// </summary>
public class HudManager : MonoBehaviour {

	public Text coinsText, scoreText, scoreIncrementText, storeAlertText;
	public Image cooldownBarBack, cooldownBarFront, hourglass;
	public static HudManager instance;

	void Awake () {
		instance = this;
	}
}
