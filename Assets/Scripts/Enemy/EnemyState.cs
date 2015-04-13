using UnityEngine;
using System.Collections;

public enum EnemyStateTransitions { TimeElapsed, WallHit, NearPlayer, Landing, LandingNearPlayer, Damaged, HealthBelow}

public class EnemyState {
	private const int MIN_TIMER = 10, MAX_TIMER = 200;
	private int calculateTimer(float arg) {
		return Mathf.FloorToInt (Mathf.Lerp (MIN_TIMER, MAX_TIMER, arg));
	}

	private int _timer = 0;

	public static EnemyStateTransitions[] TransitionTypes = {
		EnemyStateTransitions.TimeElapsed,
		EnemyStateTransitions.Landing,
		EnemyStateTransitions.WallHit
	};

	public EnemyStateTransitions[] TransitionConditions = new EnemyStateTransitions[2];
	public float[] TransitionArguments = new float[2];
	public EnemyState[] OtherState = new EnemyState[2];
	public float[] Arguments = new float[2];
	public float StateHue;

	public EnemyPlayer ToControl;

	public virtual void Initialize() {
		_timer = 0;
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
		}
	}

	public virtual void Landed() {
		for (int i = 0; i < 2; ++i) {
			if (TransitionConditions[i] == EnemyStateTransitions.Landing) {
				ToControl.SetActiveState(OtherState[i]);
				break;
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
