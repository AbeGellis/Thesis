using UnityEngine;
using System.Collections;

public class JumpInPlace : EnemyState {

	override public void PlanMovement() {
		base.PlanMovement ();

		ToControl.HandleInput (Controls.Jump);
	}
}
