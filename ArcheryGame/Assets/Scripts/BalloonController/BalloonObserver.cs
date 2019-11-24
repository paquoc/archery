using UnityEngine;
using UnityEditor;

public interface BalloonObserver 
{
    void NotifyBalloonWithItemStateChanged(GameObject balloon, Balloon.BalloonState balloonState, Balloon.ItemState itemState);
}