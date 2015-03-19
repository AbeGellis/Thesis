using UnityEngine;
using System.Collections;

public class ControlledPlayer : Player {
	public KeyCode Jump, Left, Right, Shoot;

	public void Update() {
		if (Input.GetKey (Left)) {
			HandleInput (Controls.Left);
		}
		if (Input.GetKey (Right)) {
			HandleInput (Controls.Right);
		}
		if (Input.GetKey (Jump)) {
			HandleInput (Controls.Jump);
		}
	}
}
