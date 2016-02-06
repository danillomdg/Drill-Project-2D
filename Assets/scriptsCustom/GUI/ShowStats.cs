using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowStats : MonoBehaviour {
	private Text texas;
	private PlayerStats StatusPlayer;
	private Monster Monsta;
	public GameObject Player;
	public GameObject enemy;
	// Use this for initialization
	void Start () {
		texas = GetComponent("Text") as Text;
		StatusPlayer = Player.GetComponent("PlayerStats") as PlayerStats;
		Monsta = enemy.GetComponent("Monster") as Monster;
		showThem ();
	}
	
	// Update is called once per frame
	void Update () {
		showThem ();
	}

	public void showThem()
	{

			float depth = Player.transform.position.y * -1;
		texas.text = "Money: $"+StatusPlayer.Money+"\n"+
					 "HP: "+StatusPlayer.HP+"\n"+
				"Cargo: "+StatusPlayer.CargoSpace+"%\n"+
				"depth: "+depth.ToString ("F3")+"\n"

				;
	}

}
