using UnityEngine;
using System.Collections;

public class FireRegularly : FiringPattern {
	protected const int MIN_INTERVAL = 5, MAX_INTERVAL = 60;

	protected int _timer;

	override public void Initialize() {
		SetTimer();
	}

	virtual protected void SetTimer() {
		_timer = (int)Mathf.Lerp (MIN_INTERVAL, MAX_INTERVAL, Arguments [0]);
	}

	override public bool UpdateFire () {
		--_timer;
		if (_timer <= 0) {
			SetTimer();
			return true;
		} else
			return false;
	}
}
