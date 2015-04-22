using UnityEngine;
using System.Collections;
using System;

public class EnemyPlayer : Player {
	public RandomNumberGenerator RNG;
	public Player foe;

	private static Type[] _stateOptions = {
		typeof(JumpAcrossField),
		typeof(JumpInPlace),
		typeof(RunAcrossField),
		typeof(JumpRandomly)
	};

	private EnemyState _currentState;
	private EnemyState[] _states = new EnemyState[3];

	private EnemyState GenerateState (float typeVal) {
		return (EnemyState) Activator.CreateInstance(_stateOptions[Mathf.FloorToInt(typeVal * _stateOptions.Length)]);
	}

	private SpriteRenderer _spriteRenderer;

	public void Start() {
		_spriteRenderer = GetComponent<SpriteRenderer> ();

		for (int i = 0; i < 3; ++i) {
			float val = RNG.GenerateValue("State " + i.ToString() + " type");
			_states[i] = GenerateState(val);

			_states[i].ToControl = this;

			_states[i].StateHue = RNG.GenerateValue("State " + i.ToString() + " hue");
			for (int j = 0; j < 2; ++j) 
				_states[i].Arguments[j] = RNG.GenerateValue("State " + i.ToString() + " Argument " + j.ToString());
			for (int j = 0; j < 2; ++j) {
				float transitionType = RNG.GenerateValue("State " + i.ToString() + " Transition Type " + j.ToString());
				_states[i].TransitionConditions[j] = 
					EnemyState.TransitionTypes[Mathf.FloorToInt(transitionType * EnemyState.TransitionTypes.Length)];
				_states[i].TransitionArguments[j] = 
					RNG.GenerateValue("State " + i.ToString() + " Transition Argument " + j.ToString());
			}
		}

		_states [0].OtherState [0] = _states [1];
		_states [0].OtherState [1] = _states [2];
		_states [1].OtherState [0] = _states [0];
		_states [1].OtherState [1] = _states [2];
		_states [2].OtherState [0] = _states [0];
		_states [2].OtherState [1] = _states [1];

		SetActiveState (_states [0]);
	}

	public override void BeginStep() {
		_currentState.PlanMovement ();
	}

	public override void EndStep() {
		GameObject g = coll.CheckCollision (CollisionCategory.PlayerAttack);
		if (g) {
			Health -= g.GetComponent<Bullet>().Damage;
			Destroy(g);
		}
	}

	public void SetActiveState(EnemyState state) {
		if (_spriteRenderer)
			_spriteRenderer.color = Utils.HSVToRGB (state.StateHue, .75f, 1f);
		if (_currentState != null)
			_currentState.ExitState ();
		state.EnterState ();
		_currentState = state;
	}

	new public void HandleInput(int input) {
		base.HandleInput (input);
	}

	override public void Landed() {
		base.Landed();
		_currentState.Landed();
	}
	override public void WallHit(Direction contactDir) {
		base.WallHit (contactDir);
		_currentState.WallHit (contactDir);
	}
}
