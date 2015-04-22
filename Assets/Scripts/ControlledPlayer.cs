using UnityEngine;
using System.Collections;

public class ControlledPlayer : Player {
	public KeyCode Jump, Left, Right, Shoot;
	public float shotSpeed;
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
			HandleShoot (facingRight ? Vector2.right * shotSpeed: Vector2.right * shotSpeed);
		}
	}
}
