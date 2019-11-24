using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private static int maximumNumber = 0;

    private static int ratio = 2;
    private List<int> listIndexRandom = new List<int>();

    private static int lastBalloonNumber = 0;

    private GameObject item;

    protected static List<AttributeItem> listItem = new List<AttributeItem>();
    

    private void Awake()
    {
        Instance = this;
        setDefaultListItem();
    }

    private void setDefaultListItem()
    {
        Sprite speeddown = Resources.Load<Sprite>("Textures/Items/speeddown");
        Sprite bonusarrow = Resources.Load<Sprite>("Textures/Items/bonusarrow");
        listItem.Add(new AttributeItem("speeddown", speeddown));
        listItem.Add(new AttributeItem("bonusarrow", bonusarrow));
    }

    public bool AttactItem(string tagName, Sprite sprite)
    {
        listItem.Add(new AttributeItem(tagName, sprite));
        return true;
    }

    public GameObject GenerateItem(int balloonNumber, int generatedBalloonAmount)
    {
        if (balloonNumber != lastBalloonNumber)
        {
            maximumNumber = balloonNumber / ratio;
            randomIndex(balloonNumber);
            lastBalloonNumber = balloonNumber;
        }
        if (SuitableIndex(generatedBalloonAmount - 1))
        {
            return getRandromItem();
        }
        return null;
    }

    private GameObject getRandromItem()
    {
        item = new GameObject("Item");
        int time = System.DateTime.Now.Millisecond;
        int index = time % listItem.Count;
        item.tag = listItem[index].tagName;
        item.gameObject.AddComponent<SpriteRenderer>();
        item.gameObject.GetComponent<SpriteRenderer>().sprite = listItem[index].sprite;
        return item;
    }

    private bool SuitableIndex(int generatedBalloonAmount)
    {
        if (listIndexRandom.Contains(generatedBalloonAmount))
        {
            listIndexRandom.Remove(generatedBalloonAmount);
            return true;
        }
        return false;
    }

    private void randomIndex(int balloonNumber)
    {
        for (int i=0; i<maximumNumber; i++)
        {
            int time = System.DateTime.Now.Millisecond - System.DateTime.Now.Second;
            int index = time % balloonNumber;
            while (listIndexRandom.Contains(index))
            {
                time = System.DateTime.Now.Millisecond - System.DateTime.Now.Second;
                index = time % balloonNumber;
            }
            listIndexRandom.Add(index); 
        }
    }


    public struct AttributeItem
    {
        public string tagName;
        public Sprite sprite;

        public AttributeItem(string tagName, Sprite sprite)
        {
            this.tagName = tagName;
            this.sprite = sprite;
        }
    };
}
