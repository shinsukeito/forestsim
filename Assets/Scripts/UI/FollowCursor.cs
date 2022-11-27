using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
	public float speed = 50;
	public bool toWorld = true;

	Vector2 position;

	// Start is called before the first frame update
	void Start()
	{
		Vector2 worldPos = toWorld ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
		transform.position = position = new Vector2(worldPos.x, worldPos.y);
	}


	// Update is called once per frame
	void Update()
	{
		Vector2 worldPos = toWorld ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
		position = position + (worldPos - position) * Time.deltaTime * speed;

		transform.position = position;
	}
}
