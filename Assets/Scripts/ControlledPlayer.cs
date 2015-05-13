using UnityEngine;
using System.Collections;

public class ControlledPlayer : Player {
	public KeyCode Jump, Left, Right, Shoot;

	override public void BeginStep() {
		if (Input.GetKey (Left)) {
			HandleInput (Controls.Left);
		}
		if (Input.GetKey (Right)) {
			HandleInput (Controls.Right);
		}
		if (Input.GetKey (Jump)) {
			HandleInput (Controls.Jump);
		}
		if (Input.GetKey (Shoot)) {
			HandleInput (Controls.Shoot);
		}
	}

	override public void EndStep() {
		GameObject g = coll.CheckCollision (CollisionCategory.EnemyAttack);
		if (g) {
			Health -= g.GetComponent<Bullet>().Damage;
			g.SetActive(false);
		}
	}
}
