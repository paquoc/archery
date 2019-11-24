using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCombo : MonoBehaviour, IQuest
{
    int totalAmount = 1;
    int NumArrowReward;
    int NumBalloonCombo;
    int currentAmount = 0;
    GameObject QuestUiObject;
    Text textQuest, textProcess;
    Dictionary<int, int> NumBalloonBeExplored = new Dictionary<int, int>();

    public void InitValue()
    {
        NumBalloonCombo = UnityEngine.Random.Range(2, 5);
        NumBalloonCombo = 2;
        currentAmount = 0;
        NumArrowReward = (int)Mathf.Floor(NumBalloonCombo * 1.8f);
    }

    public void ReceiveBalloonExploreNotify(GameObject balloon)
    {}

    public void ShowUi(GameObject QuestUiObject)
    {
        this.QuestUiObject = QuestUiObject;
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
        this.textQuest.text = "Shoot " + NumBalloonCombo + " balloon with only 1 arrow" +
            "\nReward: " + NumArrowReward.ToString() + " arrow(s)";
    }

    public void UpdateUi()
    {
        this.textProcess.text = this.currentAmount.ToString() + "/" + this.totalAmount.ToString();
    }

    public void ReceiveArrowHitBalloonNotify(GameObject arrow)
    {
        var arrowId = arrow.GetComponent<Arrow>().Id;
        if (NumBalloonBeExplored.ContainsKey(arrowId))
            NumBalloonBeExplored[arrowId] += 1;
        else NumBalloonBeExplored.Add(arrowId, 1);

        if (NumBalloonBeExplored[arrowId] >= NumBalloonCombo)
            OnQuestDone();
    }

    public void OnQuestDone()
    {
        QuestManager.Instance.OnCurrentQuestDone(NumArrowReward);
        //MainManager.Instance.AdjustArrow(NumArrowReward);
    }
}
