using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	public Sprite damageSprite;
	public int hp = 4;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void damageWall(int loss) {
		spriteRenderer.sprite = damageSprite;
		hp -= loss;

		if (hp <= 0) {
			gameObject.SetActive(false); 
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
