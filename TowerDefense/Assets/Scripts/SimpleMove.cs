using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {
	public Vector3 moveTo;
	public int health;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, moveTo, .5f * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Projectile")) {
			health -= other.gameObject.GetComponent<BulletController>().damage;
			Destroy (other.gameObject);

			if (health <= 0) {
				Destroy (gameObject);
			}
		}
	}
}
