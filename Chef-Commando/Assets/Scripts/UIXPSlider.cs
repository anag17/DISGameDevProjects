/// <summary>
/// UIXP slider- UI for player xp
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIXPSlider : MonoBehaviour {

    [SerializeField] RectTransform slider;
    [SerializeField] Text xpText;
    [SerializeField] Text skillAlert;

    // Update slider to reflect current amount of XP.
    public void UpdateSlider(int xpForNextLevel, int xp) {
        float relativeScale = (float)xp / (float)xpForNextLevel;

        //Update text to reflect current amount of XP.
        xpText.text = xp.ToString() + "/" + xpForNextLevel.ToString();
        Vector3 scale = slider.transform.localScale;
        scale.x = relativeScale;
        slider.transform.localScale = scale;

        // Notify the player that skill points are available.
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().skillPoints > 0) {
            skillAlert.gameObject.transform.localScale = new Vector3(1, 1, 1);
        } else {
            skillAlert.gameObject.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
