using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour {
	public GameObject GameManager;
	private GameEditorManager managerGame;

	public float distance = 5000f;

	void Start ()
	{
		managerGame = GameManager.GetComponent("GameEditorManager") as GameEditorManager;

	}

	//replace Update method in your class with this one
	void Update () 
	{    
		//if mouse button (left hand side) pressed instantiate a raycast
		if(Input.GetMouseButtonDown(0))
		{
			if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
			{
						//create a ray cast and set it to the mouses cursor position in game
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, distance)) 
						{
							//draw invisible ray cast/vector
							Debug.DrawLine (ray.origin, hit.point);
							//log hit area to the console
							managerGame.coordsToUse =  new Vector2 (Mathf.FloorToInt(hit.point.x),Mathf.FloorToInt(hit.point.y));
							//Debug.Log(hit.point);
							Debug.Log(managerGame.coordsToUse);
							managerGame.changeBlockValue(new Vector2(hit.point.x,-1* hit.point.y));


						}
						else print ("sorry");
			}

		}   
		//else print ("dalva legal");
	}
}
