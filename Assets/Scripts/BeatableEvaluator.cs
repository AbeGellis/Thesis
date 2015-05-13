﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class BeatableEvaluator : MonoBehaviour {
	public ComputerControlledPlayer Hero;
	public Player Enemy;
	public Bullet[] Bullets;
	public int RenderFrequency = 5;
	public int MaxSimulationDepth = 30;
	
	public int Granularity = 2;

	void Awake() {
		UnityEngine.Random.seed = (int) System.DateTime.Now.Ticks;
	}

	public class HealthHeuristic : IComparer<Priority> {
		public int Compare(Priority p1, Priority p2) {
			if (p1.PrimaryVal == p2.PrimaryVal) {
				if (p1.SecondaryVal == p2.SecondaryVal) {
					return Math.Sign(p1.id - p2.id);
				}
				return Math.Sign(p1.SecondaryVal - p2.SecondaryVal);
			}
			return p1.PrimaryVal - p2.PrimaryVal;
		}
	}

	public struct Priority {
		static long current_id = long.MinValue;

		public Priority(int val1, float val2) {
			PrimaryVal = val1;
			SecondaryVal = val2;
			id = current_id;
			++current_id;
		}
		public int PrimaryVal;
		public float SecondaryVal;
		public long id;

		public int CompareTo(object other) {
			if (other.GetType () == typeof(Priority))
				return CompareTo ((Priority)other);
			else
				return 0;
		}
	}

	private Priority CalculatePriority(GameState state) {
		int HPDiff = state.EnemyState.Character.Health - state.PlayerState.Character.Health;

		Vector2 ePos = state.EnemyState.Position;
		float BulletDist = float.MaxValue;

		foreach (GameObject b in state.PlayerState.Character.Bullets) {
			if (b.activeSelf)
				BulletDist = Mathf.Min (BulletDist, Vector2.SqrMagnitude ((Vector2) b.transform.position - ePos));
		}
		//if (BulletDist == float.MaxValue)
		//	BulletDist = Mathf.Min (BulletDist, Vector2.SqrMagnitude ((Vector2) state.PlayerState.Position - ePos));

		return new Priority (HPDiff, BulletDist);
	}

	private SortedDictionary<Priority,GameState> ToExplore;

	void Start () {
		ToExplore = new SortedDictionary<Priority, GameState> (new HealthHeuristic());
		GameState g = new GameState (Hero, Enemy, Bullets, null);
		ToExplore.Add (CalculatePriority (g), g);
		Time.timeScale = 100f;
		StartCoroutine (Simulate(MaxSimulationDepth,RenderFrequency));
	}

	void OnDisable() {
		Time.timeScale = 1f;
	}

	//TODO use maxDepth
	public IEnumerator Simulate(int maxDepth, int dequeuesPerUpdate) {
		int timer = dequeuesPerUpdate;

		while (ToExplore.Count != 0) {
			GameState g = null;
			Priority p = new Priority (int.MaxValue, int.MaxValue);
			foreach (var k in ToExplore) {
				g = k.Value;
				p = k.Key;
				break;
			}
			if (g == null)
				break;

			ToExplore.Remove(p);

			--timer;
			if (timer <= 0) {
				timer += dequeuesPerUpdate;
				yield return new WaitForEndOfFrame();
			}

			for (int i = 0; i < (Controls.Jump | Controls.Left | Controls.Right | Controls.Shoot); ++i) {
				g.RestoreGameState ((Player)Hero, Enemy, Bullets);
				if (Hero.UsefulInput (i)) {
					Hero.InputPressed = i;
					for (int j = Granularity; j > 0; --j)
						StepBasedComponent.GameStep ();
					GameState m = new GameState (Hero, Enemy, Bullets, g);
					if (m.PlayerState.Character.Health > 0) {
						if (m.EnemyState.Character.Health == 0) {
							Debug.Log("Boss is beatable in " + m.Frame + " frames!");
							Application.LoadLevel("main");
							yield break;
						}
						ToExplore.Add (CalculatePriority (m), m);
					}
				}
			}
		}
	}
}