using UnityEngine;
using UnityEngine.UI;
using System.Collections;

	


public class RefineryMenuControl : MonoBehaviour {

	public GameObject player;
	private PlayerStats StatusPlayer;
	public GameObject Manager;
	private GameManager ManagerGame;
	private float tradeMoney;


	public int switcherValue;

	Text Texto;
	// Use this for initialization
	void Start () {

		switcherValue = 0;
		StatusPlayer = player.GetComponent("PlayerStats") as PlayerStats;
		ManagerGame = Manager.GetComponent ("GameManager") as GameManager;
		Texto = GetComponentInChildren<Text>();
		Texto.text = "teXte \n oili";
		//OpenRefinery ();
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void OpenRefinery() 
	{
		gameObject.SetActive(true);
		tradeMoney = StatusPlayer.ouro.quantidade *StatusPlayer.ouro.preço + StatusPlayer.prata.quantidade * StatusPlayer.prata.preço + StatusPlayer.bronze.quantidade * StatusPlayer.bronze.preço + StatusPlayer.ferro.quantidade * StatusPlayer.ferro.preço + StatusPlayer.diamante.quantidade * StatusPlayer.diamante.preço;

		Texto.text =    "Gold:   x" + StatusPlayer.ouro.quantidade + "         price:  " + StatusPlayer.ouro.preço + "         total: \b " + StatusPlayer.ouro.quantidade * StatusPlayer.ouro.preço + "\n \n" +
			"silver:   x" + StatusPlayer.prata.quantidade + "         price:  " + StatusPlayer.prata.preço + "         total: \b " + StatusPlayer.prata.quantidade * StatusPlayer.prata.preço + "\n \n" +
				"bronze:   x" + StatusPlayer.bronze.quantidade + "         price:  " + StatusPlayer.bronze.preço + "         total: \b " + StatusPlayer.bronze.quantidade * StatusPlayer.bronze.preço + "\n \n" +
				"iron:   x" + StatusPlayer.ferro.quantidade + "         price:  " + StatusPlayer.ferro.preço + "         total: \b " + StatusPlayer.ferro.quantidade * StatusPlayer.ferro.preço + "\n \n" +
				"diamond:   x" + StatusPlayer.diamante.quantidade + "         price:  " + StatusPlayer.diamante.preço + "         total: \b " + StatusPlayer.diamante.quantidade * StatusPlayer.diamante.preço + "\n \n" +
				"total: " + tradeMoney;



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

	}

	void Restarto(){
		Application.LoadLevel (Application.loadedLevel);
	}


	public void buttonSwitch()
	{
		if (switcherValue == 1) 
		{
		gameObject.SetActive (true);
				OpenRefinery ();
		}
		else if (switcherValue == 2)
				ElectroHead ();	
		else if (switcherValue == 3)
			OpenWorkshop();
		else if (switcherValue == 10)
			Restarto ();
	}

	public void buttonOK()
	{
		tradeMoney = StatusPlayer.ouro.quantidade *StatusPlayer.ouro.preço + StatusPlayer.prata.quantidade * StatusPlayer.prata.preço + StatusPlayer.bronze.quantidade * StatusPlayer.bronze.preço + StatusPlayer.ferro.quantidade * StatusPlayer.ferro.preço + StatusPlayer.diamante.quantidade * StatusPlayer.diamante.preço;

			StatusPlayer.Money += tradeMoney;
			StatusPlayer.EmptyCargo();
			print ("grana: "+StatusPlayer.Money);
	
		gameObject.SetActive (false);
	}
}
