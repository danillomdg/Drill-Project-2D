using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
[System.Serializable]
public class GameManager : MonoBehaviour {
	private Vector3 position;
	public GameObject player;
	public GameCamera cam;
	public GameObject Megamenu;
	public GameObject Workshopping;
	public GameObject ButtonsController,AxisPad, BoomButton;
	public GameObject EventTextHolder;
	private EventText textoEvento;
	
	private PlayerStats StatusPlayer; 
	private PlayerControl MovimentoPlayer;
	private PlayerPhysics3D FisicaPlayer;
	private DiggingMechanics DrillWorks;
	private RefineryMenuControl Megalomaniaco;	
	private WorkshopMenuControl WorkshopScript;
	
	private bool gameOver;
	private bool restart;
	private int score;
	
	public Image newsbox;
	public Text newstext;

	//MineralsPrice
	public int goldPrice = 400;
	public int silverPrice = 250;
	public int bronzePrice = 150;
	public int ironPrice = 200;
	public int DiamondPrice = 600;

	//ServicePrice
	public float RepairPriceModifier = 1.2f;

	public int menuSwitcherValue;
	
	public List<PlayerEquip> HullList = new List<PlayerEquip>(); 
	public List<PlayerEquip> FuelTankList = new List<PlayerEquip>(); 
	public List<PlayerEquip> DrillList = new List<PlayerEquip>(); 
	public List<PlayerEquip> CargoList = new List<PlayerEquip>(); 
	public List<PlayerEquip> RocketList = new List<PlayerEquip>(); 

	float oi = 0;
	// Use this for initialization

	public int[] SpawnLevel;

	void Start () {
		
		cam.transform.position = new Vector3(player.transform.position.x,player.transform.position.y,cam.transform.position.z);
		spawnPlayer();
		StatusPlayer = player.GetComponent("PlayerStats") as PlayerStats;
		textoEvento = EventTextHolder.GetComponent ("EventText") as EventText;

//		//CreateEquips
		// CreateHull
		HullList.Add (Hull.CreateInstance(101,"Basic Hull",200,0,"this is a basic hull"));
		HullList.Add (Hull.CreateInstance(102,"armored core",1000,0.2f,"Armor ready to combat"));
		HullList.Add (Hull.CreateInstance(103,"impenetrable shield",13000,0.5f,"Seriously, you dont need this."));
		HullList.Add (Hull.CreateInstance(104,"Basic Hull 2",200,0,"this is a basic hull"));
		HullList.Add (Hull.CreateInstance(105,"armored core 2",1000,0.2f,"Armor ready to combat"));
		HullList.Add (Hull.CreateInstance(106,"Cheater",10,10.5f,"Filthy Cheater!"));
		//createFuelTank
		FuelTankList.Add (FuelTank.CreateInstance(201,"Basic Fuel Tank",200,0,"descrissaum"));
		FuelTankList.Add (FuelTank.CreateInstance(202,"Big Fuel",1000,0.2f,"descrissaum"));
		FuelTankList.Add (FuelTank.CreateInstance(203,"extra large size",13000,0.5f,"Seriously, you dont need this."));
		FuelTankList.Add (FuelTank.CreateInstance(204,"Cheater",10,0.995f,"Filthy cheater!"));
		//createDrill
		DrillList.Add (Drill.CreateInstance(301,"Basic Drill",200,1,"descrissaum"));
		DrillList.Add (Drill.CreateInstance(302,"Rock crusher",1000,1.6f,"descrissaum"));
		DrillList.Add (Drill.CreateInstance(303,"Rock Destroyer",13000,2.0f,"Seriously, you dont need this."));
		//CreateCargo
		CargoList.Add (Cargo.CreateInstance(401,"Basic Cargo",200,100,"descrissaum"));
		CargoList.Add (Cargo.CreateInstance(402,"Bigger Cargo",1000,150,"descrissaum"));
		CargoList.Add (Cargo.CreateInstance(403,"Biggest Cargo",13000,200,"Seriously, you dont need this."));
		//CreateRocket
		RocketList.Add (Rocket.CreateInstance(501,"Basic Rocket",200,1,"descrissaum"));
		RocketList.Add (Rocket.CreateInstance(502,"Sky traveler",1000,1.3f,"descrissaum"));
		RocketList.Add (Rocket.CreateInstance(503,"Space Traveler",13000,1.8f,"Seriously, you dont need this."));

		StatusPlayer.InitEquips();

		//DEFINE O PREÇO
		StatusPlayer.ouro.preço = goldPrice;
		StatusPlayer.prata.preço = silverPrice;
		StatusPlayer.bronze.preço = bronzePrice;
		StatusPlayer.ferro.preço = ironPrice;
		StatusPlayer.diamante.preço = DiamondPrice;
		
		//DEFINE O PESO
		StatusPlayer.ouro.espaco = 20;
		StatusPlayer.prata.espaco = 15;
		StatusPlayer.bronze.espaco = 10;
		StatusPlayer.ferro.espaco = 15;
		StatusPlayer.diamante.espaco = 20;
		
		MovimentoPlayer = player.GetComponent ("PlayerControl") as PlayerControl;
		FisicaPlayer = player.GetComponent ("PlayerPhysics3D") as PlayerPhysics3D;
		DrillWorks = player.GetComponent ("DiggingMechanics") as DiggingMechanics;
		Megalomaniaco = Megamenu.GetComponent("RefineryMenuControl") as RefineryMenuControl;
		WorkshopScript = Workshopping.GetComponent ("WorkshopMenuControl") as WorkshopMenuControl;

		//DEFINE A SPAWNZONE DE MONSTERS
		SpawnLevel = new int[10];
		SpawnLevel[0] = 30;
	}
	
	void Update () {
		//controlTime ();
		if (StatusPlayer.fuel <= 0 || StatusPlayer.HP <= 0)
			EndGame();
		
	}


	private void spawnPlayer(){
		position.x = 0;
		position.y = 3;
		position.z = 0;
		
		//	cam.SetTarget((Instantiate(player,position,Quaternion.identity) as GameObject).transform);
		cam.SetTarget(player.transform);
		
	}
	public void EndGame(){
		newsbox.gameObject.SetActive(true);
		newstext.text = ("Gameover! Click to Restart!");
		MovimentoPlayer.movementLocked = true;
		//MovimentoPlayer.moveAmount = new Vector2(0,0);
		DrillWorks.keyX = DrillWorks.keyY = 0;
		menuSwitcherValue = 10;
		if (Input.GetKeyDown (KeyCode.R))
		{
			//Application.LoadLevel (Application.loadedLevel);
			Restarto();
		}
		

	}
	
	public void controlTime(){
		oi += 2 * Time.deltaTime;
		if (Mathf.FloorToInt (oi) == 1) {
			oi = 0;
			print (Mathf.FloorToInt (oi));
		}
		
	}
	
	public void ToggleControls()
	{
		if (MovimentoPlayer.joystickToggle== 2)
		{

			MovimentoPlayer.joystickToggle= 3;
			BoomButton.SetActive (false);
			ButtonsController.SetActive(true);
		}
		else if (MovimentoPlayer.joystickToggle== 3)
		{
			MovimentoPlayer.joystickToggle= 1;
			ButtonsController.SetActive(false);
			AxisPad.SetActive(true);
		}
		else if (MovimentoPlayer.joystickToggle== 1)
		{
			MovimentoPlayer.joystickToggle= 2;
			ButtonsController.SetActive(false);
			AxisPad.SetActive(false);
			BoomButton.SetActive (true);
		}
		
	}
	
	public void MegaMenuSwitcher()
	{
		
		if (menuSwitcherValue == 1) 
		{
			gameObject.SetActive (true);
			OpenRefinery ();
		}
		else if (menuSwitcherValue == 2)
			ElectroHead ();	
		else if (menuSwitcherValue == 3)
			OpenWorkshop();
		else if (menuSwitcherValue == 10)
			Restarto ();
		
		
	}
	
	public void OpenRefinery() 
	{
		Megalomaniaco.OpenRefinery();
	}
	
	public void ElectroHead() 
	{
		if ((StatusPlayer.Money - 50) >= 0) {
			StatusPlayer.Money -= 50;
			StatusPlayer.fuel = 101;
			print ("grana: "+StatusPlayer.Money);
		} else
			print ("sem grana, sem serviço.");
	}
	
	public void OpenWorkshop() 
	{
		WorkshopScript.OpenWorkshop ();
	}
	
	
	public void Restarto(){
		Application.LoadLevel (Application.loadedLevel);
	}

	public void EquipCopy(PlayerEquip copied, PlayerEquip copying)
	{
		copying.ID = copied.ID;
		copying.Name = copied.Name;
		copying.Price = copied.Price;
		copying.StatusBonus = copied.StatusBonus;
	}

	public void ShowDamage(float Value,float holdTime)
	{

		StartCoroutine(textoEvento.ShowDamage("",Value, holdTime));
		textoEvento.SetAllUp();
	}
	public void ShowMining(int Value)
	{
		//3 = ouro; 4 = prata; 5 = bronze; 6 = diamante; 7 = ferro
		string Messeige = "";
		switch (Value)
		{
		case 3:
			Messeige = "gold";
			break;
		case 4:
			Messeige = "silver";
			break;
		case 5:
			Messeige = "bronze";
			break;
		case 6:
			Messeige = "diamond";
			break;
		case 7:
			Messeige = "iron";
			break;
		}
		StartCoroutine(textoEvento.ShowMining(Messeige,Value));
		textoEvento.SetLastMineral (Value);
		textoEvento.SetAllUp();
		
	}
	public void ShowCargoFull()
	{
		StartCoroutine(textoEvento.ShowCargoFull());
	}
	public void ShowLevelUp()
	{
		StartCoroutine(textoEvento.ShowLevelUp());
		textoEvento.SetAllUp();
	}

	public void ShowGotPowerUp(byte x)
	{
		StartCoroutine(textoEvento.ShowPowerUp(x));
		textoEvento.SetAllUp();
	}

}
