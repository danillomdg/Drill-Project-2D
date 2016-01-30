using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerItens : MonoBehaviour {

	public GameObject bomb;
	[HideInInspector]
	public bomb bombie;
	// Use this for initialization
	void Start () {
		bombie = bomb.GetComponent("bomb") as bomb;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
