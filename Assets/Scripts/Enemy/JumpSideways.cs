using UnityEngine;
using System.Collections;

//TODO randomize jumping height

public class JumpSideways : EnemyState {

	private bool _moveRight;

	override public void PlanMovement() {
		base.PlanMovement ();

		ToControl.HandleInput (Controls.Jump);
		if (_moveRight)
			ToControl.HandleInput (Controls.Right);
		else
			ToControl.HandleInput (Controls.Left);
	}

	override public void Initialize() {
		base.Initialize ();

		_moveRight = Arguments [0] > .5f;
	}

	override public void WallHit(Direction contactDir) {
		base.WallHit (contactDir);

		if (contactDir == Direction.Right && _moveRight) 
			_moveRight = false;
		if (contactDir == Direction.Left && !_moveRight) 
			_moveRight = true;

	}
}
