using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider))]
public class Rock : Enemy {
	
	public GameObject gameManager;
	private GameManager ManagerGame;
	
	//COLISION VARIABLES
	public LayerMask collisionMask;
	private new BoxCollider collider;
	private Vector3 s;
	private Vector3 c;
	private float skin = .005f;
	public int ColmaskRedux =5;
	Ray ray;
	RaycastHit hit;
	//END
	
	public GameObject Terrain;
	public GameObject Player;
	private PolygonGenerator tscript;
	private PlayerStats StatusPlayer;
	
	public int quantidade;
	public List<Vector2> UnityPosition = new List<Vector2>();
	private float targetospeed, currentospeed;
	private float spid = 5;
	private float acceleraçao = 15;
	
	private float timeru = 0;
	
	//	private float gravidade = 10;
	public bool safe;
	public bool muitolonje;
	public bool sendoUsada;
	private int baseHP = 10;
	// Use this for initialization
	void Start () {

		tscript = Terrain.GetComponent("PolygonGenerator") as PolygonGenerator; 
		ManagerGame = gameManager.GetComponent ("GameManager") as GameManager;
		StatusPlayer = Player.GetComponent("PlayerStats") as PlayerStats;
		
		collider = GetComponent<BoxCollider>();
		s = collider.size;
		c = collider.center;
		HP = baseHP;
	}
	
	
	
	void OnTriggerEnter (Collider col)
	{
		
		
		if (col.gameObject.name == "DrillFighter")
		{
			if (safe == false)
				StatusPlayer.HP = 0;
			
			
		} 
		
	}
	
	// Update is called once per frame
	void Update () {
		if (HP <= 0)
		{
			HP = baseHP;
			int xis = Mathf.RoundToInt(transform.position.x-0.5f);
			int yips = Mathf.RoundToInt(transform.position.y+0.5f);
			try{
				if (tscript.blocks[xis,-yips] == -1)
					tscript.blocks[xis,-yips] = 0;
			}
			catch (System.Exception e)
			{}
			DeactivateRock();

		}
// MELHORAR OS CALCULOS DE DISTANCIA
		var distance = Vector3.Distance(Player.transform.position, transform.position);
		
		if (distance >= 16) 
			DeactivateRock();
		else muitolonje = false;
		
		targetospeed =  -1 * spid;
		

		
		
		//(IN)SENSATEZ ORIGINAL
		
		if (safe == false)
		{
			//print("nao to seguro");
			int xis = Mathf.RoundToInt(transform.position.x-0.5f);
			int yips = Mathf.RoundToInt(transform.position.y+0.5f);
			try{
				if (tscript.blocks[xis,-yips] == -1)
					tscript.blocks[xis,-yips] = 0;
				}
			catch (System.Exception e)
			{}
			
			timeru += 1 * Time.deltaTime;
			
			if (timeru > 0.2f)
			{
				currentospeed = IncrementTowards(currentospeed,targetospeed - 3,acceleraçao);

				Move (new Vector2(0, currentospeed)* Time.deltaTime) ;
			}
		}
		else {
		//	print("seguro as fuark");
			int xis = Mathf.RoundToInt(transform.position.x-0.5f);
			int yips = Mathf.RoundToInt(transform.position.y +0.5f);
			try{ 
				tscript.blocks[xis,-yips] = -1;
			   }	catch (System.Exception e) {}
			timeru = 0;


			int x = Mathf.FloorToInt(transform.position.x);
			int y = Mathf.FloorToInt(transform.position.y+1);
			
			

				if (x >=0 && x < tscript.blocks.GetLength(0) && -(y-1) >=0 && -(y-1) < tscript.blocks.GetLength(1))
				{
					
					if (tscript.blocks[x,-(y-1)] == 0)
						
					{
						safe = false;
					}
				}
		}

	}


	public void DeactivateRock()
	{

		//transform.position = new Vector3(6.51f,-0.9f,-0.1f);
		transform.position = new Vector3(-99f,0,-0.1f);
		gameObject.SetActive(false);
		muitolonje = true;
		sendoUsada = false;
	}
	
	public void Move(Vector2 moveAmount) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		//Vector2 p = transform.position;


		int x = Mathf.FloorToInt(transform.position.x);
		float y = transform.position.y;

		float dir = Mathf.Sign(deltaY);
		try
		{
		if (tscript.blocks[x,Mathf.RoundToInt(-1*(y+deltaY))] != 0 )
		{
			float passY = Mathf.FloorToInt(((y-0.5f)- deltaY));
			deltaY = Mathf.Abs((y-0.5f)) - Mathf.Abs((passY)) ;
			//deltaY = deltaY * dir;
			//print ("amigo estou aqui");
			currentospeed = 0;
			safe = true;
			timeru = 0;
		}
	}
		catch (System.IndexOutOfRangeException e)
		{}


		
		Vector2 finalTransform = new Vector2(deltaX,deltaY);		
		transform.Translate(finalTransform,Space.World);
	}
	
	public float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
			
		}
	}
	
	
}