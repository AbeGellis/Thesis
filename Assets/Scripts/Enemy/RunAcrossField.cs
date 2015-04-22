using UnityEngine;
using System.Collections;

public class RunAcrossField : EnemyState {
	protected bool _moveRight;

	override public void EnterState() {
		base.EnterState ();

		_moveRight = ToControl.foe.transform.position.x > ToControl.transform.position.x;
		
		//Move forward or away from player based on arg0
		if (Arguments[0] > .5f)
			_moveRight = !_moveRight;
	}

	override public void PlanMovement() {
		base.PlanMovement ();

		if (_moveRight)
			ToControl.HandleInput (Controls.Right);
		else
			ToControl.HandleInput (Controls.Left);
	}
		
	override public void WallHit(Direction contactDir) {
		base.WallHit (contactDir);
		
		if (contactDir == Direction.Right && _moveRight) 
			_moveRight = false;
		if (contactDir == Direction.Left && !_moveRight) 
			_moveRight = true;
		
	}
}
