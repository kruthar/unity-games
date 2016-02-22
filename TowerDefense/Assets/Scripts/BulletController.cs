using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	public int damage;

	private GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.position = Vector3.Lerp (transform.position, target.transform.position, 10f * Time.deltaTime);
		}
	}

	public void setTarget(GameObject t) {
		target = t;
	}
}
