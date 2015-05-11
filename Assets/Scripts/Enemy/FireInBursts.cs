using UnityEngine;
using System.Collections;

public class FireInBursts : FireRegularly {
	protected const int MIN_BURST_DURATION = 5, MAX_BURST_DURATION = 30;
	public int BurstTimer;

	protected void SetBurstTimer() {
		BurstTimer = (int)Mathf.Lerp (MIN_INTERVAL, MAX_INTERVAL, Arguments [1]);
	}

	override public bool UpdateFire () {
		--Timer;
		if (Timer <= 0) {
			SetTimer ();
			SetBurstTimer ();
			return true;
		} else {
			if (BurstTimer > 0) {
				--BurstTimer;
				return true;
			}
			else
				return false;
			
		}
	}
}
