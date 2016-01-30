using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	[HideInInspector]
	public float HP = 100;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public virtual void Deactivate()
	{

	}
}
