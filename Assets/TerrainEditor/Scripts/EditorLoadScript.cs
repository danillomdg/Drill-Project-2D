using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class EditorLoadScript : MonoBehaviour {
	public GameObject ManagerGame;
	public GameObject terrain;
	public Canvas rosto;
	public RectTransform myPanel;
	public GameObject SlotElement;
	List<GameObject> SlotList = new List<GameObject>();

	private EditorSaveLoad rodo;
	private PolygonGenerator porigon;
	private GameEditorManager ManagerEditor;
	
	public static List<GameEditorData> savedPatterns = new List<GameEditorData>();

	// Use this for initialization
	void Start () {
		rodo = ManagerGame.GetComponent("EditorSaveLoad") as EditorSaveLoad;
		ManagerEditor = ManagerGame.GetComponent("GameEditorManager") as GameEditorManager;
		porigon = terrain.GetComponent ("PolygonGenerator") as PolygonGenerator;

		//OpenLoaded();
	}

	public void OpenLoaded()
	{

				if(File.Exists(Application.persistentDataPath + "/savedPatterns.gd")) {
					
					BinaryFormatter bf = new BinaryFormatter();
					FileStream file = File.Open(Application.persistentDataPath + "/savedPatterns.gd", FileMode.Open);
					savedPatterns = (List<GameEditorData>)bf.Deserialize(file);
					file.Close();
					

					print("Done! games saved: "+savedPatterns.Count);
					//SlotList.Clear();
					if (savedPatterns.Count > 0)
						{


								for (int i = 0; i<savedPatterns.Count;i++)
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

									
									textaum = SlotList[i].GetComponentInChildren<Text>();
									battaum = SlotList[i].GetComponentsInChildren<Button>()[0];
									//battaum.onClick.AddListener (delegate { FinallyLoad(savedPatterns,i); });
									textaum.text = savedPatterns[i].Name;
									
									int CurrentI = i;
									textaum = SlotList[i].GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>();
									butaum = SlotList[i].GetComponentsInChildren<Button>()[1];
									textaum.text = "Load";
									butaum.onClick.AddListener (delegate { FinallyLoad(savedPatterns,CurrentI); });
									
								}
					}
			else 
			{
				print ("ops");
				//SlotList.Clear();
			}

				}	

	
	}

	public void FinallyLoad(List<GameEditorData> Risto, int i)
	{
		porigon.blocks = Risto[i].blocks;	
		ManagerEditor.GameName = Risto[i].Name;
		porigon.EditorUpdateMesh();
		gameObject.SetActive(false);
	}

	public void ClearSlotList()
	{
		for (int i = 0; i<savedPatterns.Count;i++)
		{
			Destroy(SlotList[i]);
			SlotList.Remove(SlotList[i]);
		}

	}



	
	// Update is called once per frame
	void Update () {
	
	}
}
