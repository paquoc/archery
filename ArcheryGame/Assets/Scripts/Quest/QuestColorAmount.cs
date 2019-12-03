using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DefineAndConstant;

public class QuestColorAmount : MonoBehaviour, IQuest
{
    int totalAmount;
    int NumArrowReward;
    int currentAmount = 0;
    Color ColorRequire = ColorDefine.Red;
    GameObject QuestUiObject;
    Text textQuest, textProcess;

    public void ReceiveBalloonExploreNotify(GameObject balloon)
    {
        Balloon balloonScript = balloon.GetComponent<Balloon>();
        if (IsMatch(balloonScript.GetColor()))
            currentAmount++;
        if (currentAmount <= totalAmount)
        {
            this.UpdateUi();
            if (currentAmount == totalAmount)
                OnQuestDone();
        }
    }

    public void OnQuestDone()
    {
        QuestManager.Instance.OnCurrentQuestDone(NumArrowReward);
    }

    private bool IsMatch(Color balloonColor)
    {
        return balloonColor == this.ColorRequire;
    }

    public void ShowUi(GameObject questUiObject)
    {
        this.QuestUiObject = questUiObject;
        this.AddTextQuest();
        this.AddTextProcess();
    }

    private void AddTextProcess()
    {
        this.textProcess = this.QuestUiObject.transform.GetChild(1).GetComponent<Text>();
        this.textProcess.text = this.currentAmount.ToString() + "/" + this.totalAmount.ToString();
        this.textProcess.gameObject.SetActive(true);
    }

    private void AddTextQuest()
    {
        this.textQuest = this.QuestUiObject.transform.GetChild(0).GetComponent<Text>();
        this.textQuest.gameObject.SetActive(true);
        this.textQuest.text = "Shoot " + totalAmount + " " + ColorDefine.GetColorName(this.ColorRequire) + " balloon" 
            +"\nReward: " + NumArrowReward.ToString() + " arrow(s)";
    }

    public void UpdateUi()
    {
        this.textProcess.text = this.currentAmount.ToString() + "/" + this.totalAmount.ToString();
    }

    public void InitValue()
    {
        currentAmount = 0;
        totalAmount = UnityEngine.Random.Range(2, 6);
        NumArrowReward = (int)Math.Floor(0.5 * totalAmount) + 1;
        ColorRequire = ColorDefine.GetRandomColor();
    }

    public void ReceiveArrowHitBalloonNotify(GameObject arrow)
    {
        //do nothing
    }
}
