using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStats : MonoBehaviour {
	public GameObject Manager;
	private GameManager ManagerGame;
	//public PlayerItens PlayerItens;
	public ItemHandler HandlerItem;

	private SkinnedMeshRenderer[] cubez;
	private List<Color> colorz;

	//PRINCIPAL STATS
	[HideInInspector]
	public int level = 1;
	[HideInInspector]
	public float xp = 0;
	[HideInInspector]
	public float acumulXp = 0;
	[HideInInspector]
	public float TargetXp = 0;

	public float fuel = 100; 
	public float CargoSpace = 100;
	public float HP = 100;
	[HideInInspector]
	public float FuelTimeFactor;
	public float drillPower = 1;
	public float damageReducer = 0;
	public float SpeedModifier = 1;
	private float defaultFuelController = 1.8f;
	[HideInInspector]
	public float StateFuelConditioner = 1;
	 

	//PowerUpsValue
	[HideInInspector]
	public float FuelPU = 20;

	[HideInInspector]
	public float RepairKitPU = 20;

	public Coletavel ouro, prata, bronze, diamante, ferro;
	public float timerAux;

	public Text FuelText;


	public int score;
	public float Money = 50;
	private float dmgEffectTime = 100; //set to 100 to cancel loop
	private float defaultDmgTime = 0.3f;

	//PlayerEquips
	public Hull CurrentHull; 
	public FuelTank CurrentFuelTank;
	public Drill CurrentDrill; 
	public Cargo CurrentCargo; 
	public Rocket CurrentRocket;

	public List<PlayerEquip> CurrentEquips, BuyedEquips;
	public List<PlayerItem> CurrentItens;
	public int SelectedItem;
	public int SelectedItemID;



	//ADDED AFTER CREATION OF SAVELOAD:
	public List<Vector2> PlacesToGo;	

	// Use this for initialization
	void Start () {
		ManagerGame = Manager.GetComponent ("GameManager") as GameManager;
		//PlayerItens = GetComponent<PlayerItens>();
		HandlerItem = GetComponent<ItemHandler>();

		ouro =  new Coletavel();
		prata = new Coletavel();
		bronze = new Coletavel();
		diamante = new Coletavel();
		ferro = new Coletavel();

		fuel = 100;
		HP = 100;
		timerAux = 10;
		colorz = new List<Color>();
		//cor_Inicial = renderer.material.color;
		cubez = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

		for (int i = 0; i < cubez.Length; i++)
		{

			Material[] matz =  cubez[i].materials;
			foreach (Material mat in matz)
				colorz.Add (mat.color);	
		}

		TargetXp = (level * level) + (level * 200);
		Vector2 initialPosition = new Vector2(9.5f,1f);
		PlacesToGo.Add(initialPosition);
	}

	public void ChangePosition(Vector3 position)
	{
		gameObject.transform.position = position;
		ManagerGame.cam.transform.position = new Vector3(position.x,position.y,ManagerGame.cam.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (dmgEffectTime < 100)
		{
			if (dmgEffectTime > 0)
			dmgEffectTime -= Time.deltaTime;
			else ControlColor();

		}

		if (fuel > 0)
			fuel -= defaultFuelController  * Time.deltaTime * (1-FuelTimeFactor) * StateFuelConditioner;
				else {
						fuel = 0;

			Vector3 MsgLoc = new Vector3(0.5f,0.75f,0);
			Vector3 Msg2Loc = new Vector3(0.5f,0.7f,0);
									
						
			}
		timerAux -= Time.deltaTime;

	 

		FuelText.text = Mathf.FloorToInt(fuel).ToString()+"%";
	}

	public void CalculateLevel()
	{
		float SuperiorXp = (level * level) + (level * 200) ; 
		if ( xp >= SuperiorXp )
		{
			level+= 1;
			xp = 0;
			TargetXp = (level * level) + (level * 200);
			ManagerGame.ShowLevelUp();
		}
	}

	public void GetXp(float XpGot)
	{
		xp +=XpGot;
		acumulXp +=XpGot;
		CalculateLevel();
	}

	public void EmptyCargo()
	{
		ouro.quantidade = prata.quantidade = bronze.quantidade = diamante.quantidade = ferro.quantidade = 0;
		CargoSpace = 100;
	}

	public void ControlColor()
	{
		if (dmgEffectTime > 0)
		{
			for (int i = 0; i < cubez.Length; i++)
			{
				Material[] matz =  cubez[i].materials;
				foreach (Material mat in matz)
					mat.color = Color.red;
			}
		}
		else
		{
			int auxcount = 0;
			for (int i = 0; i < cubez.Length; i++)
			{
				Material[] matz =  cubez[i].materials;
				foreach (Material mat in matz)
				{
					mat.color = colorz[auxcount];
					auxcount++;
				}
			}
			dmgEffectTime = 100;

		}
	}

	public void TakeDamage (float value, float holdTime) 
	{

		dmgEffectTime = defaultDmgTime;
		ControlColor();	
		float damageTaken = value * (1 - damageReducer);
		HP-= damageTaken;
		ManagerGame.ShowDamage (damageTaken,holdTime);
		//renderer.material.color = Color.red;
	}

	public void gotPowerUp(byte x)
	{
		if (x == 51)
		{
			HP += RepairKitPU;
			if (HP> 100)
				HP = 100;

		}
		else if (x == 52)
		{
			fuel += FuelPU;
			if (fuel > 100)
				fuel = 100;
			print ("refueled");
		}

		ManagerGame.ShowGotPowerUp(x);

	}

	public void informGotMineral(int value, bool Condition)
	{
		if (Condition == true)
		ManagerGame.ShowMining(value);
		 else
			ManagerGame.ShowCargoFull();

	}

	public void InitEquips()
	{
		CurrentHull = ManagerGame.HullList[0] as Hull;
		CurrentFuelTank = ManagerGame.FuelTankList[0] as FuelTank;
		CurrentDrill = ManagerGame.DrillList[0] as Drill;
		CurrentCargo = ManagerGame.CargoList[0] as Cargo;
		CurrentRocket = ManagerGame.RocketList[0] as Rocket;
		
		CurrentEquips.Add(CurrentHull);
		CurrentEquips.Add(CurrentFuelTank);
		CurrentEquips.Add(CurrentDrill);
		CurrentEquips.Add(CurrentCargo);
		CurrentEquips.Add(CurrentRocket);
		
		BuyedEquips.Add(CurrentHull);
		BuyedEquips.Add(CurrentFuelTank);
		BuyedEquips.Add(CurrentDrill);
		BuyedEquips.Add(CurrentCargo);
		BuyedEquips.Add(CurrentRocket);
		
	}

	public void InitItens()
	{
		CurrentItens.Add (ManagerGame.ItensList[0] as PlayerItem);
		SelectedItem = 0;
		SelectedItemID = CurrentItens[SelectedItem].ID;
		HandlerItem.ItemQuantity();
	}

	public void ToggleIten()
	{
		int counto = CurrentItens.Count - 1;
		SelectedItem +=1;
		if (SelectedItem > counto)
		SelectedItem = 0;

	//	return CurrentItens[SelectedItem].ID;
		SelectedItemID = CurrentItens[SelectedItem].ID;
		print ("SelectedID: "+SelectedItemID);
		HandlerItem.ItemQuantity();
		HandlerItem.changeIcon(SelectedItemID);
	}


	public void atualizeStats()
	{
		damageReducer = CurrentHull.StatusBonus;
		print ("damageReducer "+CurrentHull.StatusBonus);
		FuelTimeFactor = CurrentFuelTank.StatusBonus;
		print ("filthyTank "+CurrentFuelTank.StatusBonus+" name: "+CurrentFuelTank.Name);
		drillPower = CurrentDrill.StatusBonus;
		print ("drillPower "+CurrentDrill.StatusBonus+" name: "+CurrentDrill.Name);
		CargoSpace = CurrentCargo.StatusBonus;
		print ("CargoSpace "+CurrentCargo.StatusBonus);
		SpeedModifier = CurrentRocket.StatusBonus;
		print ("SpeedModifier "+CurrentRocket.StatusBonus);
	}


}
