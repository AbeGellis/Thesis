using UnityEngine;
using System.Collections.Generic;

public class RandomNumberGenerator : MonoBehaviour {
	[System.Serializable]
	class RandomField {
		public string name;
		public float value;
		public RandomField(string name, float value) {
			this.name = name;
			this.value = value;
		}
	}

	[SerializeField]
	private List<RandomField> GeneratedValues = new List<RandomField> ();

	public float GenerateValue(string Field) {
		float value = Random.Range (0f, 1f);
		GeneratedValues.Add (new RandomField (Field, value));
		return value;
	}

	void Awake() { 
	}
}
