using UnityEngine;
using System.Collections;

public class Boom : MonoBehaviour {
	private PlayerStats StatusPlayer;
	private Enemy inimigo;
	public float pretimer = 0.7f;
	public float timer;
	float blowRadius = 1.1f;
	float damagePowerMulti = 52; 
	// Use this for initialization
	void Start () {
		timer = pretimer;
	}
	
	// Update is called once per frame
	void Update () {
		timer-= Time.deltaTime;
		if (timer <= 0)
		{
			gameObject.SetActive(false);
			timer = pretimer;
		}
		destruction();
	}

	void destruction()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, blowRadius);
		foreach (Collider corida in hitColliders)
		{
			if (corida.gameObject.name != ("DrillFighter") && corida.gameObject.name != ("Boom") && corida.gameObject.name != ("Terrain"))
				{
				Enemy WhatRThose = corida.gameObject.GetComponent<Enemy>();
				if (WhatRThose != null)
				{
			
					WhatRThose.HP -= Time.deltaTime * (damagePowerMulti/pretimer);
				}
			}
// A LINHA A SEGUIR E PROVISORIA:

			else if (corida.gameObject.name == ("DrillFighter"))
			{
				StatusPlayer = corida.GetComponent("PlayerStats") as PlayerStats;
     			StatusPlayer.TakeDamage(Time.deltaTime * (damagePowerMulti/pretimer),pretimer);
			}
		}
	}

	public void giveInfo(GameObject monster)
	{
		Vector2 bombintel = new Vector2(pretimer,timer);
		monster.SendMessage("getBombInfo",bombintel);
	}
}
