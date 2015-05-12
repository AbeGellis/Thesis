using UnityEngine;
using System.Collections.Generic;

public class RandomNumberGenerator : MonoBehaviour {

	public struct GeneratedValue
	{
		public GeneratedValue(float value, bool discrete) {
			Value = value;
			Discrete = discrete;
		}
		public float Value;
		public bool Discrete;
	}

	private Dictionary<string, GeneratedValue> GeneratedValues = new Dictionary<string, GeneratedValue> ();

	public float GenerateValue(string Field, bool discrete) {
		if (GeneratedValues.ContainsKey (Field))
			return GeneratedValues [Field].Value;
		else {
			float val = Random.Range (0f,1f);
			GeneratedValues.Add(Field, new GeneratedValue(val, discrete));
			return val;
		}
	}
}
