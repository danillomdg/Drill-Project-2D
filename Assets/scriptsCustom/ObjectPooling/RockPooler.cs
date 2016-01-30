using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockPooler : MonoBehaviour {
	public GameObject Player;
	public GameObject Terrain;
	private PolygonGenerator tscript;
	private NewObjectPoolerScript poola;
	private int size = 7;
	private float timar=0;
	// Use this for initialization
	void Start () {
		tscript = Terrain.GetComponent("PolygonGenerator") as PolygonGenerator;
		poola = GetComponent<NewObjectPoolerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		PlaceRocks();
		timar += Time.deltaTime;
		if (timar >= 1)
		{

			timar = 0;
		}
	}

	public void PlaceRocks(){
		int x = Mathf.FloorToInt(Player.transform.position.x);
		int y = Mathf.FloorToInt(Player.transform.position.y);
		
		for(int px=x-size;px<x+size+1;px++){
			for(int py=y-size;py<y+size+1;py++){
				if (px >=0 && px < tscript.blocks.GetLength(0) && -py >=0 && -py <tscript.blocks.GetLength(1))
				{
			
					if (tscript.blocks[px,-py] == 20)
					{


						if (!poola.VerifyPosition(new Vector2(px+0.5f, py-0.5f)))
						{
						
							int i = poola.GetPooledObject();
						if (i != -100)
						{

						poola.pooledObjects[i].transform.position = new Vector2(px+0.5f, py-0.5f);
						poola.pooledObjects[i].SetActive(true);
							}



						}
	
					}
				}	
			}
		}
	}



}
