using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	[HideInInspector] public static GameController instance = null;
	public Text waterText;
	public Text foodText;

	private int food = 0;
	private int water = 0;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		updateFood (0);
		updateWater (0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateFood(int amount) {
		food += amount;
		foodText.text = "Food: " + food;
	}

	public void updateWater(int amount) {
		water += amount;
		waterText.text = "Water: " + water;
	}
}
