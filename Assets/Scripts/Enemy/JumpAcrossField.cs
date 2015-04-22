using UnityEngine;
using System.Collections;

//TODO randomize jumping height

public class JumpAcrossField : RunAcrossField {

	override public void Landed() {
		_moveRight = (Random.Range (0, 2) == 1);
	}


}
