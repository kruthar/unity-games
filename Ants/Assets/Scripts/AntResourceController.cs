using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AntResourceController : MonoBehaviour {
	public GameObject waterObject;
	public GameObject foodObject;

	[HideInInspector] public bool returning = false;
	private GameObject objectCarrying;
	private Vector3 waterObjectOffset = new Vector3(0, .5f, 1.5f);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (returning && objectCarrying != null) {
			objectCarrying.transform.position = transform.position;
			objectCarrying.transform.Translate(transform.forward * 1.5f);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("WaterSource") && !returning) {
			returning = true;
			objectCarrying = (GameObject) Instantiate (waterObject, transform.position + waterObjectOffset, Quaternion.identity);
		}

		if (other.CompareTag("FoodSource") && !returning) {
			returning = true;
			objectCarrying = (GameObject) Instantiate (foodObject, transform.position + waterObjectOffset, Quaternion.identity);
		}

		if (other.name == "MoundEntrance") {
			if (returning && objectCarrying != null) {
				if (objectCarrying.CompareTag("Water")) {
					GameController.instance.updateWater(10);
				} else if (objectCarrying.CompareTag("Food")) {
					GameController.instance.updateFood(10);
				}
				Destroy (objectCarrying);
				objectCarrying = null;
				returning = false;
			}
		}
	}
}
