using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuest
{
    void ShowUi(GameObject QuestUiObject);
    void UpdateUi();
    void ReceiveBalloonExploreNotify(GameObject balloon);
    void ReceiveArrowHitBalloonNotify(GameObject arrow);
    void InitValue();
    void OnQuestDone();
}
