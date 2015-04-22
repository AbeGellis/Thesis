using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown (KeyCode.LeftControl))
			Application.LoadLevel (Application.loadedLevel);
	}

	void FixedUpdate() {
		StepBasedComponent.GameStep ();
	}
}
