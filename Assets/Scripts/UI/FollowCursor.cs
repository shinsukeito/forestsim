using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
	public float speed = 50;

	Vector2 position;

	// Start is called before the first frame update
	void Start()
	{
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = position = new Vector2(worldPos.x, worldPos.y);
	}


	// Update is called once per frame
	void Update()
	{
		Vector2 worldPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		position = position + (worldPos - position) * Time.deltaTime * speed;

		transform.position = position;
	}
}
