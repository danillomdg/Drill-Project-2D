using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeoMonster : MonoBehaviour {
	
	public GameObject terrain;
	public GameObject Player;
	private PolygonGenerator tScript;
	private PlayerControl PlayerMovement;
	
	//public variables:
	public bool following = true;
	public bool transcend = false;
	public Vector2[] playerTraces;
	public Vector2[] oldPlayerTraces;
	
	
	public int faceLR = 1; //1 = direita, 0 = nada, -1 = esquerda 
	public int faceUD = 0; // 1 = cima, 0 = nada, -1 = baixo
	public bool ComplexMoves = false;
	
	//passar pra private depois de testado:
	public float ProAuxUD, ProAuxLR = -100;
	public bool esquina = false;
	//rastro:
	List<Vector2> rastro;
	float rastroTempo = 0;
	
	
	private float flDist = 0.3f; //floor distance
	
	
	
	// Use this for initialization
	void Start () {
		tScript=terrain.GetComponent("PolygonGenerator") as PolygonGenerator; 
		rastro = new List<Vector2>();
		playerTraces = new Vector2[20];
		playerTraces[0] = new Vector2(0,0);
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	public void Move (){
		float posX = transform.position.x;
		float posY = -1 * transform.position.y;
		
		
		if (posY <= 4)
		{ if (faceUD >= 1)
			{faceUD = 0;
				faceLR = -1;}
		}
		
			
			if(Mathf.Abs(Player.transform.position.x - playerTraces[0].x) >= 1  || Mathf.Abs(Player.transform.position.y - playerTraces[0].y) >= 1) 
				
			{
				if (rastro.Count >= 20)
					rastro.RemoveAt(rastro.Count - 1);
				  rastro.Insert(0,new Vector2(Player.transform.position.x,Player.transform.position.y));
				
				
			}
			
			print ("COUNTOO: "+rastro.Count+"  traces desu "+ playerTraces[0]+" || Puraiar desu "+Player.transform.position);
			
			faceLR = Mathf.Clamp(Mathf.RoundToInt(rastro[rastro.Count -1].x - transform.position.x),-1,1);
			faceUD = Mathf.Clamp(Mathf.RoundToInt(rastro[rastro.Count -1].y - transform.position.y),-1,1);
			
			if (Mathf.FloorToInt(rastro[rastro.Count -1].x) == Mathf.FloorToInt(transform.position.x) && Mathf.FloorToInt(rastro[rastro.Count -1].y) == Mathf.FloorToInt(transform.position.y))
			{
				rastro.RemoveAt(rastro.Count-1);
				print ("yummy!");
			}
			
			if (transcend == false)
			{
				if (tScript.blocks[Mathf.FloorToInt(posX + flDist * faceLR),Mathf.FloorToInt(posY - flDist * faceUD)] != 0)
				{
					if (tScript.blocks[Mathf.FloorToInt(posX + flDist * faceLR),Mathf.FloorToInt(posY)] != 0)
						faceLR = 0;
					if (tScript.blocks[Mathf.FloorToInt(posX),Mathf.FloorToInt(posY - flDist * faceUD)] != 0)
						faceUD = 0;
					
				}
				
			}
			
			playerTraces[0] = Player.transform.position;
			
		
		
		Vector2 finalTransform = new Vector2(faceLR * 2.5f * Time.deltaTime,faceUD * 2.5f * Time.deltaTime);
		transform.Translate(finalTransform,Space.World);
	}
}

