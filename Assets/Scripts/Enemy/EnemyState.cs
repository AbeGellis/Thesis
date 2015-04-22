using UnityEngine;
using System.Collections;

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

	public EnemyPlayer ToControl;

	public virtual void EnterState() {
		_timer = 0;
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
