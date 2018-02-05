using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player class: No class
/// </summary>
public class NoClass : PlayerClass {

	public override string title {get; protected set;}

	void Start () {
		title = "None";
	}
}
