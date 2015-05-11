using UnityEngine;
using System.Collections;

public class FireRegularly : FiringPattern {
	protected const int MIN_INTERVAL = 5, MAX_INTERVAL = 60;

	public int Timer;

	override public void Initialize() {
		SetTimer();
	}

	virtual protected void SetTimer() {
		Timer = (int)Mathf.Lerp (MIN_INTERVAL, MAX_INTERVAL, Arguments [0]);
	}

	override public bool UpdateFire () {
		--Timer;
		if (Timer <= 0) {
			SetTimer();
			return true;
		} else
			return false;
	}
}
