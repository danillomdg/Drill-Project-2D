using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
[System.Serializable]
public class PlayerStats : MonoBehaviour {
	public GameObject Manager;
	private GameManager ManagerGame;

	private SkinnedMeshRenderer[] cubez;
	private List<Color> colorz;

	//PRINCIPAL STATS
	public float fuel = 100; 
	public float CargoSpace = 100;
	public float HP = 100;
	[HideInInspector]
	public float FuelTimeFactor;
	public float drillPower = 1;
	public float damageReducer = 0;
	public float SpeedModifier = 1;
	private float defaultFuelController = 1.8f;


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

	// Use this for initialization
	void Start () {
		ManagerGame = Manager.GetComponent ("GameManager") as GameManager;


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
						fuel -= defaultFuelController  * Time.deltaTime * (1-FuelTimeFactor);
				else {
						fuel = 0;

			Vector3 MsgLoc = new Vector3(0.5f,0.75f,0);
			Vector3 Msg2Loc = new Vector3(0.5f,0.7f,0);
									
						
			}
		timerAux -= Time.deltaTime;

	 

		FuelText.text = Mathf.FloorToInt(fuel).ToString()+"%";
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
