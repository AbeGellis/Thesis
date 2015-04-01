using UnityEngine;
using System.Collections;

public class EnemyState {
	public virtual void PlanMovement(EnemyPlayer toControl) {
		toControl.HandleInput (Controls.Jump);
	}
}
