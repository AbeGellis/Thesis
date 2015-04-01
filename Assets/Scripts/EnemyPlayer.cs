using UnityEngine;
using System.Collections;

public class EnemyPlayer : Player {
	private EnemyState _currentState = new EnemyState();

	public override void BeginStep() {
		_currentState.PlanMovement (this);
	}

	new public void HandleInput(int input) {
		base.HandleInput (input);
	}
}
