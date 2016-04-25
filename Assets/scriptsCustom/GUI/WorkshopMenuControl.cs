using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorkshopMenuControl : MonoBehaviour {


	public GameObject player;
	private PlayerStats StatusPlayer;
	public GameObject Manager;
	private GameManager ManagerGame;
	public Canvas rosto;
	public RectTransform myPanel;

	public GameObject ConfirmationWindow;
	public Text ConfirmationText;
	//private Text textaum;
	private float tradeMoney;
	private float valorAPagar = 0;
	public int switcherValue;
	public int ElementSession = 0; //0 = repair 1 = hull 2 = fuel 3 = drill 4 = cargo 5 = rocket 6 = itens

	public Text DescriptionText, SlotText, ButtonSlotText;

	public GameObject SlotElement;
	List<GameObject> SlotList = new List<GameObject>();
	// Use this for initialization
	void Start () {
		switcherValue = 0;
		StatusPlayer = player.GetComponent ("PlayerStats") as PlayerStats;
		ManagerGame = Manager.GetComponent ("GameManager") as GameManager;

		//OpenRefinery ();
		//gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OpenWorkshop() 
	{
		gameObject.SetActive(true);
	}
	public void ClickRepair()
	{
		clearSlot();
		ElementSession = 0;
		valorAPagar = (100 - StatusPlayer.HP) * ManagerGame.RepairPriceModifier;
		ConfirmationText.text = "You want to repair damage for $"+valorAPagar+"?";
		ConfirmationWindow.SetActive (true);
	}
	public void ActualRepair()
	{
	
		if (StatusPlayer.Money >= valorAPagar )
		{
		StatusPlayer.HP = 100;
		StatusPlayer.Money -= valorAPagar;
		valorAPagar = 0;
		}
		ConfirmationWindow.SetActive (false);
	}

	public void OpenHulls()
	{

	if (ElementSession != 1 )
	{
		clearSlot();
		ElementSession = 1;
			OpenSection(ManagerGame.HullList);
	}
	
	}

	public void OpenFuelTank()
	{
		print ("foi");
		if (ElementSession != 2 )
		{
			clearSlot();
			ElementSession = 2;
			OpenSection(ManagerGame.FuelTankList);
		}
		
	}

	public void OpenDrills()
	{

		if (ElementSession != 3 )
		{
			clearSlot();
			ElementSession = 3;
			OpenSection(ManagerGame.DrillList);
		}
	}
	public void OpenCargo()
	{

		if (ElementSession != 4 )
		{
			clearSlot();
			ElementSession = 4;
			OpenSection(ManagerGame.CargoList);
		}
	}
	public void OpenRocketss()
	{

		if (ElementSession != 5 )
		{
			clearSlot();
			ElementSession = 5;
			OpenSection(ManagerGame.RocketList);
		}
	}
	public void OpenItens()
	{

		if (ElementSession != 5 )
		{
			clearSlot();
			ElementSession = 5;
			OpenItens(ManagerGame.ItensList);

		}
	}

	public void OpenItens(List<PlayerItem> Risto)
	{
		for (int i = 0; i<Risto.Count;i++)
		{
			Vector3 vectoru = SlotElement.transform.position;
			GameObject TempSlot = Instantiate(SlotElement,vectoru , Quaternion.identity) as GameObject;
			Image mag = TempSlot.GetComponentInChildren<Image>();
			float slider = i * mag.rectTransform.rect.height * mag.rectTransform.localScale.y * -1;		
			SlotList.Add(TempSlot);
			SlotList[i].transform.SetParent(myPanel,false);

			Text textaum;
			Button battaum;
			Button butaum;

			int destiny = 1; // 1 = comprar; 2 = nao pode comprar; 3 = equipando 4 = equipar	
			if (StatusPlayer.Money < Risto[i].Price)
				destiny = 2;
			int CurrentI = i;
			
			textaum = SlotList[i].GetComponentInChildren<Text>();
			battaum = SlotList[i].GetComponentsInChildren<Button>()[0];
			battaum.onClick.AddListener (delegate { ShowItemDescription(Risto[CurrentI],battaum); });
			textaum.text = Risto[i].Name;
			
			
			textaum = SlotList[i].GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>();
			butaum = SlotList[i].GetComponentsInChildren<Button>()[1];
			
			
			if (destiny == 1)
			{
				textaum.text = "$"+Risto[i].Price;
				butaum.image.color = Color.green;
				butaum.onClick.AddListener (delegate { buyItem(Risto[CurrentI],Risto); });
			}
			else if (destiny == 2)
			{
				textaum.text = "$"+Risto[i].Price;
				butaum.image.color = Color.red;
			}

		}
					
		}
		
	public void OpenSection(List<PlayerEquip> Risto)
	{
		for (int i = 0; i<Risto.Count;i++)
		{

			//float slider = i * -16.3f * 2.75f;

			Vector3 vectoru = SlotElement.transform.position;
			GameObject TempSlot = Instantiate(SlotElement,vectoru , Quaternion.identity) as GameObject;

			Image mag = TempSlot.GetComponentInChildren<Image>();
			float slider = i * mag.rectTransform.rect.height * mag.rectTransform.localScale.y * -1;
			print ("HeighSuraida "+slider);

			SlotList.Add(TempSlot);
			//SlotList[i].transform.SetParent(this.transform,false);
			SlotList[i].transform.SetParent(myPanel,false);
		//	SlotList[i].transform.position += new Vector3(0,slider,0);



			Text textaum;
			Button battaum;
			Button butaum;
			int destiny = 1; // 1 = comprar; 2 = nao pode comprar; 3 = equipando 4 = equipar

			if (StatusPlayer.Money < Risto[i].Price)
				destiny = 2;

			//print (StatusPlayer.CurrentEquips[0].ID);
			for (int j = 0; j < StatusPlayer.CurrentEquips.Count ; j++)
			{
		    	if (Risto[i].ID == StatusPlayer.CurrentEquips[j].ID)
				destiny = 3;
			}
			if (StatusPlayer.BuyedEquips.Count != 0)
			{
				for (int  k= 0; k < StatusPlayer.BuyedEquips.Count ; k++)
				{
					if (destiny != 3)
					{
						if (Risto[i].ID == StatusPlayer.BuyedEquips[k].ID)
							destiny = 4;
					}
				}

				
			}
			int CurrentI = i;

			textaum = SlotList[i].GetComponentInChildren<Text>();
			battaum = SlotList[i].GetComponentsInChildren<Button>()[0];
			battaum.onClick.AddListener (delegate { ShowDescription(Risto[CurrentI],battaum); });
			textaum.text = Risto[i].Name;
			
			
			textaum = SlotList[i].GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>();
			butaum = SlotList[i].GetComponentsInChildren<Button>()[1];


			if (destiny == 1)
			{
			textaum.text = "$"+Risto[i].Price;
			butaum.image.color = Color.green;
			butaum.onClick.AddListener (delegate { buy(Risto[CurrentI],Risto); });
			}
			else if (destiny == 2)
			{
			textaum.text = "$"+Risto[i].Price;
			butaum.image.color = Color.red;
			}
			else if (destiny == 3)
			{
			butaum.image.color = Color.white;
			textaum.text = "Using";
			}
			else if (destiny == 4)
			{
			butaum.image.color = Color.cyan;
			textaum.text = "Equip";
				butaum.onClick.AddListener (delegate { equip(Risto[CurrentI],Risto); });
			}
			//butaum.onClick.AddListener (delegate { foi(); });
			}
	
	}
	public void ShowDescription(PlayerEquip showing, Button bataum)
	{

		DescriptionText.text = showing.Description;
	}
	public void ShowItemDescription(PlayerItem showing, Button bataum)
	{
		
		DescriptionText.text = showing.Description;
	}

	public void foi(int oi)
	{
		print ("foi "+oi);
	}

	public void buy(PlayerEquip buying, List<PlayerEquip> Risto)
	{

		StatusPlayer.Money -= buying.Price;
		StatusPlayer.BuyedEquips.Add(buying);
		print ("Comprado "+buying.Name+" por $"+buying.Price );
		clearSlot();
		OpenSection(Risto);
	}

	public void buyItem(PlayerItem buying, List<PlayerItem> Risto)
	{


		StatusPlayer.Money -= buying.Price;
		bool teste = false;
		for (int i = 0; i < StatusPlayer.CurrentItens.Count; i++)
		{
			if (buying.ID == StatusPlayer.CurrentItens[i].ID)
			{
				teste = true;
				StatusPlayer.CurrentItens[i].Quantity += buying.Quantity;
				if (StatusPlayer.CurrentItens[i].Quantity > 99)
					StatusPlayer.CurrentItens[i].Quantity = 99;
				print ("added quantity");
			}
		}
		if (teste != true)
		{
			StatusPlayer.CurrentItens.Add(buying);
			print ("buyed new");
		}

			
		print ("Comprado "+buying.Name+" por $"+buying.Price );
		clearSlot();
		OpenItens(Risto);
		StatusPlayer.HandlerItem.ItemQuantity();
	}

	public void equip(PlayerEquip equipping, List<PlayerEquip> Risto)
	{
		string Typo = equipping.GetType().Name;
		print("Type-S: "+Typo);
		for (int m = 0; m < StatusPlayer.CurrentEquips.Count; m++)
		{
			string Typoru = StatusPlayer.CurrentEquips[m].GetType().Name;
			if (Typoru == Typo)
				StatusPlayer.CurrentEquips.RemoveAt(m);
		}
		if (Typo == "Hull")
		{
			StatusPlayer.CurrentHull = equipping as Hull;
		}
		else if (Typo == "FuelTank")
			StatusPlayer.CurrentFuelTank = equipping as FuelTank;
		else if (Typo == "Drill")
			StatusPlayer.CurrentDrill = equipping as Drill;
		else if (Typo == "Cargo")
			StatusPlayer.CurrentCargo = equipping as Cargo;
		else if (Typo == "Rocket")
			StatusPlayer.CurrentRocket = equipping as Rocket;
		StatusPlayer.CurrentEquips.Add(equipping);
		clearSlot();
		OpenSection(Risto);
		print ("Equipado: "+equipping.Name);
		StatusPlayer.atualizeStats();

	}


	public void clearSlot()
	{

		int counto = SlotList.Count;
			for (int i = 0; i<counto;i++)
		{
			Destroy (SlotList[SlotList.Count-1]);
			SlotList.RemoveAt(SlotList.Count-1);
		}
		DescriptionText.text = "Description";
	}


}
