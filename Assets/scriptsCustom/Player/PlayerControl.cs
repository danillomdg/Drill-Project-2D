using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnitySampleAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerPhysics3D))]
public class PlayerControl : MonoBehaviour {

		//Player Handling

	public GameObject terrain;
	public GameCamera cam;
	[HideInInspector]
	public int joystickToggle = 0;
	[HideInInspector]
	public float PosState = 1;
	[HideInInspector]
	public int offsetPos = 0;
	public bool digdown = false;
	public bool digLR = false;
	public float isDigging,DDownValue = 0;
	public bool movementLocked;


	public float gravity;
	public float speed;
	public float defaultspeed;
	public float digspeed;


	public float acceleration;
	private float FDesacceleration = 10f;
	public float jumpHeight;

	private float rotationSpeed = 15;  // divisores de 180

	private float AllertTimer = 0;
	private float AuxPGTimer = 0;

	private Vector2 currentspeed;
	public Vector2 targetspeed;
	private Vector2 moveAmount;

	private PlayerPhysics3D PlayerPhysics3D;
	private PlayerStats StatusPlayer;
	private Animator animator;
	private DiggingMechanics DiggingMechanics;
	private PolygonGenerator Paragon;
	private PlayerItens PlayerItens;

	private Vector2 TouchPositionAux;
	private Vector2 TouchPositionAux2;
	private Vector3 DeltaPosition;
	public Vector2 TouchPosition;
	public Vector2 Final;
	public Vector2 previous;
	public int[] myfingerID;

	//rastro:
	List<Vector2> rastro;
	float rastroTempo = 0;


	// Use this for initialization
	void Start () {
		DeltaPosition = new Vector2(0,0);
		PlayerPhysics3D = GetComponent<PlayerPhysics3D>();
		StatusPlayer = GetComponent<PlayerStats>();
		Paragon = terrain.GetComponent("PolygonGenerator") as PolygonGenerator;  
		animator = GetComponent<Animator>();
		DiggingMechanics = GetComponent<DiggingMechanics>();
		PlayerItens = GetComponent<PlayerItens>();
		movementLocked = false;
		PosState = 0;
		rastro = new List<Vector2>();

	}



	// Update is called once per frame
	void Update () {

		movement();
		actions();
	}

	void actions()
	{
		if (Input.GetKeyDown("space"))
			PlayerItens.bombie.DropIt();

	}
	

	public void movement ()
	{
		
		rastroTempo += Time.deltaTime;
		if (rastroTempo >= 2)
		{
			if (rastro.Count >= 20)
				rastro.RemoveAt(rastro.Count - 1);
			//rastro.Insert(0,)
		}

		
		//	print ("joyAxis - x:  "+	CrossPlatformInputManager.GetAxisRaw("Horizontal")+ "    y:  "+CrossPlatformInputManager.GetAxisRaw("Vertical"));
		
		
		// If digging, change speed 
		if (PosState != 0) speed = digspeed * StatusPlayer.drillPower;
		else speed = defaultspeed * StatusPlayer.SpeedModifier;;
		
		if (PlayerPhysics3D.movementStoppedX){
			targetspeed.x =  currentspeed.x  = 0;
			
		}
		if (PlayerPhysics3D.movementStoppedY){
			targetspeed.y =  currentspeed.y = 0;
			
			
		}
		animator.SetFloat("digging",PosState);
		
		if (joystickToggle == 0)
		{
			if (TouchPosition.y == 0 || PlayerPhysics3D.grounded == false)
				TouchPosition.x = Mathf.Sign ( CrossPlatformInputManager.GetAxisRaw("Horizontal"))* Mathf.FloorToInt(Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("Horizontal"))*1.2f);
			else  TouchPosition.x = 0;
			
			if (TouchPosition.x == 0 || PlayerPhysics3D.grounded == false)
				TouchPosition.y = Mathf.Sign (CrossPlatformInputManager.GetAxisRaw("Vertical"))* Mathf.FloorToInt(Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("Vertical"))*1.2f);
			else  TouchPosition.y = 0;
			
			if (PlayerPhysics3D.grounded || digdown)
			{
				TouchPosition.x = TouchPosition.x * (1-Mathf.Abs(TouchPosition.y));
				TouchPosition.y = TouchPosition.y * (1-Mathf.Abs(TouchPosition.x));
			}
			
			print ("issdigging: "+digdown+"x : "+TouchPosition.x+ "  y: "+TouchPosition.y);
		}
		else if (joystickToggle == 1)
		{
			if (Input.touchCount > 0) {
				bool itson = false;
				bool handsdown = false;
				for (int i = 0; i <= Input.touchCount-1; i++)
				{
					string Evento = "hola";
					if (EventSystem.current.IsPointerOverGameObject (Input.GetTouch(i).fingerId) || EventSystem.current.IsPointerOverGameObject (-1))
					{
						print(EventSystem.current.currentSelectedGameObject);
						if (EventSystem.current.currentSelectedGameObject != null)
						Evento = EventSystem.current.currentSelectedGameObject.name;
						else Evento = "null";
					}
					if ((Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved ) && (Evento == "hola" || Evento == "AxisPadController"))
						TouchPositionAux = Input.GetTouch (i).position;

					
					if (Input.GetTouch(i).phase != TouchPhase.Ended) 
						itson = true;
					if (Input.GetTouch(i).phase == TouchPhase.Ended)
						handsdown = true;
				}
				
				if (handsdown == true && itson == false){
					TouchPositionAux = new Vector2(0,0);
					itson = false;
					handsdown = false;
				}
			}
			
			float tangerina = Mathf.Atan2((TouchPositionAux.y - (Screen.height*0.25f)),(TouchPositionAux.x - (Screen.width*0.25f)));
			
			TouchPosition.x = Mathf.Clamp01(TouchPositionAux.x) * Mathf.RoundToInt(Mathf.Cos(tangerina));
			TouchPosition.y = Mathf.Clamp01 (TouchPositionAux.y) * Mathf.RoundToInt(Mathf.Sin (tangerina));
			
			print ("x : "+Mathf.RoundToInt(Mathf.Cos(tangerina))+ "  y: "+Mathf.RoundToInt(Mathf.Sin(tangerina)));
		}
		
		else if (joystickToggle == 2)
		{
			if (Input.touchCount > 0) {
				
				if (Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					
					float x =  Input.GetTouch (0).deltaPosition.x;
					float y =  Input.GetTouch (0).deltaPosition.y;
					
					
					print ("again  x: "+x+" y: "+y);
					
					if (Mathf.Abs(x) > 0.8f && Mathf.Abs(y) > 0.8f)
					{
						if (Mathf.Abs(x)>Mathf.Abs(y)){
							TouchPosition.x = 1 * Mathf.Sign (x);
							if (TouchPosition.y != 1)
								TouchPosition.y = 0;
						}
						else if (Mathf.Abs(y)>Mathf.Abs(x)){
							TouchPosition.y = 1 * Mathf.Sign (y);
							
							TouchPosition.x = 0;
						}
						else {
							TouchPosition.x = Mathf.Clamp(Mathf.FloorToInt(x/y),-1,1);
							TouchPosition.y = Mathf.Clamp(Mathf.FloorToInt(y/x),-1,1);
						}
					}
					else TouchPositionAux = new Vector2(0,0);
					
					
				}
				else 
					if (Input.GetTouch(0).phase == TouchPhase.Ended)
						TouchPosition = new Vector2(0,0);
			}
			
		}
		
		//APLICANDO CONTROLES XY
		
		
		
		Final.x = Input.GetAxisRaw ("Horizontal") + TouchPosition.x;
		
		Final.y = Input.GetAxisRaw("Vertical")+TouchPosition.y;
		
		
		//		
		if (digdown == true)Final.x = 0;
		if (digLR == true)  Final.y = 0;
		
		
		
		
		
		
		
		//APLICANDO CONTROLE NA POSIÇAO X:
		
		if (movementLocked == false)
		{	
			targetspeed.x = (Final.x) * speed;
		} 

		else {
			targetspeed.x = 0;
		}

		float AccSwitcher;

		if (PlayerPhysics3D.grounded == false && targetspeed.x <= 0)
		{
			AccSwitcher = FDesacceleration;

		}
		else AccSwitcher = acceleration;
		//currentspeed.x = IncrementTowards(currentspeed.x,targetspeed.x,acceleration *5);
		currentspeed.x = IncrementTowards(currentspeed.x,targetspeed.x,AccSwitcher *5);

		
		float MoveAux = transform.position.x + currentspeed.x * Time.deltaTime;

		//CRIADO PRO BIXO NAO CAIR NO LIMBO

		if (MoveAux < 2f || MoveAux > Paragon.SizeX-2)
		{
			currentspeed.x = 0;
			moveAmount.x = 0;
		}
		else
			moveAmount.x = currentspeed.x;
		
		
		
		
		
		//APLICANDO CONTROLE NA POSIÇAO Y:
		
		if (PlayerPhysics3D.grounded) {
			moveAmount.y = 0;
			
		}
		
		
		if (PosState == 2)                                          
			moveAmount.y = (IncrementTowards(Final.y * speed,targetspeed.x,acceleration));
		
		
		
		else if  (Final.y==0)
		{
			if (PlayerPhysics3D.grounded)
			{
				currentspeed.y = 0;
			}
			else
				targetspeed.y =  -1 * speed;
			currentspeed.y = IncrementTowards(currentspeed.y,targetspeed.y - 3,acceleration);
			moveAmount.y = currentspeed.y;
		}
		else if (movementLocked == false)						    
		{
			targetspeed.y = Final.y * speed;
			currentspeed.y = IncrementTowards(currentspeed.y,targetspeed.y ,acceleration * 2);
			moveAmount.y = currentspeed.y;
			//	print("porque nao? "+targetspeed.y);
		}
		
		
		//Don't move it motherfucker!
		if (DiggingMechanics.lockAxis == true) {
			if ((DDownValue == 0)|| PosState == 1){
				
				movementLocked = false;
				
			}
		}
		
		// Move it motherfucker!
		if (movementLocked == false) {
			PlayerPhysics3D.Move(moveAmount * Time.deltaTime);
			AllertTimer = 0;
		}
		if (movementLocked == true) {
			AllertTimer +=1 * Time.deltaTime;
			
			if (AllertTimer >=1.5f)
			{
				movementLocked = false;
				AllertTimer = 0;
			}
		}
		
		
		//Face Direction
		if (digdown == false){
			
			//Rotate the motherfucker, based on position and speed n shit
			if (targetspeed.x == 0){
				if ((offsetPos == 1 && transform.eulerAngles.y > 0.01)|| (offsetPos == -1 && transform.eulerAngles.y < 180+rotationSpeed))
					transform.eulerAngles -= Vector3.up * rotationSpeed * offsetPos;
				
				if ((offsetPos == 1 && transform.eulerAngles.y < 1)|| (offsetPos == -1 && transform.eulerAngles.y > 179-rotationSpeed))
					transform.eulerAngles = Vector3.up * 180 * ((-1+offsetPos)/-2) ; 
			}
			else	if (movementLocked == false)
				offsetPos = System.Math.Sign(targetspeed.x);
			
			if (offsetPos == -1) if (transform.eulerAngles.y < 180) transform.eulerAngles += Vector3.up * rotationSpeed;
			if (offsetPos == 1)  if (transform.eulerAngles.y > 0.01) transform.eulerAngles -= Vector3.up * rotationSpeed;
		}
		else {
			if (transform.eulerAngles.y != 90){
				if ((offsetPos == 1 && transform.eulerAngles.y < 90)|| (offsetPos == -1 && transform.eulerAngles.y > 90+rotationSpeed))
					transform.eulerAngles += Vector3.up * rotationSpeed * offsetPos;			
			}
		}
		//	print(transform.position.x);
		
		
		
		
		
		if (DeltaPosition != transform.position && isDigging == 0)
		{
			AuxPGTimer+= 5 * Time.deltaTime;
			if (Mathf.FloorToInt (AuxPGTimer) == 1) {
				AuxPGTimer = 0;
				
				Paragon.update2 = new Vector2(Mathf.RoundToInt(cam.transform.position.x),Mathf.RoundToInt(cam.transform.position.y-Paragon.transform.position.y));
			}
		}
		DeltaPosition = transform.position;

	}

	//funçao que cria a aceleraçao do caboclo
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

	public void DropItHard()
	{
		print ("Trigged");
		PlayerItens.bombie.DropIt();
	}



}
