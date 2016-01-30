using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewObjectPoolerScript : MonoBehaviour {

	public static NewObjectPoolerScript current;
	public GameObject pooledObject;
	public int pooledAmount = 8;
	public bool willGrow = true;
	[HideInInspector]
	public List <GameObject> pooledObjects;
	// Use this for initialization

	void Awake()
	{
		current = this;
	}

	void Start () {

		pooledObjects = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			GameObject obj = (GameObject) Instantiate(pooledObject);
			pooledObjects.Add(obj);

		}

	}

	public int GetPooledObject()
	{
		for (int i = 0; i < pooledObjects.Count ; i++)
		{
			if (!pooledObjects[i].activeInHierarchy)
			{
			//	print ("INDEXURU DESU: "+i);
				return i;

			}
		}
		if (willGrow)	
		{
			GameObject obj = (GameObject) Instantiate(pooledObject);
			pooledObjects.Add(obj);

			return pooledObjects.Count-1;
		}
		return -100;
	}

	public bool VerifyPosition(Vector2 posicao)
	{
		for (int i = 0; i < pooledObjects.Count ; i++)
		{
			if (Mathf.FloorToInt(pooledObjects[i].transform.position.x) == Mathf.FloorToInt(posicao.x) && Mathf.FloorToInt(pooledObjects[i].transform.position.y) == Mathf.FloorToInt(posicao.y))
			{

				return true;
			}

		}
		return false;
				
			}
}
