using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonManager : MonoBehaviour, BalloonObserver
{
    public static BalloonManager Instance;

    public GameObject balloonPrefab;
    public GameObject boundingBox;

    private GameObject myBalloonPrefab;
    private GameObject balloon;
    private int poolBalloonSize;
    private static Vector3 startPosition;
    private Vector3 leftBoundingBox, topBoundingBox, bottomBoundingBox;

    public List<Balloon> listBalloon = new List<Balloon>();
    public List<BalloonObserver> balloonObservers = new List<BalloonObserver>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolBalloonSize = 5;
        myBalloonPrefab = balloonPrefab;
        Vector3 rightBoundingBox = boundingBox.transform.Find("right").transform.position;
        leftBoundingBox = boundingBox.transform.Find("left").transform.position;
        topBoundingBox = boundingBox.transform.Find("top").transform.position;
        bottomBoundingBox = boundingBox.transform.Find("bottom").transform.position;
        startPosition = new Vector3(rightBoundingBox.x + 1, topBoundingBox.y + 2, rightBoundingBox.z); 
        InitBalloonPool();
    }

    private void InitBalloonPool()
    {
        for (int i = 0; i < poolBalloonSize; i++)
        {
            AddMoreBalloon();
        }
    }

    public void CreateBalloon(BalloonAttribute attribute)
    {
        for (int i = 0; i < listBalloon.Count; i++)
        {
            if (listBalloon[i].isFly == false)
            {
                ConfigBalloon(attribute, i);
                return;
            }
        }
        poolBalloonSize++;
        AddMoreBalloon();
        ConfigBalloon(attribute, listBalloon.Count - 1);
        return;
    }

    private void ConfigBalloon(BalloonAttribute attribute, int index)
    {
        listBalloon[index].gameObject.SetActive(true);
        listBalloon[index].Config(attribute);
        listBalloon[index].isFly = true;
        poolBalloonSize--;
    }

    private void AddMoreBalloon()
    {
        balloon = Instantiate(myBalloonPrefab, startPosition, Quaternion.identity) as GameObject;
        balloon.name = "balloon";
        balloon.SetActive(false);
        listBalloon.Add(balloon.GetComponent<Balloon>());
    }


    private void ReturnToPool(GameObject balloon)
    {
        for (int i = 0; i < listBalloon.Count; i++)
        {
            if (listBalloon[i].gameObject.Equals(balloon))
            {
                listBalloon[i].SetPosition(startPosition);
                listBalloon[i].SetColor(new Color(255, 255, 255));
                listBalloon[i].isFly = false;
                listBalloon[i].gameObject.SetActive(false);
                break;
            }
        }
    }

    public void RegistryObserver(BalloonObserver observer)
    {
        balloonObservers.Add(observer);
    }

    public void NotifyBalloonWithItemStateChanged(GameObject balloon, Balloon.BalloonState balloonState, Balloon.ItemState itemState)
    {
        if (balloonState == Balloon.BalloonState.outer)
        {
            //ReturnToPool(balloon);
        }
    }
}
