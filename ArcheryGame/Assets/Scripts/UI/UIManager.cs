using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject optionScreen, pauseScreen, mainMenu, playerUI,endScreen;

    public GameObject loadingScreen/*, loadingIcon*/;

    public Text loadingText;
    public Text arrowText;
    public Text scoreText;
    public Text hpText;
    public Text lvText;
    public Text nextLvText;
    public Text currentScoreText;
    public Text endScoreText;
    public Text hiScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        scoreText.text = GameData.Instance.Score.ToString();
        currentScoreText.text = GameData.Instance.Score.ToString();
        endScoreText.text = GameData.Instance.Score.ToString();
    }

    public void UpdateHPoint()
    {
        hpText.text = GameData.Instance.HP.ToString();
    }

    public void UpdateLevel()
    {
        lvText.text = GameData.Instance.Level.ToString();
    }

    public void UpdateArrowNumber()
    {
        arrowText.text = GameData.Instance.NumArrow.ToString();
    }

    public void ShowNextLVText()
    {
        nextLvText.gameObject.SetActive(true);
        nextLvText.text = "Level " + GameData.Instance.Level.ToString();
        StartCoroutine(MoveRoutine(nextLvText.transform, nextLvText.transform.position, new Vector3(0, 0, 0)));
    }

    public void HideNextLVText()
    {
        StartCoroutine(HideNextLV());
    }


    IEnumerator HideNextLV()
    {
        yield return new WaitForSeconds(2f);
        nextLvText.gameObject.SetActive(false);
        nextLvText.transform.position = new Vector3(0,-344,0);
    }
    public void OpenOptions()
    {
        optionScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionScreen.SetActive(false);
    }

    public void MoveMainMenuUp(Vector3 target)
    {
        StartCoroutine(MoveRoutine(mainMenu.transform, mainMenu.transform.position, target));
    }

    public void resetMainMenu()
    {
        mainMenu.transform.position = new Vector3(0, 0, 0);
        mainMenu.SetActive(true);
    }

    private IEnumerator MoveRoutine(Transform transform, Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        mainMenu.SetActive(false);
    }

    internal void UpdateHighScore(int lastHiScore)
    {
        hiScoreText.text = lastHiScore.ToString();
    }
}
