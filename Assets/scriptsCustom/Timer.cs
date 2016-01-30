using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float value;
	private float factor = 1;
	private float preValue;
	public bool working;

	// Use this for initialization
	void Start () {
		value = preValue;
	}
	
	// Update is called once per frame
	void Update () {
		if (working == true)
		{
			value -= factor * Time.deltaTime;
			if (value <= 0)
			{
				value = preValue;
				working = false;
			}
		}
		else value = preValue;

		}
	public void SetValues(float preValue, float factor)
	{
		this.preValue = preValue;
		this.factor = factor;

	}

	public Vector2 GetValues ()
	{
		return new Vector2(preValue,factor);
	}

}
