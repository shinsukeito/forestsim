using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
	[Header("References")]
	public Camera cam;
	public Terraformer terraformer;

	[Header("Configurations")]
	public AnimationCurve zoomCurve;
	[Range(0, 50)]

	public float scrollSpeed;
	[Range(0, 50)]
	public float maxZoom = 10;
	[Range(0, 10)]
	public int zoomSpeed = 1;

	private Vector2 lastDragPosition;
	private Vector2 speed;

	private float targetZoom = 3;
	private float trueZoom = 3;
	private float prevZoom = 3;
	private float zoomTime = 1;
	private float finalZoomKeyTime = 0;

	// Start is called before the first frame update
	void Start()
	{
		cam = GetComponent<Camera>();
		finalZoomKeyTime = zoomCurve.keys[zoomCurve.keys.Length - 1].time;
	}

	// Update is called once per frame

	void Update()
	{
		UpdateDrag();
		UpdateZoom();
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
		Vector2 dims = terraformer.GetDimensions() * cam.aspect / trueZoom;
		nextPos = new Vector2(
			Mathf.Clamp(nextPos.x, -dims.x, dims.x),
			Mathf.Clamp(nextPos.y, -dims.y, dims.y)
		);
		nextPos.z = -5;
		cam.transform.position = nextPos;
	}

	void UpdateZoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && targetZoom > 3)
		{
			prevZoom = trueZoom;
			targetZoom -= zoomSpeed;
			zoomTime = 0;
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0 && targetZoom <= maxZoom)
		{
			prevZoom = trueZoom;
			targetZoom += zoomSpeed;
			zoomTime = 0;
		}

		if (zoomTime < finalZoomKeyTime)
		{
			zoomTime += Time.deltaTime;
			trueZoom = prevZoom + zoomCurve.Evaluate(zoomTime) * (targetZoom - prevZoom);
		}
		else
		{
			trueZoom = targetZoom;
		}

		cam.orthographicSize = trueZoom;
	}
}
