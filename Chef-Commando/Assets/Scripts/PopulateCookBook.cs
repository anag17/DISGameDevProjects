/// <summary>
/// Populate cook book in the Cook Book menu
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateCookBook : MonoBehaviour {
    [SerializeField] private Canvas cookBookMenu;
    [SerializeField] private Canvas mainMenu;

    private Text recipes;

	// Use this for initialization
	void Start () {
        recipes = GetComponent<Text>();
        Populate();
	}

    void Populate() {
        recipes.text = "";
        foreach (List<Pickup> meal in CookBook.cookBook.Keys) {
            foreach (Pickup ingredient in meal) {
                recipes.text += ingredient.name + " ";
            }

			recipes.text += "= " + CookBook.cookBook[meal].prefabName + " (" + CookBook.cookBook[meal].tag + ")" + "\n";
        }
    }

    public void Back() {
        mainMenu.gameObject.SetActive(true);
        cookBookMenu.gameObject.SetActive(false);
    }
}
