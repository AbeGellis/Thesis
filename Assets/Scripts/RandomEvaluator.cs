using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEvaluator : Evaluator {
	public List<int> commands;

	void Start () {
		StepBasedComponent.Frame = 0;
		commands = new List<int> (SimulationDepth);
		int x = Controls.Left | Controls.Right | Controls.Jump | Controls.Shoot;
		for (int i = 0; i < SimulationDepth; ++i) {
			commands.Add(Random.Range(0, x + 1));
		}
		Time.timeScale = 100f;
		StartCoroutine (Simulate (commands, RenderFrequency));
	}

	void OnDisable() {
		Time.timeScale = 1f;
	}

	public IEnumerator Simulate(List<int> commands, int renderFrequency) {
		yield return new WaitForEndOfFrame ();
		int timer = renderFrequency;

		for (int i = 0; i < commands.Count; ++i) {
			Hero.InputPressed = commands[i];
			for (int j = 0; j < CycleLength; ++j) {
				StepBasedComponent.GameStep();

				--timer;
				if (timer <= 0) {
					timer += renderFrequency;
					yield return new WaitForEndOfFrame();
				}
			}
			if (Enemy.Health <= 0) {
				if (OnFail != null)
					OnFail(this);
				yield break;
			}

			if (Hero.Health <= 0) 
				break;

		}

		if (OnSucceed != null)
			OnSucceed(this);
	}
}
