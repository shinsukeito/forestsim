using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
	[Header("References")]
	public Camera cam;
	public Terraformer terraformer;

	[Header("Configurations")]
	[Range(0, 50)]
	public float scrollSpeed;
	public Vector2 offset = new Vector2(-2, 0); // 128px, 0px

	private Vector2 lastDragPosition;
	private Vector2 speed;

	// Start is called before the first frame update
	void Start()
	{
		cam = GetComponent<Camera>();
	}

	// Update is called once per frame

	void Update()
	{
		UpdateDrag();
	}

	void UpdateDrag()
	{
		Vector2 screenMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButtonDown(1))
			lastDragPosition = screenMousePosition;
		if (Input.GetMouseButton(1))
		{
			Vector2 nextSpeed = speed + (lastDragPosition - screenMousePosition) * scrollSpeed;
			speed = nextSpeed;

			lastDragPosition = screenMousePosition;
		}
		else
		{
			if (Mathf.Abs(speed.x) > 0.01) speed.x *= 0.99f;
			else speed.x = 0;

			if (Mathf.Abs(speed.y) > 0.01) speed.y *= 0.99f;
			else speed.y = 0;
		}

		Vector3 nextPos = (Vector2)cam.transform.position + (speed * Time.deltaTime);
		Vector2 dims = terraformer.GetDimensions() * 0.5f - new Vector2(5.5f, 4.6875f);
		nextPos = new Vector2(
			Mathf.Clamp(nextPos.x, -dims.x + offset.x, dims.x + offset.x),
			Mathf.Clamp(nextPos.y, -dims.y + offset.y, dims.y + offset.y)
		);
		nextPos.z = -5;
		cam.transform.position = nextPos;
	}
}
