using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : Enemy {
	public int waitFrame;

	public GameObject terrain;
	public GameObject Player;
	private PolygonGenerator tScript;
	private PlayerControl PlayerMovement;
	private PlayerStats StatusPlayer;

	private Animator animata;
	//public variables:
	private bool following = false;
	private bool transcend = false;
	private bool moveIt = true;
	public Vector2 TracingCurves;

	public int faceLR = 1; //1 = direita, 0 = nada, -1 = esquerda 
	public int faceUD = 0; // 1 = cima, 0 = nada, -1 = baixo
	private bool ComplexMoves = false;

	//passar pra private depois de testado:
	public float ProAuxUD, ProAuxLR = -100;
	public bool esquina = false;
	//rastro:
	List<Vector2> rastro;
	float rastroTempo = 0;
	int maxRastro = 20;
	bool outOfSight = true;
	private int RangeSize = 10;
	private int SensitiveRange = 8;
	private int baseHP = 5;
	private float preSpeed = 3.5f;
	private float speed = 3.5f;
	private float upSpeed = 2f;
	private float attackPower = 20f; //if (timefactor != 0) attackpower = 70f; 

	private float flDist = 0.3f; //floor distance

	private float ReactionTime = 0.01f;
	
	private Timer attackDelay;
	private Timer turnDelay;


	Ray ray, ray2;
	RaycastHit hit, hit2;


	// Use this for initialization
	void Start () {
		waitFrame = 0;
		tScript=terrain.GetComponent("PolygonGenerator") as PolygonGenerator;
		PlayerMovement = Player.GetComponent("PlayerControl") as PlayerControl;
		StatusPlayer = Player.GetComponent("PlayerStats") as PlayerStats;
		animata = this.GetComponent<Animator>();
		//animata = this.GetComponent("Animator") as Animator;
		rastro = new List<Vector2>();
	//	TracingCurves = PlayerMovement.Final;
		TracePlayer(4);

		HP = baseHP;
		attackDelay = gameObject.AddComponent<Timer>();
		turnDelay = gameObject.AddComponent<Timer>();
		attackDelay.SetValues(0.5f,1);
	}
	
	// Update is called once per frame
	void Update () {


		if (HP <= 0)
			Deactivate();
		
		if (moveIt == true)
		Move ();




		int distanceX = Mathf.Abs(Mathf.FloorToInt(Player.transform.position.x) - Mathf.FloorToInt(transform.position.x));
		int distanceY = Mathf.Abs(Mathf.FloorToInt(Player.transform.position.y) - Mathf.FloorToInt(transform.position.y));
		if (distanceX >= RangeSize || distanceY >= RangeSize)
			Deactivate();

		ReadyToAttack(distanceX,distanceY,1f,1f);
		if (waitFrame == 0)
			waitFrame +=1;
		
	}

	void ReadyToAttack(int distanceX, int distanceY, float minx,float miny)
	{
		Vector2 getPre = attackDelay.GetValues();
		float timeFactor = Time.deltaTime;
		if (getPre.x != 0)
			timeFactor = 1;
		if (gameObject.activeInHierarchy == true)
		{
			if (distanceX < minx + 2 && distanceY < miny + 2 && following == true)
			animata.SetInteger("Kamimasu",1);
			else animata.SetInteger("Kamimasu",0);
		}
		if (distanceX < minx && distanceY < miny)
		{

			if (!attackDelay.working)
			{
			StatusPlayer.TakeDamage(timeFactor * attackPower,0);
				attackDelay.working = true;
			}
		}

	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		for (int i = 0; i<rastro.Count; i++)
		{
			Gizmos.DrawSphere(rastro[i],0.1f);
		}

	}

	public void TracePlayer(int CurvaAux)
	{
		bool pode = true;
		if (outOfSight == false) 
		{
			maxRastro = 1;
			rastro.Clear();
			rastro.Add (new Vector2(Mathf.FloorToInt(Player.transform.position.x)+0.5f,Mathf.FloorToInt(Player.transform.position.y)+0.5f) );
		}
		else maxRastro = 20;

		if (rastro.Count > maxRastro)
		{
			for (int i = 0;i< rastro.Count-maxRastro;i++)
			rastro.RemoveAt(rastro.Count - 1);
		}


		Vector2 preaquecimento = new Vector2(Mathf.FloorToInt(Player.transform.position.x)+0.5f,Mathf.FloorToInt(Player.transform.position.y)+0.5f);
// HABILITAR ISSO DEPOIS se pa
//		if (CurvaAux == 1)
//		{
//			if (preaquecimento.x != rastro[0].x)
//				pode = false;
//			                                }
//		if (CurvaAux == 2)
//		{
//			if (preaquecimento.y != rastro[0].y)
//				pode = false;
//		}

		if (pode == true){
		rastro.Insert(0,preaquecimento);	
		
		}
	}

	public void CurvaTracer()
	{
		int CurvaAux;

		if  (PlayerMovement.Final != TracingCurves)
		{
			//Valor de CurveAux
			//Se X muda e Y nao: CurvaAux = 1
			//Se Y muda e X nao: CurvaAux = 2
			//Se X e Y mudam: CurvaAux = 3
			//Se nenhum muda: CurvaAux = 4

			if  (PlayerMovement.Final.x != TracingCurves.x && PlayerMovement.Final.y == TracingCurves.y)
				CurvaAux = 1;
			else 
				if  (PlayerMovement.Final.x == TracingCurves.x && PlayerMovement.Final.y != TracingCurves.y)
					CurvaAux = 2;
			else 
				if  (PlayerMovement.Final.x != TracingCurves.x && PlayerMovement.Final.y != TracingCurves.y)
					CurvaAux = 3;
			else 
				CurvaAux = 4;

			TracePlayer (CurvaAux);
						TracingCurves = PlayerMovement.Final; 

		}
	}

	public bool EvadeReflex(Vector2 finalTransform)
	{
		Vector3 Origin = new Vector3(transform.position.x,transform.position.y,0.1f);
		Vector3 Direction = finalTransform;
		ray2 = new Ray(Origin,Direction);
		Debug.DrawRay(ray2.origin,ray2.direction);


		if (Physics.Raycast(ray2,out hit2,2f))
		{
			if (hit2.transform.gameObject.name == "Boom")
			{
				//print ("eu to vendo ta");
				Boom baku = hit2.transform.gameObject.GetComponent<Boom>();
				if (baku.pretimer - baku.timer >= ReactionTime)
				return true;
			}
		}
		return false;
	}

	public void getBombInfo(Vector2 infow)
	{
	
	}



	public void RaycastPlayer()

	{
		Vector3 razao = new Vector3(Player.transform.position.x - transform.position.x,Player.transform.position.y - transform.position.y,0f);
		Vector3 origin = new Vector3(transform.position.x,transform.position.y,0.1f);

		ray = new Ray(origin,razao);
		Debug.DrawRay(ray.origin,ray.direction);
		if (Physics.Raycast(ray,out hit))
		{
		   
		
//ESTOY TESTANDO
			if (hit.transform.gameObject.name != "DrillFighter")
				outOfSight = true;
			else
				outOfSight = false;
	
		}

	}

	public void DebuGizmos()
	{
		Debug.DrawLine(transform.position,hit.point,Color.red);


	}


	public void Deactivate()
	{
		if (HP >0)
		{
			try {
				tScript.blocks[Mathf.FloorToInt(transform.position.x),-1*Mathf.FloorToInt(transform.position.y)-1] = 22;
				}
			catch (System.Exception e)
			{}
		}
		following = false;
		outOfSight = true;
		transform.position = new Vector3(-100f,0,0);
		HP = baseHP;
		gameObject.SetActive(false);
	}

	public void turn(Vector2 xiy)
	{
		//print (transform.rotation.eulerAngles);
		Vector3 magickFormula = new Vector3 (0, (Mathf.Abs (xiy.x)*(1 - xiy.x) * 90),((xiy.y *xiy.y*90) - Mathf.Abs (xiy.y)*(1 - xiy.y) * 90));
		float magick = transform.rotation.z;
	//	transform.rotation = Quaternion.Euler(new Vector3 (0, (Mathf.Abs (xiy.x)*(1 - xiy.x) * 90),((xiy.y *xiy.y*90) - Mathf.Abs (xiy.y)*(1 - xiy.y) * 90)));
		transform.rotation = Quaternion.Euler(new Vector3 (0, (Mathf.Abs (xiy.x)*(1 - xiy.x) * 90), magick));


	
		if (transform.rotation.z != magickFormula.z )
		{
	
			transform.eulerAngles += (new Vector3 (0, 0, Mathf.Sign(xiy.y)*10));
		
		}
			
	}

	public void Move (){
		Vector2 initialtransform = new Vector2(faceLR * speed * Time.deltaTime,faceUD * speed * Time.deltaTime);
		float posX = transform.position.x;
		float posY = -1 * transform.position.y;


		if (posY <= 4)
		{ if (faceUD >= 1)
			{faceUD = 0;
			faceLR = -1;}
		}


		if (following == false)
		{

			try
			{


			//MOVIMENTO PASSIVO:

			//Storage the initial value
			int StrLR = faceLR;
			int StrUD = faceUD;

			//se bateu na parede,
			byte verifyPassive1 = tScript.blocks[Mathf.FloorToInt(posX+ flDist* faceLR),Mathf.FloorToInt(posY- flDist* faceUD)];
			if (verifyPassive1 != 0 && verifyPassive1 != 22)
			{
				//Se tiver indo na horizontal, tenta ir na vertical
				if (faceUD == 0)
				{
					if (ComplexMoves == true)
					{

					int rand = Random.Range(0,2);
					if (rand == 0) rand = -1;
					faceUD = rand;
					}
					else faceUD =  faceLR;
					faceLR = 0;
				}
				//Se tiver indo na vertical, tenta ir na horizontal
				else
				{
					faceLR = -faceUD;
					faceUD = 0;
				}
				//se nao der, troca o lado
				verifyPassive1 = tScript.blocks[Mathf.FloorToInt(posX+ flDist* faceLR),Mathf.FloorToInt(posY- flDist* faceUD)];
				if (verifyPassive1 != 0 && verifyPassive1 != 22)
					{
					faceUD = -1* faceUD;
					faceLR = -1* faceLR;
					//se nao der, vai pro lado contrario da primeira verificaçao (usando o storage)
					verifyPassive1 = tScript.blocks[Mathf.FloorToInt(posX+ flDist* faceLR),Mathf.FloorToInt(posY- flDist* faceUD)];
					if (verifyPassive1 != 0 && verifyPassive1 != 22)
						{
						if (faceLR == 0)
						{
							faceLR = -StrLR;
							faceUD = 0;
						}
						if (faceUD == 0)
						{
							faceUD = -StrUD;
							faceLR = 0;
						}

						}
					}
				}
			else 
			{


					//MOVIMENTO PRO-ATIVO:
				if (faceUD != 0)
				{
					byte verifyActive1 = tScript.blocks[Mathf.FloorToInt(posX+ flDist* faceUD -(0.5f * faceLR) ),Mathf.FloorToInt(posY + 0.5f * faceUD)];
					if ((verifyActive1 == 0 || verifyActive1 == 22 ) && esquina == false)
					{
	
							faceLR = faceUD;
							faceUD = 0;
							ProAuxLR = transform.position.x;
							esquina = true;
						
					}

				}
				else
				{
					byte verifyActive2 = tScript.blocks[Mathf.FloorToInt(posX-(0.5f * faceLR)),Mathf.FloorToInt(posY+ flDist*faceLR)];
					if ((verifyActive2 == 0 || verifyActive2 == 22) && esquina == false)
					{



							faceUD = -1 * faceLR;
							faceLR = 0;
							ProAuxUD = transform.position.y;
							esquina = true;

					}
				}

				if (Mathf.Abs(transform.position.y - ProAuxUD) >= 1 && faceUD != 0) esquina = false;
				if (Mathf.Abs (transform.position.x - ProAuxLR) >= 1 && faceLR != 0) esquina = false;

			}
			


			var distance = Vector3.Distance(Player.transform.position, transform.position);
			if (distance <= SensitiveRange)
			{
				RaycastPlayer();
				if (outOfSight == false)
				{
					following = true;
				}
			}


		}
		catch (System.IndexOutOfRangeException e)
		{}

		}
		else if (following == true) 

		{
			RaycastPlayer();
			CurvaTracer();
			DebuGizmos();
			if (outOfSight == true)

					{
				speed = preSpeed + upSpeed;
					
					faceLR = Mathf.RoundToInt(Mathf.Clamp(rastro[rastro.Count -1].x - transform.position.x,-1,1));
					faceUD = Mathf.RoundToInt(Mathf.Clamp(rastro[rastro.Count -1].y - transform.position.y,-1,1));

					//if (Mathf.RoundToInt(rastro[rastro.Count -1].x) == Mathf.RoundToInt(transform.position.x) && Mathf.RoundToInt(rastro[rastro.Count -1].y) == Mathf.RoundToInt(transform.position.y))
					if (faceLR == 0 && faceUD == 0)
					{
						rastro.RemoveAt(rastro.Count-1);
						if (rastro.Count == 0)
							rastro.Add (new Vector2(Mathf.FloorToInt(Player.transform.position.x)+0.5f,Mathf.FloorToInt(Player.transform.position.y)+0.5f) );
						print ("yummy!");
					}


					if (transcend == false)
					{
					byte verifyTranscend1 = tScript.blocks[Mathf.FloorToInt(posX + flDist * faceLR),Mathf.FloorToInt(posY - flDist * faceUD)];
					if (verifyTranscend1 != 0 && verifyTranscend1 != 22)
						{
						byte verifyTranscend2 = tScript.blocks[Mathf.FloorToInt(posX + flDist * faceLR),Mathf.FloorToInt(posY)];
						if (verifyTranscend2 != 0 && verifyTranscend2 != 22)
								faceLR = 0;
						byte verifyTranscend3 = tScript.blocks[Mathf.FloorToInt(posX),Mathf.FloorToInt(posY - flDist * faceUD)];
						if (verifyTranscend3 != 0 && verifyTranscend3 != 22)
								faceUD = 0;

						}

					}

				}
			else
			{
				speed = preSpeed;
				faceLR = Mathf.Clamp(Mathf.RoundToInt(Player.transform.position.x-transform.position.x),-1,1);
				faceUD = -1*Mathf.Clamp(   Mathf.RoundToInt((-1*Player.transform.position.y)-(-1*transform.position.y)), -1,1  );
			}
				
		}

		Vector2 finalTransform = new Vector2(faceLR * speed * Time.deltaTime,faceUD * speed * Time.deltaTime);
		if (!EvadeReflex(finalTransform))
		{
			//if (initialtransform != finalTransform)
			turn(new Vector2(faceLR,faceUD));
		transform.Translate(finalTransform,Space.World);

		}
	}
}
