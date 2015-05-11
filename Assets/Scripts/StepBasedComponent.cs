using UnityEngine;
using System.Collections;

abstract public class StepBasedComponent : MonoBehaviour, System.ICloneable {
	public static int Frame;


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

	public static void GameStep() {
		Random.seed = Frame;
		OnBeginStep ();
		OnStep ();
		OnEndStep ();
		++Frame;
	}

	
	public object Clone() {
		return this.MemberwiseClone ();
	}

	virtual public void BeginStep() {}
	virtual public void Step() {}
	virtual public void EndStep() {}
}
