using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Arrow;

public class GameData : MonoBehaviour, ArrowObserver
{
    public static GameData Instance;
    private int score = 0;
    private int numArrow = 15;
    private int numFlyingArrow = 0;
    private int hp = 10;
    private int level = 1;
    private int bonusArrowC = 1;

    public int Score { get => score; set => score = value; }
    public int NumArrow { get => numArrow; set => numArrow = value; }
    public int HP { get => hp; set => hp = value; }
    public int Level { get => level; set => level = value; }
    public int NumFlyingArrow { get => numFlyingArrow; set => numFlyingArrow = value; }
    public int BonusArrowC { get => bonusArrowC; set => bonusArrowC = value; }

    void Awake()
    {
        Instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustScore(int score)
    {
        this.score += score;
        if (this.score < 0) this.score = 0;
    }

    public void AdjustHP(int hp)
    {
        this.hp += hp;
        if (this.hp < 0) this.hp = 0;
    }

    public void AdjustArrow(int number)
    {
        this.numArrow += number;
        if (this.numArrow < 0) this.numArrow = 0;
    }

    public void AdjustLevel()
    {
        this.level++;
    }

    public bool IsEnoughArrow(int n)
    {
        return this.numArrow >= n;
    }

    public void NotifyArrowStateChanged(GameObject arrow, ArrowState state)
    {
        if (state == ArrowState.Outside)
            this.numFlyingArrow--;
        else if (state == ArrowState.Flying)
            this.numFlyingArrow++;
    }

    internal bool CheckEndGame()
    {
        return numFlyingArrow <= 0 && this.numArrow <= 0;
    }
}
