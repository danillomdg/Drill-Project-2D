using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class debugger : MonoBehaviour {
	public GameObject player;
	public GameObject controller;

	private PlayerStats StatusPlayer; 
	private PlayerControl MovimentoPlayer;
	private PlayerPhysics3D FisicaPlayer;
	private DiggingMechanics DrillWorks;
	private Buttons buttonscript;
	public List<Vector2> TouchFinger = new List<Vector2>();


	private Text texxtu;
	// Use this for initialization
	void Start () {
		StatusPlayer = player.GetComponent("PlayerStats") as PlayerStats;
		MovimentoPlayer = player.GetComponent ("PlayerControl") as PlayerControl;
		FisicaPlayer = player.GetComponent ("PlayerPhysics3D") as PlayerPhysics3D;
		DrillWorks = player.GetComponent ("DiggingMechanics") as DiggingMechanics;
		buttonscript = controller.GetComponent ("Buttons") as Buttons;
		texxtu  = GetComponent("Text") as Text;
	}
	
	// Update is called once per frame
	void Update () {
		texxtu.text = "X: "+MovimentoPlayer.Final.x+" Y:"+MovimentoPlayer.Final.y+"\n\n";
		texxtu.text = "Pos: "+buttonscript.position+"  LocalPos: "+buttonscript.localPosition+"AnchoredPos: "+buttonscript.anchoredposition ;
		//texxtu.text = string.Concat(texxtu.text,"up: "+buttonscript.apertado[0]+"  down: "+ buttonscript.apertado[1] + "  left: " + buttonscript.apertado[2] + "  right: " + buttonscript.apertado[3]  );
		if (Input.touchCount > 0)
		{

			for (int i = 0; i <= Input.touchCount-1; i++)
			{

				//texxtu.text = string.Concat(texxtu.text,"TOUCH "+i+": \n x: "+Input.GetTouch(i).position.x +" y:"+Input.GetTouch(i).position.y+" \n" );
				//texxtu.text = string.Concat(texxtu.text,"up: "+buttonscript.apertado[0]+"  down: "+ buttonscript.apertado[1] + "  left: " + buttonscript.apertado[2] + "  right: " + buttonscript.apertado[3]  );
			}
				}
		texxtu.text = (
					   "Digdown: "+MovimentoPlayer.digdown+"\n"+
		               "DigLR: "+MovimentoPlayer.digLR+"\n"+
//		               "Timer: "+DrillWorks.timer+"\n"+
		               "Grounded: "+FisicaPlayer.grounded+"\n"+
//		               "FinalX: "+MovimentoPlayer.Final.x+"FinalY: "+MovimentoPlayer.Final.y+"\n"
//			
		""
			);
	
	}


}
