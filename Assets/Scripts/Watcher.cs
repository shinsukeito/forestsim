using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
	public AnimationCurve zoomCurve;

	private Vector2 lastDragPosition;
	private Vector2 speed;

	private Terraformer terraformer;
	private float zoom = 3;
	private float trueZoom = 3;
	private float prevZoom = 3;
	private bool canZoom;
	private float zoomTime = 1;
	private float finalZoomKeyTime = 0;

	private Camera cam;


	// Start is called before the first frame update
	void Start()
	{
		terraformer = FindObjectOfType<Terraformer>();
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
			Vector2 nextSpeed = speed + (lastDragPosition - screenMousePosition) * 10f;

			if (nextSpeed.magnitude < 10)
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

		Vector2 dims = terraformer.GetDimensions() / 2f;
		Vector3 nextPos = Vector2.ClampMagnitude((Vector2)cam.transform.position + (speed * Time.deltaTime), Mathf.Max(dims.x, dims.y));
		nextPos.z = -5;
		cam.transform.position = nextPos;
	}

	void UpdateZoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom > 3)
		{
			prevZoom = trueZoom;
			zoom -= 0.5f;
			zoomTime = 0;
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom < 10)
		{
			prevZoom = trueZoom;
			zoom += 0.5f;
			zoomTime = 0;
		}

		if (zoomTime < finalZoomKeyTime)
		{
			zoomTime += Time.deltaTime;
			trueZoom = prevZoom + zoomCurve.Evaluate(zoomTime) * (zoom - prevZoom);
		}
		else
		{
			trueZoom = zoom;
		}

		cam.orthographicSize = trueZoom;
	}
}
