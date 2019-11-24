using UnityEngine;
using UnityEditor;

public class BalloonAttribute 
{
    public GameObject item;
    public int score;
    public float moveSpeed;

    public BalloonAttribute(GameObject item, int score, float moveSpeed)
    {
        this.item = item;
        this.score = score;
        this.moveSpeed = moveSpeed;
    }
}