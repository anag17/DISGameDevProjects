using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AntonsJob : MonoBehaviour {

	public float cooldown = 3;

	[SerializeField] private AudioClip ding;
    [SerializeField] private GrabThrow player;
	[SerializeField] private Material cookingMat;
    [SerializeField] private Transform ingredientSpawn;

	protected AudioSource cooksound;

    private List<Pickup> ingredients = new List<Pickup>();
    private List<GameObject> displayedIngredients = new List<GameObject>();
    private Material ogMat;
    private Renderer stoveRenderer;
    private int matIndex = 9;
	private bool canCook = true;

	void Awake() {
        stoveRenderer = GetComponentInChildren<Renderer>();
        ogMat = stoveRenderer.materials[matIndex];
        cooksound = gameObject.GetComponent<AudioSource> ();
	}

    void OnCollisionEnter (Collision other) { 
		// Only allows ingredients to go into stove
		if (canCook && other.gameObject.CompareTag("ingredient") && !other.gameObject.GetComponent<Pickup>().isMeal) {
            ingredients.Add(other.gameObject.GetComponent<Pickup>());
	        DisplayIngredient(other.gameObject.GetComponent<Pickup>().GrabPickup());

	        Pickup meal = CookBook.GetRecipe(ingredients);
	        if (meal != null) {
	            ingredients.RemoveRange(0, ingredients.Count);
				canCook = false;
				StartCoroutine(CookingCoroutine (meal));
	            ClearDisplay();
	        } 
        } 
		// Conditional statement for Mr. Clean removing ingredients in a stove
		else if (other.gameObject.GetComponent<Pickup>() && other.gameObject.GetComponent<Pickup>().prefabName == "Clean") {
            ingredients.RemoveRange(0, ingredients.Count);
            ClearDisplay();
            Destroy(other.gameObject);
        }
    }

	// Adds the specified meal to inventory
	private void CookMeal(Pickup meal) {
		canCook = true;
		cooksound.PlayOneShot (ding);
        player.AddAmmo(meal);
	}

	// Displays ingredients on top of stove
    private void DisplayIngredient(GameObject ingredient) {
        ingredient = Instantiate(ingredient, ingredientSpawn);
        ingredient.AddComponent<Spin>();

        Vector3 newPosition = new Vector3(ingredientSpawn.localPosition.x + 0.5f * displayedIngredients.Count, ingredientSpawn.localPosition.y);
        ingredient.transform.localPosition = newPosition;

        displayedIngredients.Add(ingredient);
    }

	// Clears the display above stove
    private void ClearDisplay() {
        foreach (GameObject ingredient in displayedIngredients) {
            Destroy(ingredient);
        }
        displayedIngredients = new List<GameObject>();
    }

	// Waits for cooking cooldown and turns inside of stove red while cooking
	protected IEnumerator CookingCoroutine(Pickup meal) {
        Material[] mats = stoveRenderer.materials;
        mats[matIndex] = cookingMat;
        stoveRenderer.materials = mats;

        cooksound.Play();

        yield return new WaitForSeconds(cooldown);

        mats[matIndex] = ogMat;
        stoveRenderer.materials = mats;

        cooksound.Stop();
        CookMeal(meal);
	}
}
