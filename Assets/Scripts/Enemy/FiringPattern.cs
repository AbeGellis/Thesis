using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FiringPattern : ScriptableObject {
	public float[] Arguments = new float[2];

	public static Type[] PatternTypes = {
		typeof(FireRandomly),
		typeof(DontFire),
		typeof(FireRegularly),
		typeof(FireInBursts)
	};

	virtual public void Initialize() {
	}

	virtual public bool UpdateFire () {
		return false;
	}
}
