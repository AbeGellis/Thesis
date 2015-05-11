using UnityEngine;
using System.Collections;

public class FireRandomly : FireRegularly {

	override protected void SetTimer() {
		Timer = (int)Mathf.Lerp (MIN_INTERVAL, MAX_INTERVAL, Random.Range(0f,1f));
	}
}
