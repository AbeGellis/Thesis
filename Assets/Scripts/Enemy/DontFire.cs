using UnityEngine;
using System.Collections;

public class DontFire : FiringPattern {
	override public bool UpdateFire () {
		return false;
	}
}
