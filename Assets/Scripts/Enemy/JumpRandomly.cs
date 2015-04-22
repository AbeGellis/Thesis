using UnityEngine;
using System.Collections;

public class JumpRandomly : JumpAcrossField {

	override public void PlanMovement() {
		base.PlanMovement ();
		
		ToControl.HandleInput (Controls.Jump);
	}
}
