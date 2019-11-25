using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Arrow;

public class MainManager : MonoBehaviour, BalloonObserver, ArrowObserver
{
    public static MainManager Instance;
    //[SerializeField] GameObject bowArrowManager;
    [SerializeField] GameObject levelManager;
    [SerializeField] GameObject audioManager;
    [SerializeField] GameObject uiManager;
    [SerializeField] GameObject Castle;
    public GameObject BG;
    public GameObject snow;

    //private BowArrowManager BAManager;
    private LevelManager LVManager;
    private AudioManager AUManager;
    private UIManager UIs;
    private GameData playerData;
    private static int BalloonCounter;
    private bool isChangingBG = false, isReplacingBG = false;
    protected static List<Sprite> listBG = new List<Sprite>();

    public enum GameStates
    {
        menu,
        paused,
        game,
        end,
    };
    public GameStates gameState = GameStates.menu;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        setDefaultBG();
        LVManager = levelManager.GetComponent<LevelManager>();
        AUManager = audioManager.GetComponent<AudioManager>();
        UIs = uiManager.GetComponent<UIManager>();
        playerData = GameData.Instance;
        AUManager.playMusic("MainMenu");
    }

    private void setDefaultBG()
    {
        Sprite BG = Resources.Load<Sprite>("Textures/Background/BG");
        Sprite Iceberg = Resources.Load<Sprite>("Textures/Background/Iceberg");
        Sprite Candy = Resources.Load<Sprite>("Textures/Background/Candy");
        Sprite Midnight = Resources.Load<Sprite>("Textures/Background/Midnight");
        listBG.Add(BG); listBG.Add(Iceberg); listBG.Add(Candy); listBG.Add(Midnight);
    }

    // Update is called once per frame
    void Update()
    {
        doChangeBackground();
        doReplaceBackground();
    }

    public void StartGame()
    {
        Castle.SetActive(true);
        AUManager.playMusic("InGame");
        gameState = GameStates.game;
        float step = 400;
        Vector3 target = new Vector3(UIs.mainMenu.transform.position.x, UIs.mainMenu.transform.position.y + step, UIs.mainMenu.transform.position.z);
        UIs.playerUI.SetActive(true);
        UIs.MoveMainMenuUp(target);
        NextLevel();
    }

    public void NextLevel()
    {
        // -- Change background
        isChangingBG = true;
        // -- End change background
        if (playerData.Level != 1)
        {
            playerData.AdjustArrow((LVManager.currentLevel + 1));
            playerData.AdjustHP((LVManager.currentLevel + 1) + 1);
            AUManager.UIPlaySFX("NextLevel");
        }
        UpdatePlayerUI();
        LVManager.NextLevel();
        BalloonCounter = LVManager.GetCurrentLevel().balloonNumber;
    }

    public void PauseResumeGame()
    {
        if (gameState == GameStates.game)
        {
            UIs.pauseScreen.SetActive(true);
            gameState = GameStates.paused;

            Time.timeScale = 0f;
        }
        else if (gameState == GameStates.paused)
        {
            UIs.pauseScreen.SetActive(false);
            gameState = GameStates.game;

            Time.timeScale = 1f;
        }
    }

    public void EndGame()
    {
        AUManager.UIPlaySFX("EndGame");
        AUManager.playMusic("MainMenu");

        int lastHiScore = SaveLoadSystem.GetInt(SaveLoadSystem.KeyHiScore, -1, true);
        if (lastHiScore < 0 || lastHiScore < playerData.Score)
        {
            SaveLoadSystem.SetInt(SaveLoadSystem.KeyHiScore, playerData.Score, true);
            lastHiScore = playerData.Score;
        }

        UIs.UpdateHighScore(lastHiScore);

        gameState = GameStates.end;
        Time.timeScale = 0f;
        UIs.endScreen.SetActive(true);
    }

    public void QuitToMain()
    {
         StartCoroutine(LoadScene("LoadingScene"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        gameState = GameStates.menu;
    }

    public IEnumerator LoadScene(string sceneName)
    {
        if (sceneName != "LoadingScene")
        {
            UIs.loadingScreen.SetActive(true);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= .9f)
                {
                    
                        UIs.loadingText.text = "Press any key to continue";
                        //UIs.loadingIcon.SetActive(false);
                        if (Input.anyKeyDown)
                        {
                            asyncLoad.allowSceneActivation = true;

                            Time.timeScale = 1f;

                            UIs.loadingScreen.SetActive(false);
                        }
                }

            }
        }
        else {
            SceneManager.LoadScene(sceneName);
        }
        yield return null;
    }

    void UpdatePlayerData(int Score, int HP)
    {
        playerData.AdjustScore(Score);
        playerData.AdjustHP(HP);
    }

    public void AdjustArrow(int arrowNum)
    {
        GameData.Instance.AdjustArrow(arrowNum);
        UIs.UpdateArrowNumber();
    }

    void UpdatePlayerUI()
    {
        UIs.UpdateScore();
        UIs.UpdateHPoint();
        UIs.UpdateLevel();
        UIs.UpdateArrowNumber();
    }

    public void NotifyArrowStateChanged(GameObject arrow, ArrowState state)
    {
        if (state == ArrowState.Outside)
        {
            if (playerData.CheckEndGame() || playerData.HP == 0)
            {
                EndGame();
            }
        }
    }

    public void NotifyBalloonWithItemStateChanged(GameObject balloon, Balloon.BalloonState balloonState, Balloon.ItemState itemState)
    {
        if (balloonState == Balloon.BalloonState.bursted)
        {
            BalloonCounter--;
            UpdatePlayerData(balloon.GetComponent<Balloon>().GetScore(), 0);
            UpdatePlayerUI();
        }
        else if (balloonState == Balloon.BalloonState.outer)
        {
            BalloonCounter--;
            AUManager.UIPlaySFX("LoseHP");
            UpdatePlayerData(0, -1);
            UpdatePlayerUI();
        }

        if (BalloonCounter == 0 && playerData.HP != 0)
        {
            playerData.AdjustLevel();
            UIs.ShowNextLVText();
            NextLevel();
            UIs.HideNextLVText();
        }

        if (playerData.HP == 0)
        {
            EndGame();
        }
        // --- Handle ballon with item --- ///
        if (balloonState == Balloon.BalloonState.bursted)
        {
            if (itemState != Balloon.ItemState.Null)
                AUManager.balloonPlaySFX("BalloonBonusItem");
            else
                AUManager.balloonPlaySFX("BalloonBurst");
            switch (itemState)
            {
                case Balloon.ItemState.BonusArrow:
                    doBonusArrow();
                    break;
                case Balloon.ItemState.SpeedDown:
                    doSpeedDown();
                    break;
            }
        }


    }

    private void doBonusArrow()
    {
        this.AdjustArrow((LVManager.currentLevel + 1) * playerData.BonusArrowC);
    }

    private void doSpeedDown()
    {
        List<Balloon> temp = LVManager.BLManager.listBalloon;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].isFly)
            {
                LVManager.BLManager.listBalloon[i].SetMoveSpeed(temp[i].GetMoveSpeed()/2);
            }
        }
    }

    private void doChangeBackground()
    {
        if (isChangingBG)
        {
            Color temp = BG.GetComponent<SpriteRenderer>().color;
            temp.a -= 0.05f;
            BG.GetComponent<SpriteRenderer>().color = temp;
            if (temp.a <= 0.1f)
            {
                isReplacingBG = true;
                int time = System.DateTime.Now.Millisecond;
                int index = time % listBG.Count;
                BG.GetComponent<SpriteRenderer>().sprite = listBG[index];
                isChangingBG = false;
                //----- Show snow-- 
                if (time % 2 == 0)
                    snow.SetActive(true);
                else
                    snow.SetActive(false);
            }
        }
    }

    private void doReplaceBackground()
    {
        if (isReplacingBG)
        {
            Color temp = BG.GetComponent<SpriteRenderer>().color;
            temp.a += 0.05f;
            BG.GetComponent<SpriteRenderer>().color = temp;
            if (temp.a >= 1.0f)
            {
                isReplacingBG = false;
            }
        }
    }
}
