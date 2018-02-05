/// <summary>
/// Game controller- Goes between teh games different menus
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private Canvas pauseMenu;
    [SerializeField] private Canvas upgradeMenu;
	[SerializeField] private Canvas lossMenu;
    [SerializeField] private Canvas cookBookMenu;
    [SerializeField] private GameObject player;

    private static GameController GC;
	private bool isPaused = false;
	private bool isDead = false;

    private int jumpLevels = 0;
    private int speedLevels = 0;
    private int healthLevels = 0;
    private int throwLevels = 0;

	AudioSource gameSound;

    void OnEnable() {
		EventManager.StartListening (Events.playerDead, OnPlayerDeath);
	}

    void Awake () {
		gameSound = GetComponent<AudioSource> ();

        if (GC == null) {
			GC = this;
		} else {
			Destroy(gameObject);
		}
	}

    void Update () {
        // If the escape key is pressed, toggle the pause state.
		if ((Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Tab)) && (!isDead)) {
            upgradeMenu.gameObject.SetActive(false);
            cookBookMenu.gameObject.SetActive(false);
			TogglePause ();
        }
		Cursor.visible = isPaused;
    }

    // Loads the main menu scene
    public void MainMenu() {
		SceneManager.LoadSceneAsync(0);
	}

    // Displays the upgrade menu
    public void UpgradeMenu () {
        pauseMenu.gameObject.SetActive(false);
        upgradeMenu.gameObject.SetActive(true);
        UpdateUpgradeMenu();
    }

    public void CookBookMenu() {
        cookBookMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
    }

    // Restarts the game scene
	public void Restart() {
		isDead = false;
		SceneManager.LoadSceneAsync (1);
		TogglePause ();
	}

    // Increases the player's movement speed
    public void UpgradeSpeed () {
        if (player.GetComponent<PlayerHealth>().skillPoints > 0) {
            player.GetComponent<PlayerHealth>().skillPoints--;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().movementSettings.ForwardSpeed *= 1.5f;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().movementSettings.BackwardSpeed *= 1.5f;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().movementSettings.StrafeSpeed *= 1.5f;
            speedLevels++;
        }

        UpdateUpgradeMenu();
    }

    // Increases the player's jump height
    public void UpgradeJump () {
        if (player.GetComponent<PlayerHealth>().skillPoints > 0) {
            player.GetComponent<PlayerHealth>().skillPoints--;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().movementSettings.JumpForce *= 1.5f;
            jumpLevels++;
        }

        UpdateUpgradeMenu();
    }

    // Increases the player's throwing power
    public void UpgradeThrow () {
        if (player.GetComponent<PlayerHealth>().skillPoints > 0) {
            player.GetComponent<PlayerHealth>().skillPoints--;
            player.GetComponent<GrabThrow>().throwSpeed *= 1.5f;
            throwLevels++;
        }

        UpdateUpgradeMenu();
    }

    // Increases the player's health
	public void UpgradeHealth() {
        if (player.GetComponent<PlayerHealth>().skillPoints > 0) {
            player.GetComponent<PlayerHealth>().skillPoints--;
            player.GetComponent<PlayerHealth>().playerHP += 1;
            healthLevels++;
        }

        GameObject.FindGameObjectWithTag("HealthPanel").GetComponent<UIHealthPanel>().SetHealth(player.GetComponent<PlayerHealth>().playerHP);

        UpdateUpgradeMenu();
    }

    // Refreshes the upgrade menu to reflect the player's current amount of skill points and enables/disables buttons accordingly
    public void UpdateUpgradeMenu() {
        GameObject.FindGameObjectWithTag("PointsToSpend").GetComponent<Text>().text = "Points to spend: " + player.GetComponent<PlayerHealth>().skillPoints;
        GameObject.FindGameObjectWithTag("XPPanel").GetComponent<UIXPSlider>().UpdateSlider(player.GetComponent<PlayerHealth>().xpForNextLevel, player.GetComponent<PlayerHealth>().xp);
        GameObject.FindGameObjectWithTag("JumpLevels").GetComponent<Text>().text = "Levels: " + jumpLevels;
        GameObject.FindGameObjectWithTag("SpeedLevels").GetComponent<Text>().text = "Levels: " + speedLevels;
        GameObject.FindGameObjectWithTag("ThrowLevels").GetComponent<Text>().text = "Levels: " + throwLevels;
        GameObject.FindGameObjectWithTag("HealthLevels").GetComponent<Text>().text = "Levels: " + healthLevels;

        if (player.GetComponent<PlayerHealth>().skillPoints > 0){
            Button[] buttons = GameObject.FindObjectsOfType<Button>();

            // Find and enable all upgrade buttons if skill points are available.
            foreach (Button button in buttons) {
                if (button.CompareTag("UpgradeButton")) {
                    button.interactable = true;
                }
            }
        } else {
            Button[] buttons = GameObject.FindObjectsOfType<Button>();

            // Find and disable all upgrade buttons if skill points are unavailable.
            foreach (Button button in buttons) {
                if (button.CompareTag("UpgradeButton")) {
                    button.interactable = false;
                }
            }
        }

    }

	public static bool GetPaused() {
		return GC.isPaused;
	}

	// pauses and unpauses game
	public void SetPause(bool b) {
		isPaused = b;
		if (isDead) {
			lossMenu.gameObject.SetActive(true);
		} else{
			pauseMenu.gameObject.SetActive(b);
		    upgradeMenu.gameObject.SetActive(false);
	    }

		if (b) {
			Time.timeScale = 0;
            UnityBugWorkaround();
		} else {
			Time.timeScale = 1;
            UnityBugWorkaround();
		}
	}

    public static GameObject GetPlayer() {
        return GC.player;
    }
    
    // If the game is paused, unpause it. If the game is unpaused, pause it.
    private void TogglePause() {
		SetPause(!isPaused);
    }

    private void UnityBugWorkaround() {
        if (isPaused) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

//	Display loss Screen when PlayerPrefs dies
	void OnPlayerDeath() {
		isDead = true;
		SetPause(true);
		gameSound.Play();
	}
}
