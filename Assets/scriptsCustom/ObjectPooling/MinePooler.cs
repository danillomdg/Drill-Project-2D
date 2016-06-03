﻿using UnityEngine;
using System.Collections;

public class MinePooler : MonoBehaviour {

	public GameObject Player;
	public GameObject Terrain;
	private PolygonGenerator tscript;
	private NewObjectPoolerScript poola;
	private int size = 3;
	private float timar=0;

	private Color color;


	
	// Use this for initialization
	void Start () {
		tscript = Terrain.GetComponent("PolygonGenerator") as PolygonGenerator;
		poola = GetComponent<NewObjectPoolerScript>();
		
	}
	
	// Update is called once per frame
	void Update () {
		PlaceMines();


	}


	public void PlaceMines(){
		int x = Mathf.FloorToInt(Player.transform.position.x);
		int y = Mathf.FloorToInt(Player.transform.position.y);
		
		for(int px=x-size;px<x+size+1;px++){
			for(int py=y-size;py<y+size+1;py++){
				if (px >=0 && px < tscript.blocks.GetLength(0) && -py >=0 && -py <tscript.blocks.GetLength(1))
				{
					
					if (tscript.blocks[px,-py] == 70)
					{
						
						
						if (!poola.VerifyPosition(new Vector2(px+0.44f, py-0.557f)))
						{
							
							int i = poola.GetPooledObject();
							if (i != -100)
							{
								
								poola.pooledObjects[i].transform.position = new Vector3(px+0.44f, py-0.557f,-0.01f);
								poola.pooledObjects[i].SetActive(true);
								Landmine rand = poola.pooledObjects[i].GetComponent<Landmine>();
								rand.sendoUsada = false;
								Renderer rend = poola.pooledObjects[i].GetComponent<Renderer>();
								//rend.enabled = false;
								color = rend.material.color;
								color.a = 0;
								rend.material.color = color;
							}
							
							
							
						}
						
					}
				}	
			}
		}
	}



}
