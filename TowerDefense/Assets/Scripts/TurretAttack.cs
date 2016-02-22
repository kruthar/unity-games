using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretAttack : MonoBehaviour {
	public GameObject projectile;
	public float fireDelay;

	private Transform barrel;
	private Transform barrelCylinder;
	private List<GameObject> objectsInRadius;
	private float fireTimer;

	// Use this for initialization
	void Start () {
		barrel = transform.FindChild ("Barrel");
		barrelCylinder = barrel.transform.FindChild ("Cylinder");
		objectsInRadius = new List<GameObject>();
		fireTimer = fireDelay;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject toFireAt = null;
		int index = 0;
		while (index < objectsInRadius.Count && toFireAt == null) {
			toFireAt = objectsInRadius[index];
			index++;
		}

		if (index > 0) {
			objectsInRadius.RemoveRange (0, index - 1);
		}

		if (toFireAt != null) {
			turnBarrelTo (toFireAt.transform.position);
			fireAt(toFireAt);
		}
	}

	void OnTriggerEnter(Collider other) {
		objectsInRadius.Add (other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		objectsInRadius.Remove (other.gameObject);
	}

	void turnBarrelTo(Vector3 point) {
		Vector3 lookAt = point;
		lookAt.y = barrel.transform.position.y;
		barrel.transform.LookAt (lookAt);
	}

	void fireAt(GameObject toFireAt) {
		fireTimer += Time.deltaTime;

		if (fireTimer >= fireDelay) {
			GameObject fired = (GameObject)Instantiate (projectile, barrelCylinder.position, Quaternion.identity);
			fired.GetComponent<BulletController> ().setTarget (toFireAt);
			fireTimer = 0f;
		}
	}
}
