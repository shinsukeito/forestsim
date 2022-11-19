using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
	public float truePercent = 1f;
	public float prevPercent = 1f;
	public float targetPercent = 0.5f;
	public Transform fill;
	public AnimationCurve curve;

	private float time = 1;
	private float finalKeyTime = 0;

	// Start is called before the first frame update
	void Start()
	{
		finalKeyTime = curve.keys[curve.keys.Length - 1].time;
	}

	// Update is called once per frame
	void Update()
	{
		UpdateHealth();
	}

	public void SetFill(float percent)
	{
		prevPercent = truePercent;
		targetPercent = percent;
		time = 0;
	}

	void UpdateHealth()
	{
		if (time < finalKeyTime)
		{
			time += Time.deltaTime;
			truePercent = prevPercent + curve.Evaluate(time) * (targetPercent - prevPercent);
		}
		else
		{
			truePercent = targetPercent;
			if (truePercent <= 0)
			{
				Destroy(this.gameObject);
			}
		}

		fill.localScale = new Vector3(truePercent, 1, 0);
	}
}
