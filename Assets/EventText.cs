using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class EventText : MonoBehaviour {
	public GameObject player;
	private float preTimar = 0.9f; // default: 0.8f
	private float timar;
	[HideInInspector]
	public bool showstart = false;
	public bool eventInit = false;
	private Text texto;
	private float damageStacker, timeholded;
	private Vector3 startPos;
	private bool movingUp;
	private bool fading;

	
	private float defaultHoldTime = 0.9f;
	private int comboStacker;

	
	public CanvasGroup fadeCanvasGroup;
	
	// Use this for initialization
	void Start () {
		startPos = transform.position;
		timeholded = 100;
		texto = gameObject.GetComponent ("Text") as Text;
		gameObject.SetActive(false);
		damageStacker = 0;
		comboStacker = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (eventInit == true)
			lifespam();
		if (showstart == true)
		{
			transform.position = new Vector3 (26,36,0);
			showstart = false;
		}
		if (movingUp == true)
			transform.Translate(new Vector3(0,0.6f,0),Space.World);
		else transform.position = startPos;

		if (fading ==true)
		{
			FadeToBlack(20);
			print ("a we faded");
		}
			//texto.color.a -=1;

	}

	public IEnumerator FadeToBlack(float speed)
	{
		while (fadeCanvasGroup.alpha < 1f)
		{
			fadeCanvasGroup.alpha += speed * Time.deltaTime;
			yield return null;
		}
	}
	


public void lifespam()
{

	gameObject.SetActive(true);
	timar -= Time.deltaTime;
		if (timar <= 0)
		{
			eventInit = false;
			timar = preTimar;
			gameObject.SetActive(false);
		}
	}
	public void ShowEvent(int type, string msg, float value)
	{
		gameObject.SetActive(true);
		eventInit = true;
		timeholded = 100;
		//TEXT BELOW IS WRONG
		//type: 0 = damage taken, 1 = mineral coleted, 2 = money earned, 3 = misk
		if (type == 0)
		{
			texto.color = Color.red;
			texto.text = "- "+value+" HP";
		}

	}


	public void ShowMining(string msg)
	{
		movingUp = false;
		int quantity = 1;
		gameObject.SetActive(true);
		eventInit = true;  

		if (timeholded == 100)
		{
			movingUp = false;
			comboStacker  = 0;
			timeholded = defaultHoldTime;
		}
		if (timeholded == defaultHoldTime )
		{
			transform.position = startPos;
			movingUp = true;
		}
			
			
		comboStacker += quantity;
		//quantity = damageStacker;
			if (timeholded > 0)
			{
				timeholded -=Time.deltaTime;
				
			}
			else if (timeholded < 0)
			{
				movingUp = false;
				timeholded = 100;
				//	damageStacker = 0;
			}
		

		texto.color = Color.green;
		texto.text = "+"+quantity+" "+msg+"!";
	}
	

	public void ShowDamage(string msg, float value, float HoldTime)
	{

		print ("timeholded "+timeholded.ToString("F2"));
		gameObject.SetActive(true);
		eventInit = true;
		if (timeholded == 100)
		{
			movingUp = false;
			damageStacker = 0;
			timeholded = HoldTime;
		}
		if (timeholded == HoldTime )
		{
			transform.position = startPos;
			movingUp = true;
		}
		if (HoldTime > 0)
		{
			fading = false;


			damageStacker += value;
			value = damageStacker;
			if (timeholded > 0)
			{
			timeholded -=Time.deltaTime;

			}
			else if (timeholded < 0)
			{
				movingUp = false;
				timeholded = 100;
			//	damageStacker = 0;
			}
		}
		else fading = true;


	
		//type: 0 = damage taken, 1 = mineral coleted, 2 = money earned, 3 = misk

			texto.color = Color.red;
			texto.text = "- "+value.ToString ("F2")+" HP";
	
		print ("tecnally showing damage");
	}
}
