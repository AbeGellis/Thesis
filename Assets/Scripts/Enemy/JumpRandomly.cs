using UnityEngine;
using System.Collections;

public class JumpRandomly : JumpAcrossField {
	override public void Landed() {
		MoveRight = (Random.Range (0, 2) == 1);
	}
}
