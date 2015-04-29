using UnityEngine;
using System.Collections;
using System;

public class EnemyPlayer : Player {
	public RandomNumberGenerator RNG;
	public Player foe;

	[SerializeField]
	private EnemyState _currentState;
	[SerializeField]
	private EnemyState[] _states = new EnemyState[3];

	private EnemyState GenerateState (float typeVal) {
		//return (EnemyState) Activator.CreateInstance(EnemyState.StateTypes[Mathf.FloorToInt(typeVal * EnemyState.StateTypes.Length)]);
		return (EnemyState) ScriptableObject.CreateInstance(EnemyState.StateTypes[Mathf.FloorToInt(typeVal * EnemyState.StateTypes.Length)]);
	}

	private FiringPattern GenerateFiringPattern (float typeVal) {
		//return (FiringPattern) Activator.CreateInstance(FiringPattern.PatternTypes[Mathf.FloorToInt(typeVal * FiringPattern.PatternTypes.Length)]);
		return (FiringPattern) ScriptableObject.CreateInstance(FiringPattern.PatternTypes[Mathf.FloorToInt(typeVal * FiringPattern.PatternTypes.Length)]);
	}

	private SpriteRenderer _spriteRenderer;

	public void Start() {
		_spriteRenderer = GetComponent<SpriteRenderer> ();

		for (int i = 0; i < 3; ++i) {
			float val = RNG.GenerateValue("State " + i.ToString() + " Type");
			_states[i] = GenerateState(val);

			_states[i].ToControl = this;

			_states[i].StateHue = RNG.GenerateValue("State " + i.ToString() + " Hue");

			for (int j = 0; j < 2; ++j) 
				_states[i].Arguments[j] = RNG.GenerateValue("State " + i.ToString() + " Argument " + j.ToString());
			for (int j = 0; j < 2; ++j) {
				float transitionType = RNG.GenerateValue("State " + i.ToString() + " Transition Type " + j.ToString());
				_states[i].TransitionConditions[j] = 
					EnemyState.TransitionTypes[Mathf.FloorToInt(transitionType * EnemyState.TransitionTypes.Length)];
				_states[i].TransitionArguments[j] = 
					RNG.GenerateValue("State " + i.ToString() + " Transition Argument " + j.ToString());
			}

			val = RNG.GenerateValue("State " + i.ToString() + " Firing Pattern Type");
			_states[i].FirePattern = GenerateFiringPattern(val);

			for (int j = 0; j < 2; ++j) 
				_states[i].FirePattern.Arguments[j] = RNG.GenerateValue("State " + i.ToString() + " Firing Pattern Argument " + j.ToString());
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
			g.SetActive(false);
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
