using UnityEngine;
using System.Collections;

public class RunAcrossField : EnemyState {
	public bool MoveRight;

	override public void EnterState() {
		base.EnterState ();

		MoveRight = ToControl.foe.transform.position.x > ToControl.transform.position.x;
		
		//Move forward or away from player based on arg0
		if (Arguments[0] > .5f)
			MoveRight = !MoveRight;
	}

	override public void PlanMovement() {
		base.PlanMovement ();

		if (MoveRight)
			ToControl.HandleInput (Controls.Right);
		else
			ToControl.HandleInput (Controls.Left);
	}
		
	override public void WallHit(Direction contactDir) {
		base.WallHit (contactDir);
		
		if (contactDir == Direction.Right && MoveRight) 
			MoveRight = false;
		if (contactDir == Direction.Left && !MoveRight) 
			MoveRight = true;
		
	}
}
