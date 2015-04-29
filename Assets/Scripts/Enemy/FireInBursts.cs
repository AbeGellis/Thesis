using UnityEngine;
using System.Collections;

public class FireInBursts : FireRegularly {
	protected const int MIN_BURST_DURATION = 5, MAX_BURST_DURATION = 30;
	protected int _burstTimer;

	protected void SetBurstTimer() {
		_burstTimer = (int)Mathf.Lerp (MIN_INTERVAL, MAX_INTERVAL, Arguments [1]);
	}

	override public bool UpdateFire () {
		--_timer;
		if (_timer <= 0) {
			SetTimer ();
			SetBurstTimer ();
			return true;
		} else {
			if (_burstTimer > 0) {
				--_burstTimer;
				return true;
			}
			else
				return false;
			
		}
	}
}
