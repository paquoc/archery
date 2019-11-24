using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int balloonNumber;
    public float balloonSpeed;
    public int balloonScore;

    public Level()
    {
        balloonNumber = 0;
        balloonSpeed = 0;
        balloonScore = 0;
    }

    public Level(int balloonNumber, float balloonSpeed, int balloonScore)
    {
        this.balloonNumber = balloonNumber;
        this.balloonSpeed = balloonSpeed;
        this.balloonScore = balloonScore;
    }
}
