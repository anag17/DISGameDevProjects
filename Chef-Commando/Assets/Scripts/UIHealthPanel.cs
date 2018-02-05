/// <summary>
/// User interface health panel.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class UIHealthPanel : MonoBehaviour
{
	[SerializeField] Text healthText;
	public  Image HeartImg;
	public Text HPcount;

	// Displays the player's health
	public void SetHealth (int playerHP)
	{
		HPcount.text = "x" + playerHP;
	}
		
}
