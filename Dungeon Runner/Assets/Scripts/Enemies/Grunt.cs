using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy type: Goblin grunt
/// </summary>
public class Grunt : Enemy {

	protected override void Die () {
		base.Die ();
		ScoreManager.instance.gruntsDefeated++;
	}
}
