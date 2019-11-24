using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour, BalloonObserver, ArrowObserver
{
    public static QuestManager Instance;
    public GameObject QuestUiObject;
    public GameObject ArrowBonusPrefab;
    IQuest CurrentQuest;
    int curr = 0;
    public List<GameObject> ListQuestPrefab = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CurrentQuest = GetNewQuest();
        CurrentQuest.InitValue();
        CurrentQuest.ShowUi(QuestUiObject);
    }

    private IQuest GetNewQuest()
    {
        int randIdx = UnityEngine.Random.Range(0, ListQuestPrefab.Count);
        IQuest res = ((GameObject)Instantiate(ListQuestPrefab[randIdx])).GetComponent<IQuest>();
        return res;
    }

    public void NotifyBalloonWithItemStateChanged(GameObject balloon, Balloon.BalloonState balloonState, Balloon.ItemState itemState)
    {
        if (balloonState == Balloon.BalloonState.bursted)
            CurrentQuest.ReceiveBalloonExploreNotify(balloon);
    }

    public void OnCurrentQuestDone(int numArrowReward)
    {
        ShowAnimAddArrowBonus(numArrowReward);
        CurrentQuest = GetNewQuest();
        CurrentQuest.InitValue();
        CurrentQuest.ShowUi(QuestUiObject);
    }

    public void NotifyArrowStateChanged(GameObject arrow, Arrow.ArrowState state)
    {
        if (state == Arrow.ArrowState.Hit)
        {
            CurrentQuest.ReceiveArrowHitBalloonNotify(arrow);
        }
    }

    public void ShowAnimAddArrowBonus(int numArrow) {
        for (int i = 0; i < 1; i++)
        {
            GameObject arrow = Instantiate(ArrowBonusPrefab);
            float timeInvoke = GetTimeAnimation(arrow.GetComponent<Animator>());
            IEnumerator coroutine = OnArrowMoveDone(arrow, numArrow, timeInvoke);
            StartCoroutine(coroutine);
        }
    }

    float GetTimeAnimation(Animator animator)
    {
        return 1f;
    }

    IEnumerator OnArrowMoveDone(GameObject arrowBonus, int numArrow, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(arrowBonus);
        MainManager.Instance.AdjustArrow(numArrow);
    }
}
