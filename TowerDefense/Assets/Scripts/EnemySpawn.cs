using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
	public GameObject enemy;
	public float spawnDelay;

	private float spawnTimer = 0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer += Time.deltaTime;

		if (spawnTimer >= spawnDelay) {
			spawnTimer = 0f;
			Vector3 spawnPosition = transform.position;
			spawnPosition.y = .5f;
			Instantiate (enemy, spawnPosition, Quaternion.identity);
		}
	}
}
