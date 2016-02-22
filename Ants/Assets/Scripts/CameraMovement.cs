using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public GameObject player;
	public Vector3 cameraOffset;

	// Use this for initialization
	void Start () {
		transform.position = player.transform.position + cameraOffset;
		transform.LookAt (player.transform);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position + cameraOffset;
		transform.rotation = Quaternion.identity;

		Vector3 cameraVector = transform.forward;
		Vector3 playerVector = player.transform.forward;
		cameraVector.y = 0;
		playerVector.y = 0;
		float rotationAngle = Vector3.Angle (cameraVector, playerVector);
		Vector3 rotationDirection = Vector3.Cross (cameraVector, playerVector);

		if (rotationDirection.y < 0f) {
			rotationAngle *= -1;
		}

		transform.RotateAround (player.transform.position, Vector3.up, rotationAngle);
		transform.LookAt (player.transform);
	}
}
