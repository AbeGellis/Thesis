using UnityEngine;
using System.Collections;

abstract public class StepBasedComponent : MonoBehaviour {

	delegate void StepEvent();
	static event StepEvent OnBeginStep;
	static event StepEvent OnStep;
	static event StepEvent OnEndStep;

	virtual public void OnEnable() {
		OnBeginStep += BeginStep;
		OnStep += Step;
		OnEndStep += EndStep;
	}

	virtual public void OnDisable() {
		OnBeginStep -= BeginStep;
		OnStep -= Step;
		OnEndStep -= EndStep;
	}

	public void FixedUpdate() {	//TODO move into better system
		BeginStep ();
		Step ();
		EndStep ();
	}

	virtual public void BeginStep() {}
	virtual public void Step() {}
	virtual public void EndStep() {}
}
