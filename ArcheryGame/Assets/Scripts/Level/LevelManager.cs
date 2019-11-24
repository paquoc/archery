using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject balloonManager;
    public GameObject bow;

    public BalloonManager BLManager;
    public int currentLevel = -1;

    List<Level> levelList = new List<Level>();

    private ItemManager itemManager;


    // Start is called before the first frame update
    void Start()
    {
        BLManager = balloonManager.GetComponent<BalloonManager>();
        itemManager  = ItemManager.Instance; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        bow.SetActive(true);
        StartCoroutine(CreateBalloons(Random.Range(1f, 1.5f)));
    }

    IEnumerator CreateBalloons(float seconds)
    {
        int limit = levelList[currentLevel].balloonNumber;
        while (limit != 0)
        {
            BalloonAttribute balloonConfig = new BalloonAttribute(
                itemManager.GenerateItem(levelList[currentLevel].balloonNumber, limit),
                levelList[levelList.Count - 1].balloonScore, 
                levelList[levelList.Count - 1].balloonSpeed);
            BLManager.CreateBalloon(balloonConfig);
            limit--;
            yield return new WaitForSeconds(seconds);
        }
    }

    public Level GetCurrentLevel()
    {
        return levelList[currentLevel];
    }

    public void NextLevel()
    {
        GenerateNewLevel();
        currentLevel++;
        StartLevel();
    }

    public void End()
    {

    }

    public void GenerateNewLevel()
    {
        Level newLevel;
        if (levelList.Count != 0)
        {
            newLevel = new Level(levelList[currentLevel].balloonNumber + 3, levelList[currentLevel].balloonSpeed + 1f, levelList[currentLevel].balloonScore + 10);
        }
        else
        {
            newLevel = new Level(5,5,30);
        }
        levelList.Add(newLevel);
    }

}
