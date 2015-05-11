using UnityEngine;
using System.Collections;
using System;

public class FiringPattern {
	public float[] Arguments = new float[2];

	public virtual FiringPattern CreateCopy() {
		FiringPattern copy = (FiringPattern) Activator.CreateInstance(this.GetType());
		copy.Arguments = Arguments;

		if (typeof(FireRegularly).IsAssignableFrom (GetType ())) 
			((FireRegularly)copy).Timer = ((FireRegularly)this).Timer;
		
		if (typeof(FireInBursts).IsAssignableFrom (GetType ())) 
			((FireInBursts)copy).BurstTimer = ((FireInBursts)this).BurstTimer;

		return copy;
	}

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
