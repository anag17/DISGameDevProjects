using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookBook : MonoBehaviour {

    [SerializeField] private List<Recipe> recipes;

    public static Dictionary<List<Pickup>, Pickup> cookBook = new Dictionary<List<Pickup>, Pickup>();

	void Awake () {
        if (cookBook.Count == 0) {
            foreach (Recipe recipe in recipes) {
                cookBook[recipe.ingredients] = recipe.meal;
            }
        }
	}

    public static Pickup GetRecipe(List<Pickup> ingredients) {
        foreach (List<Pickup> recipe in cookBook.Keys) {
            if (recipe.Count == ingredients.Count) {
                recipe.Sort();
                ingredients.Sort();
                if (ingredients.SequenceEqual(recipe)) {
                    return cookBook[recipe];
                }
            }
        }
        return null;
    }
}

[System.Serializable]
class Recipe {
    public Pickup meal = null;
    public List<Pickup> ingredients = new List<Pickup>();
}
