using UnityEngine;
using System.Collections;

//TODO randomize jumping height

public class JumpAcrossField : RunAcrossField {

	override public void PlanMovement() {
		base.PlanMovement ();
		
		ToControl.HandleInput (Controls.Jump);
	}
}
