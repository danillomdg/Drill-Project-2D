using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class Controller : MonoBehaviour {
	private Image ControllerSprite;
	public Canvas losto;
	private RectTransform rt;

	// Use this for initialization
	void Start () {
		ControllerSprite = GetComponent<Image>() as Image;
	//	ControllerSprite.rectTransform.anchoredPosition = new Vector2(Screen.width*0.25f,Screen.height*0.25f);
		rt = losto.GetComponent (typeof (RectTransform)) as RectTransform;
	}
	
	// Update is called once per frame
	void Update () {
		ControllerSprite.rectTransform.anchoredPosition = new Vector2(rt.rect.width / -4 ,rt.rect.height / -4);
		//print("lula molusco x "+ ControllerSprite.rectTransform.anchoredPosition.x+"   lula molusco y "+ControllerSprite.rectTransform.anchoredPosition.y);
	}
}
