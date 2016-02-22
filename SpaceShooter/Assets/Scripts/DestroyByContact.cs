using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject asteriodExplosion;
	public GameObject playerExplosion;
	public int scoreValue;

	private GameObject gameControllerObject;
	private GameController gameController;

	void Start () {
		gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
			if (gameController == null) {
				Debug.Log ("Cannot find GameController script");
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Boundary")) {
			return;
		}

		Instantiate (asteriodExplosion, transform.position, transform.rotation);
		if (other.CompareTag ("Player")) {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			gameController.gameOver();
		}

		Destroy (other.gameObject);
		Destroy (gameObject);

		gameController.addScore (scoreValue);
	}
}
