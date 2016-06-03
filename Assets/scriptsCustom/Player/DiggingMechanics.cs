using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerControl))]
public class DiggingMechanics : MonoBehaviour {
	public GameObject terrain;
	public GameCamera cam;
	private PolygonGenerator tScript;
	private PlayerControl PlayerMovement;
	private PlayerPhysics3D PlayerPhysics;
	private PlayerStats PlayerStats;
	private float AuxSizeY, AuxSizeX ;
	public float timer = 2;

	[HideInInspector]
	public float keyX, keyY = 0;

	private float auxKeyX,auxKeyY=0;
	private int posInCollision= 0;
	public bool lockAxis = false;
	int endireitando = 0;

	private float fuel = 100;


		// Use this for initialization
	void Start () {
		tScript=terrain.GetComponent("PolygonGenerator") as PolygonGenerator;  
		PlayerMovement = GetComponent<PlayerControl>();
		PlayerPhysics = GetComponent<PlayerPhysics3D>();
		PlayerStats = GetComponent<PlayerStats>();
		AuxSizeY = 1.5f - transform.localScale.y; 
		//		AuxSizeX = (0.5f - transform.localScale.x)/0.25f; 
		PlayerMovement.isDigging = 0;
	}


	void Update () {

		//DEFINE DIREÇAO
		
		if (PlayerMovement.movementLocked == false && endireitando == 0 ){
			keyX = PlayerMovement.Final.x;
			keyY = PlayerMovement.Final .y;
		}

		// QUANDO FOR CAVAR PARA BAIXO, CAVE NO LUGAR CERTO (faz o personagem cavar sempre no meio)

		if (lockAxis == true && keyY == -1 && keyX == 0)
		{
				 


			float x1 = transform.position.x ;
			float x2 = x1 - Mathf.RoundToInt (x1);
					
					
					posInCollision = System.Math.Sign(x2);
			 
					if (System.Math.Abs(x1 - (Mathf.FloorToInt(x1)+0.5f)) > 0.3f)

					{
					auxKeyX = Mathf.RoundToInt(x1);
						endireitando = 1;
						keyY = 0;
			}
		}	


		if (endireitando == 1)
		{	
			PlayerMovement.movementLocked = true;
			if ((transform.position.x < auxKeyX + 0.5f && posInCollision ==1 )||
			    (transform.position.x > auxKeyX - 0.5f && posInCollision ==-1 ) )
			{

				 
				PlayerMovement.offsetPos = posInCollision;
				PlayerMovement.movementLocked = true;
				
				Vector2 moveDig = new Vector2(PlayerMovement.digspeed*Time.deltaTime*posInCollision,0);		
				transform.Translate(moveDig, Space.World);
				
			
			} else {
				endireitando = 0;
				keyY = -1;		
			}
			
		}
		

		//REFRESH NOS CONTROLADORES DE "ISDIGGING"		
	
//		
//		if (PlayerMovement.isDigging >= 2) PlayerMovement.isDigging = 0;
//		if (PlayerMovement.DDownValue >= 2) PlayerMovement.DDownValue = 0;

	

//VERIFICA SE E PARA REMOVER OS BLOCOS
		 
// OBS: FALTA  COLOCAR UM VERIFICADOR DE MOVIMENTO DO PLAYER

		RemoveBlock (keyX, -1 + keyY, keyX, keyY, 0);


		if (keyY == 0 && keyX == 0 ) {
			PlayerMovement.PosState = 0;
			PlayerMovement.digdown = false;
			PlayerMovement.digLR = false;
		}
		
		
		// FIM DA FUNÇAO UPDATE

		if (keyY != -1 || (keyY != -1 && !PlayerPhysics.grounded) ) PlayerMovement.digdown = false;
		if (keyX == 0 || !PlayerPhysics.grounded ) PlayerMovement.digLR = false;
	}



	void RemoveBlock(float offsetX, float offsetY, float keyX, float keyY, int Xinicial)
	{

//
//		if (keyY == -1 && keyX == 0 && PlayerPhysics.grounded ) PlayerMovement.digdown = true;
//		else PlayerMovement.digdown = false;
//		if (keyX != 0 && keyY == 0 && PlayerPhysics.grounded ) PlayerMovement.digLR = true;
//		else PlayerMovement.digLR = false;


		float ajudinha = 0;

		if (lockAxis == true){
						 
			ajudinha = ( 1.5f); //era 1.5f
			
			if	( keyX == -1)ajudinha = ( 0.1f); //era 0.5f
			if	( keyX == 1) ajudinha = -1.0f; //era -1.5f
			if	( keyX == 0) ajudinha = - 0.5f; //era -0.5f
	

		}

			
		int x =Mathf.RoundToInt(transform.position.x+offsetX+ajudinha);


		float ajudinhay = 0;
		if (keyY == -1 && keyX == 0)
			ajudinhay = 0.7f; //era 1f - passou pra 0.7f
		
		int y=Mathf.RoundToInt(transform.position.y+AuxSizeY+offsetY+ajudinhay);

		y = -y;



		if(x<tScript.blocks.GetLength(0) && y<tScript.blocks.GetLength(1) && x>=0 && y>=0 && keyY != 1 && (lockAxis ==false || keyX * keyY ==0))
		{

			if ((PlayerPhysics.grounded ) || (PlayerMovement.PosState ==2 && PlayerPhysics.grounded == false) )
			{
				if( tScript.blocks[x,y]!=0 && tScript.blocks[x,y] != 20 && tScript.blocks[x,y] != 21)
				//if( tScript.blocks[x,y]!=0  )
				{


					if (keyY == -1 && keyX == 0 && PlayerPhysics.grounded ) PlayerMovement.digdown = true;
					else PlayerMovement.digdown = false;
					if (keyX != 0 && keyY == 0 && PlayerPhysics.grounded ) PlayerMovement.digLR = true;
					else PlayerMovement.digLR = false;


					timer = 0.43f;
					
					// DEFINE O "PosState" 
					if (keyX != 0){
						if (keyY == 0){
							PlayerMovement.PosState = 1;
							PlayerMovement.digLR = true;
						}
						else if (keyY < 0){
							if (lockAxis == false) {
								PlayerMovement.PosState = 2; 
								PlayerMovement.digLR = true;
							}

						}
						else PlayerMovement.PosState = 0;
						PlayerMovement.digdown = false;
					}
					if (keyX== 0) {
						if (keyY < 0){
							PlayerMovement.digdown = true;
							PlayerMovement.PosState = 2; 
						
						}
						else {PlayerMovement.PosState = 0;
							PlayerMovement.digdown = false;	
						}
							PlayerMovement.digLR = false;
					}
					

					
//REMOVE DE FATO OS BUROCKUS (obs: precisa adicionar funçao "bater de cara na parede" quando nao tem cargo suficiente,
							//que tambem sera usada na hora de bater em pedras que precisam de um drill melhor pra furar


					if (tScript.blocks[x,y] > 2 && tScript.blocks[x,y] <21)
						ColetaMineral(x,y);
					else if (tScript.blocks[x,y] >= 50 && tScript.blocks[x,y] <= 52 )
						PlayerStats.gotPowerUp(tScript.blocks[x,y]);
					// else if 
					//if (PlayerMovement.digdown == true)
					//StartCoroutine(PlayerMovement.AddDigDown(0,transform.position.x));

					tScript.blocks[x,y]=0;



					if (PlayerMovement.DDownValue == 0){
						if (endireitando == 0)
							auxKeyX = x;
						auxKeyY = y;
					}

					tScript.update2 = new Vector2(Mathf.RoundToInt(cam.transform.position.x),Mathf.RoundToInt(cam.transform.position.y-tScript.transform.position.y));
				
				}	
				else {


					if(tScript.blocks[x,y]==0 || (tScript.blocks[x,y+1]==0 && keyY == -1)|| (tScript.blocks[x+PlayerMovement.offsetPos,y]==0 && keyX != 0) )

					{

					if (timer > 0)
						timer -= Time.deltaTime;
					if (timer <= 0){
						PlayerMovement.PosState = 0;
						PlayerMovement.digdown = false;
						PlayerMovement.digLR = false;
					}


					}
					else{

						PlayerMovement.PosState = 0;
						PlayerMovement.digdown = false;
						PlayerMovement.digLR = false;
					}

				}

			}
			else if (PlayerPhysics.grounded == false)
				PlayerMovement.PosState = 0;

		}
		else PlayerMovement.PosState = 0;

	}




	void ColetaMineral (int x, int y)
	{
//		print ("CARGOW "+PlayerStats.CargoSpace);
		byte bucetinha = tScript.blocks[x,y];

		bool coletado = false;

		if (bucetinha == 3){
			if (PlayerStats.CargoSpace - PlayerStats.ouro.espaco >=0) {
			PlayerStats.ouro.quantidade += 1;
//			print ("ouro: "+PlayerStats.ouro.quantidade);
				PlayerStats.CargoSpace -= PlayerStats.ouro.espaco;
			coletado = true;
			}
		}

		else 	if (bucetinha == 4){
			if (PlayerStats.CargoSpace - PlayerStats.prata.espaco >=0) {
			PlayerStats.prata.quantidade += 1;
//			print ("prata: "+PlayerStats.prata.quantidade);
				PlayerStats.CargoSpace -= PlayerStats.prata.espaco;
			coletado = true;
			}
		}
		else 	if (bucetinha == 5){
			if (PlayerStats.CargoSpace - PlayerStats.bronze.espaco >=0) {
			PlayerStats.bronze.quantidade += 1;
//			print ("bronze: "+PlayerStats.bronze.quantidade);
				PlayerStats.CargoSpace -= PlayerStats.bronze.espaco;
			coletado = true;
			}
		}
		else 	if (bucetinha == 6){
			if (PlayerStats.CargoSpace - PlayerStats.diamante.espaco >=0) {
			PlayerStats.diamante.quantidade += 1;
//			print ("diamante: "+PlayerStats.diamante.quantidade);
				PlayerStats.CargoSpace -= PlayerStats.diamante.espaco;
			coletado = true;
			}
		}
		else 	if (bucetinha == 7){
			if (PlayerStats.CargoSpace - PlayerStats.ferro.espaco >=0) {
			PlayerStats.ferro.quantidade += 1;
//			print ("ferro: "+PlayerStats.ferro.quantidade);
				PlayerStats.CargoSpace -= PlayerStats.ferro.espaco;
				coletado = true;
			}
		}

	if (coletado == false)
			print("Deuruim");
		PlayerStats.informGotMineral (bucetinha,coletado);

	}
	
}