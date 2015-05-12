using UnityEngine;
using System.Collections;
using System;

public enum EnemyStateTransitions { 
	TimeElapsed, 
	WallHit, 
	NearPlayer, 
	FarFromPlayer, 
	Landing, 
	LandingNearPlayer, 
	LandingFarFromPlayer,
	NearPoint,
	LandingNearPoint,
	Damaged, 
	HealthBelow
}

public class EnemyState {
	private const int MIN_TIMER = 10, MAX_TIMER = 200;
	private const float MIN_DISTANCE = .5f, MAX_DISTANCE = 8f;
	private const float MIN_X = .75f, MAX_X = 11.25f;
	private const float NEAR_POINT_DISTANCE = 2f;
	private int calculateTimer(float arg) {
		return Mathf.FloorToInt (Mathf.Lerp (MIN_TIMER, MAX_TIMER, arg));
	}

	private int _timer = 0;

	public static Type[] StateTypes = {
		typeof(JumpAcrossField),
		typeof(JumpInPlace),
		typeof(RunAcrossField),
		typeof(JumpRandomly)
	};

	/*public EnemyState() {}

	public EnemyState(EnemyState original) {
		Array.Copy (original.Arguments, Arguments, Arguments.Length);
		Array.Copy (original.TransitionArguments, TransitionArguments, TransitionArguments.Length);
		_timer = original._timer;
		TransitionConditions = original.TransitionConditions;
		StateHue = original.StateHue;
		FirePattern = (FiringPattern) System.Activator.CreateInstance(original.FirePattern.GetType(), original.FirePattern);
		ToControl = original.ToControl;
	}*/

	public virtual EnemyState CreateCopy() {
		EnemyState copy = (EnemyState) Activator.CreateInstance(this.GetType());
		Array.Copy (Arguments, copy.Arguments, Arguments.Length);
		Array.Copy (TransitionArguments, copy.TransitionArguments, TransitionArguments.Length);
		copy._timer = _timer;
		copy.TransitionConditions = TransitionConditions;
		copy.StateHue = StateHue;
		copy.FirePattern = FirePattern.CreateCopy ();
		copy.ToControl = ToControl;

		if (typeof(RunAcrossField).IsAssignableFrom(this.GetType ())) 
			((RunAcrossField)copy).MoveRight = ((RunAcrossField)this).MoveRight;

		return copy;
	}

	/*virtual public object Clone() {
		EnemyState copy = new EnemyState ();
		copy.Arguments = new float[Arguments.Length];
		Array.Copy (Arguments, copy.Arguments, Arguments.Length);
		copy._timer = _timer;
		copy.TransitionConditions = TransitionConditions;
		copy.TransitionArguments = new float[TransitionArguments.Length];
		Array.Copy (TransitionArguments, copy.TransitionArguments, TransitionArguments.Length);
		copy.StateHue = StateHue;
		copy.FirePattern = (FiringPattern) FirePattern.Clone();
		return copy;
	}*/

	public static EnemyStateTransitions[] TransitionTypes = {
		EnemyStateTransitions.TimeElapsed,
		EnemyStateTransitions.Landing,
		EnemyStateTransitions.WallHit,
		EnemyStateTransitions.NearPlayer,
		EnemyStateTransitions.FarFromPlayer,
		EnemyStateTransitions.NearPoint,
		EnemyStateTransitions.LandingNearPlayer,
		EnemyStateTransitions.LandingFarFromPlayer,
		EnemyStateTransitions.LandingNearPoint
	};
	public EnemyStateTransitions[] TransitionConditions = new EnemyStateTransitions[2];
	public float[] TransitionArguments = new float[2];
	public EnemyState[] OtherState = new EnemyState[2];
	public float[] Arguments = new float[2];
	public float StateHue;
	public FiringPattern FirePattern;

	public EnemyPlayer ToControl;
	public virtual void EnterState() {
		_timer = 0;
		FirePattern.Initialize ();
	}

	public virtual void ExitState() {
	}

	public virtual void PlanMovement() {
		++_timer;
		for (int i = 0; i < 2; ++i) {
			if (TransitionConditions[i] == EnemyStateTransitions.TimeElapsed) {
				if (_timer > calculateTimer(TransitionArguments[i])) {
					ToControl.SetActiveState(OtherState[i]);
					break;
				}
			}

			if (TransitionConditions[i] == EnemyStateTransitions.NearPlayer) {
				if (Mathf.Abs(ToControl.foe.transform.position.x - ToControl.transform.position.x) < 
				    Mathf.Lerp (MIN_DISTANCE, MAX_DISTANCE, TransitionArguments[i])) {
					ToControl.SetActiveState(OtherState[i]);
					break;
				}
			}

			if (TransitionConditions[i] == EnemyStateTransitions.FarFromPlayer) {
				if (Mathf.Abs(ToControl.foe.transform.position.x - ToControl.transform.position.x) > 
				    Mathf.Lerp (MIN_DISTANCE, MAX_DISTANCE, TransitionArguments[i])) {
					ToControl.SetActiveState(OtherState[i]);
					break;
				}
			}

			if (TransitionConditions[i] == EnemyStateTransitions.NearPoint) {
				if (Mathf.Abs(ToControl.foe.transform.position.x - Mathf.Lerp(MIN_X, MAX_X, TransitionArguments[i])) < NEAR_POINT_DISTANCE) {
					ToControl.SetActiveState(OtherState[i]);
					break;
				}
			}
		}

		if (FirePattern.UpdateFire())
			ToControl.HandleInput (Controls.Shoot);
	}

	public virtual void Landed() {
		for (int i = 0; i < 2; ++i) {
			if (TransitionConditions[i] == EnemyStateTransitions.Landing) {
				ToControl.SetActiveState(OtherState[i]);
				break;
			}

			if (TransitionConditions[i] == EnemyStateTransitions.LandingNearPlayer) {
				if (Mathf.Abs(ToControl.foe.transform.position.x - ToControl.transform.position.x) < 
				    Mathf.Lerp (MIN_DISTANCE, MAX_DISTANCE, TransitionArguments[i])) {
					ToControl.SetActiveState(OtherState[i]);
					break;
				}
			}

			if (TransitionConditions[i] == EnemyStateTransitions.LandingFarFromPlayer) {
				if (Mathf.Abs(ToControl.foe.transform.position.x - ToControl.transform.position.x) > 
				    Mathf.Lerp (MIN_DISTANCE, MAX_DISTANCE, TransitionArguments[i])) {
					ToControl.SetActiveState(OtherState[i]);
					break;
				}
			}

			if (TransitionConditions[i] == EnemyStateTransitions.LandingNearPoint) {
				if (Mathf.Abs(ToControl.foe.transform.position.x - Mathf.Lerp(MIN_X, MAX_X, TransitionArguments[i])) < NEAR_POINT_DISTANCE) {
					ToControl.SetActiveState(OtherState[i]);
					break;
				}
			}
		}
	}

	public virtual void WallHit(Direction contactDir) {
		if (contactDir == Direction.Left || contactDir == Direction.Right) {
			for (int i = 0; i < 2; ++i) {
				if (TransitionConditions [i] == EnemyStateTransitions.WallHit) {
					ToControl.SetActiveState (OtherState [i]);
					break;
				}
			}
		}
	}
}
