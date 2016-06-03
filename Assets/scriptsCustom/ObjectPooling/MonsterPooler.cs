using UnityEngine;
using System.Collections;

public class MonsterPooler : MonoBehaviour {

	public GameObject Player;
	public GameObject Terrain;
	private PolygonGenerator tscript;
	private NewObjectPoolerScript poola;
	private int size = 7;

	// Use this for initialization
	void Start () {
		tscript = Terrain.GetComponent("PolygonGenerator") as PolygonGenerator;
		poola = GetComponent<NewObjectPoolerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		SpawnMonster();
	}

	public void SpawnMonster(){
		int x = Mathf.FloorToInt(Player.transform.position.x);
		int y = Mathf.FloorToInt(Player.transform.position.y);
		
		for(int px=x-size;px<x+size+1;px++){
			for(int py=y-size;py<y+size+1;py++){
				if (px >=0 && px < tscript.blocks.GetLength(0) && -py >=0 && -py <tscript.blocks.GetLength(1))
				{
					
					if (tscript.blocks[px,-py] == 22)
					{
						
						
						if (!poola.VerifyPosition(new Vector2(px+0.5f, py-0.5f)))
						{
							
							int i = poola.GetPooledObject();
							if (i != -100)
							{
								
//								print ("Spawming mosta");
								poola.pooledObjects[i].transform.position = new Vector3(px+0.5f, py-0.5f,-0.01f);
								tscript.blocks[px,-py] = 0;
								poola.pooledObjects[i].SetActive(true);
							}
							
							
						}
					}
					//else print ("retornou false");
				}	
			}
		}
	}
}
