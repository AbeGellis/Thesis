using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	void FixedUpdate() {
		StepBasedComponent.GameStep ();
	}
}
