using UnityEngine;
using System.Collections;

public abstract class Evaluator : MonoBehaviour {
	
	public ComputerControlledPlayer Hero;
	public Player Enemy;
	public Bullet[] Bullets;
	public int RenderFrequency = 5;
	public int SimulationDepth = 30;
	public int CycleLength = 2;

	
	public delegate void EvaluationEvent(Evaluator sender, int data = 0);
	public static EvaluationEvent OnSucceed, OnFail;
}
