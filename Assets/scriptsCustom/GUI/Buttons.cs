using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Buttons : MonoBehaviour {
	public GameObject player;
	private PlayerControl MovimentoPlayer;
	private PlayerPhysics3D FisicaPlayer;
	private DiggingMechanics DrillWorks;
	public bool[] apertado; 		// 0= up 1 = down 2 = left 3 = right
	public Image Up, Down, Left, Right;
	public Vector2 localPosition,position,anchoredposition;

	// Use this for initialization
	void Start () {
		MovimentoPlayer = player.GetComponent ("PlayerControl") as PlayerControl;
		FisicaPlayer = player.GetComponent ("PlayerPhysics3D") as PlayerPhysics3D;
		DrillWorks = player.GetComponent ("DiggingMechanics") as DiggingMechanics;
		apertado = new bool[4];
	//	if (MovimentoPlayer.joystickToggle != 3) gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (MovimentoPlayer.joystickToggle != 3) gameObject.SetActive(false);
		else if (MovimentoPlayer.joystickToggle == 3) gameObject.SetActive(true);
		position = Up.rectTransform.position; 
		localPosition = Up.rectTransform.localPosition;
		anchoredposition = Up.rectTransform.anchoredPosition;
		//Rect teste =  new Rect(position.x-39.2f,position.y-55.05f,78.4f,110.1f);
		Rect testeUp = genRect(Up);
		Rect testeDown = genRect(Down);
		Rect testeLeft = genRect(Left);
		Rect testeRight = genRect(Right);
//		//DrawQuad(teste,Color.red);
		bool testeApertou = false;

//		for (int i = 0; i<apertado.Length; i++)
//		{
//			if (apertado[i] == true)
//				testeApertou = true;
//		}


		if (Input.touchCount > 0)
		{
			for (int i = 0; i <= Input.touchCount-1; i++)
			{
			if	(Input.GetTouch(i).phase == TouchPhase.Began)
				{
				bool isRed = false;
				if (EventSystem.current.IsPointerOverGameObject (Input.GetTouch(i).fingerId) || EventSystem.current.IsPointerOverGameObject (-1))
			{
				print(EventSystem.current.currentSelectedGameObject);
			}
				if (!EventSystem.current.IsPointerOverGameObject (Input.GetTouch(i).fingerId)&& !EventSystem.current.IsPointerOverGameObject (-1))
			   dropIt();
			}

			}
		}
		
	
		
		//COLOCAR O SCRIPT SECRETO AQUI
		
		
		
		
		
		
	}
	
	
	
	public bool WasJustADamnedButton()
	{
		UnityEngine.EventSystems.EventSystem ct
			= UnityEngine.EventSystems.EventSystem.current;
		
		if (! ct.IsPointerOverGameObject() ) return false;
		if (! ct.currentSelectedGameObject ) return false;
		if (ct.currentSelectedGameObject.GetComponent<Button>() == null )
			return false;
		
		return true;
	}
	
	public void up()
	{
		
		MovimentoPlayer.TouchPosition.y = 1;
		apertado[0] = true;
		if (FisicaPlayer.grounded || MovimentoPlayer.digdown || MovimentoPlayer.digLR)
		{
			MovimentoPlayer.TouchPosition.x = 0;
			MovimentoPlayer.digLR = false;
		}
	}
	public void down()
	{
		MovimentoPlayer.TouchPosition.y = -1;
		apertado[1] = true;
		if (FisicaPlayer.grounded || MovimentoPlayer.digdown || MovimentoPlayer.digLR)
		{
			MovimentoPlayer.TouchPosition.x = 0;
			MovimentoPlayer.digLR = false;
		}
	}
	public void left()
	{
		MovimentoPlayer.TouchPosition.x = -1;
		apertado[2] = true;
		if (FisicaPlayer.grounded || MovimentoPlayer.digdown || MovimentoPlayer.digLR)
		{
			MovimentoPlayer.TouchPosition.y = 0;
			MovimentoPlayer.digdown = false;
		}
	}
	public void right()
	{
		MovimentoPlayer.TouchPosition.x = 1;
		apertado[3] = true;
		if (FisicaPlayer.grounded || MovimentoPlayer.digdown || MovimentoPlayer.digLR)
		{
			MovimentoPlayer.TouchPosition.y = 0;
			MovimentoPlayer.digdown = false;
		}
	}
	public void release(int i)
	// 0= up 1 = down 2 = left 3 = right
	{
		int calaboca = 0;
		apertado[i] = false;
		

		for (int j = 0; j< 4; j++)
			if (apertado[j])
				calaboca+= 1;
		if (calaboca == 0)
		{
			MovimentoPlayer.TouchPosition = new Vector2(0,0);
			MovimentoPlayer.digdown = MovimentoPlayer.digLR = false;
		}
		else {
			if (apertado[0] ==  true && apertado[1] == apertado[2] == apertado[3] == false) 
				up();
			else if (apertado[1] ==  true && apertado[0] == apertado[2] == apertado[3] == false) 
				down();
			else if (apertado[2] ==  true && apertado[0] == apertado[1] == apertado[3] == false) 
				left();
			else if (apertado[3] ==  true && apertado[0] == apertado[2] == apertado[1] == false) 
				right();
		//	else if ( apertado[0] == apertado[1] == apertado[2] == apertado[3] == false)
		//		MovimentoPlayer.DropItHard();
		}
	}


	public Rect genRect(Image oi)
	{
		RectTransform rt = (RectTransform)oi.rectTransform;
		//Rect ereto = new Rect(oi.rectTransform.position.x-(rt.rect.width/2),oi.rectTransform.position.y-(rt.rect.height/2),rt.rect.width, rt.rect.height );
		Vector3 cornar = rt.TransformPoint(rt.position);

		//cornar = oi.rectTransform.GetWorldCorners();
		Rect ereto = new Rect(cornar.x - (rt.rect.width/2) ,cornar.y - (rt.rect.height/2)  ,cornar.x - (rt.rect.width/2) + rt.rect.width,  rt.rect.height -cornar.y - (rt.rect.height/2) );

		return ereto;
	}


	public void printRect(Rect recto)
	{
		//print (" xmin : "+recto.position.x+" xmax : "+recto.width+" ymin : "+ recto.position.y +" ymax : "+recto.height);
		print (" xmin : "+recto.xMin+" xmax : "+recto.xMax+" ymin : "+ recto.yMin +" ymax : "+recto.yMax);

	}
	public void dropIt()
	{
		MovimentoPlayer.DropItHard();
	}

//	void DrawQuad(Rect position, Color color) {
//		Texture2D texture = new Texture2D(1, 1);
//		texture.SetPixel(0,0,color);
//		texture.Apply();
//		GUI.skin.box.normal.background = texture;
//		GUI.Box(position, GUIContent.none);
//		print ("hello");
//	}

}
