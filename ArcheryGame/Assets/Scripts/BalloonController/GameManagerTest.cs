using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerTest : MonoBehaviour
{
    public GameObject balloonManager;

    void Start()
    {
        InvokeRepeating("CreateBalloon", 1f,0.2f);
    }


    private void CreateBalloon()
    {
        float moveSpeed = Random.Range(0f, 10f);
        int score = Random.Range(0, 255);
        GameObject item = null;
        BalloonAttribute balloonAttribute = new BalloonAttribute(item, score, moveSpeed);
        balloonManager.GetComponent<BalloonManager>().CreateBalloon(balloonAttribute);
    }
}
