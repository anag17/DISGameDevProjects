using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu script. Provides functionality to start and quit the game controlled by buttons from the Unity UI.
/// </summary>
public class MainMenu : MonoBehaviour {

    [SerializeField] private Canvas cookBookMenu;
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas tutorial;

	public void StartGame(){
        SceneManager.LoadSceneAsync(1);
		Time.timeScale = 1;
    }

    public void CookBookMenu() {
        cookBookMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
    }

    public void Back() {
        mainMenu.gameObject.SetActive(true);
        tutorial.gameObject.SetActive(false);
    }

    public void Tutorial(){
        tutorial.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
    }

	public void Quit(){
		Application.Quit();
	}
}