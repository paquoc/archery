using UnityEngine;

public interface ArrowObserver
{
    void NotifyArrowStateChanged(GameObject arrow, Arrow.ArrowState state);
}