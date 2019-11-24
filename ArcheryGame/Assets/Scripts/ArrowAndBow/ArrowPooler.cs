using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPooler : MonoBehaviour
{
    public GameObject pooledObject;
    public int pooledAmount;
    int availableId = 0;

    int currentAvailable;

    List<GameObject> pooledObjects;

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
            AddNewObject();
        currentAvailable = 0;
    }

    void AddNewObject()
    {
        GameObject obj = (GameObject)Instantiate(pooledObject);
        obj.SetActive(false);
        obj.GetComponent<Arrow>().AttachObserver(GameData.Instance);
        obj.GetComponent<Arrow>().AttachObserver(MainManager.Instance);
        obj.GetComponent<Arrow>().AttachObserver(QuestManager.Instance);
        pooledObjects.Add(obj);
    }

    public GameObject GetNewArrow()
    {
        GameObject objectReturn = pooledObjects[currentAvailable];
        objectReturn.GetComponent<Arrow>().InitState();
        objectReturn.GetComponent<Arrow>().Id = this.availableId++;
        currentAvailable++;
        if (currentAvailable >= pooledAmount)
        {
            if (pooledObjects[0].activeInHierarchy)
                AddNewObject();
            else currentAvailable = 0;
        }
        return objectReturn;
    }
}
