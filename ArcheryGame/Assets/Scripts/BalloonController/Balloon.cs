using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefineAndConstant;

public class Balloon : MonoBehaviour
{
    private GameObject boundingBox;
    private float moveSpeed = 0.5f;
    private float frequency = 5f;
    private float magnitude = 0.02f;
    private int score = 5;
    private GameObject item;

    private Vector3 pos;
    private float index;
    public bool isFly = false, isBursting = false, isAnimationScore = false;

    private List<BalloonObserver> balloonObserveres = new List<BalloonObserver>();
    private Animator animator;

    private Random random = new Random();

    private static List<Color> listColorDefault = new List<Color>();
    private static List<Vector3> listPositionDefault = new List<Vector3>();

    private static Vector3 leftBoundingBox, topBoundingBox, bottomBoundingBox, rightBoundingBox, startPosition;

    public GameObject ArrowBonus;
    private GameObject myArrowBonus;
    private bool isBonusArrow = false;
    private float time = 1.0f, i = 0.0f, rate = 0.0f;
    private Vector3 startPosAnimation;


    public Balloon()
    {
       
    }

    public enum BalloonState
    {
        normal,
        bursted, 
        outer
    }

    public enum ItemState
    {
        Null,
        SpeedDown,
        BonusArrow
    }

    public static BalloonState balloonState = BalloonState.normal;

    private void Awake()
    {
        boundingBox = GameObject.FindWithTag("Bounding Box");
        rightBoundingBox = boundingBox.transform.Find("right").transform.position;
        leftBoundingBox = boundingBox.transform.Find("left").transform.position;
        topBoundingBox = boundingBox.transform.Find("top").transform.position;
        bottomBoundingBox = boundingBox.transform.Find("bottom").transform.position;
        startPosition = new Vector3(rightBoundingBox.x + 1, rightBoundingBox.y, rightBoundingBox.z);
        pos = transform.position;
        setListColor();
        setListPosition();
        attach();
    }
    void Start()
    {
       
    }

    private void setListColor()
    {
        listColorDefault.Add(ColorDefine.Red);
        listColorDefault.Add(ColorDefine.Green);
        listColorDefault.Add(ColorDefine.Orange);
        listColorDefault.Add(ColorDefine.Pink);
        listColorDefault.Add(ColorDefine.Purple);
        listColorDefault.Add(ColorDefine.Yellow);
        listColorDefault.Add(ColorDefine.White);
    }

    public Color GetColor()
    {
        return gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void setListPosition()
    {

    }
    void attach()
    {
        balloonObserveres.Add(MainManager.Instance);
        balloonObserveres.Add(BalloonManager.Instance);
        balloonObserveres.Add(QuestManager.Instance);  
    }

    void Update()
    {
        MoveLeft();
        scoreAnimation();
        doBonusArrowAnim();
    }

    private IEnumerator BalloonStateChanged(BalloonState balloonState, ItemState itemState)
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("isBursted", false);
        NotifyAll(balloonState, itemState);
    }

    private void NotifyAll(BalloonState balloonState, ItemState itemState)
    {
        for (int i=0; i< balloonObserveres.Count; i++)
        {
            balloonObserveres[i].NotifyBalloonWithItemStateChanged(gameObject, balloonState, itemState);
        }
    }

    void MoveLeft()
    {
        if (isFly && !isBursting)
        {
            pos = transform.position;
            pos -= transform.right * Time.deltaTime * moveSpeed;
            transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude *Time.timeScale;
        }
    }


    public void Config(BalloonAttribute attribute)
    {
        int index = Random.Range(0, listColorDefault.Count - 1);
        SetColor(listColorDefault[index]);
        SetPosition(new Vector3(rightBoundingBox.x + 1, Random.Range(topBoundingBox.y - 5, bottomBoundingBox.y + 1), 0));
        SetScore(attribute.score);
        SetItem(attribute.item);
        if (attribute.item != null)
        {
            Vector3 tempPosition = transform.position;
            tempPosition.x += 1f;
            tempPosition.y += 2f;
            attribute.item.transform.position = tempPosition;
            attribute.item.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            attribute.item.transform.SetParent(gameObject.transform);
        }
        this.moveSpeed = attribute.moveSpeed;
        this.frequency = Random.Range(8f, 12f) ;
        this.magnitude = Random.Range(0.05f, 0.15f);
        this.isFly = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = this.score.ToString();
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetItem(GameObject item)
    {
        this.item = item;
    }

    public GameObject GetItem()
    {
        return this.item;
    }
    
    public void SetMoveSpeed(float newMoveSpeed)
    {
        this.moveSpeed = newMoveSpeed;
    }

    public float GetMoveSpeed()
    {
        return this.moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "arrow")
        {
            Notify(Balloon.BalloonState.bursted);
            animator = gameObject.GetComponent<Animator>();
            animator.SetBool("isBursted", true);
            transform.GetChild(0).gameObject.SetActive(true);
            isAnimationScore = true;
            isBursting = true;
            Invoke("ResetPosition", 0.75f);
        }
        if (collision.gameObject.tag == "castle")
        {
            Notify(Balloon.BalloonState.outer);
            animator = gameObject.GetComponent<Animator>();
            animator.SetBool("isBursted", true);
            isBursting = true;
            Invoke("ResetPosition", 0.75f);
        }
    }

    private void Notify(BalloonState balloonState)
    {
        if (transform.childCount == 1)
        {
            NotifyAll(balloonState, ItemState.Null);
        }
        else
        {
            switch (transform.GetChild(1).tag)
            {
                case "speeddown":
                    NotifyAll(balloonState, ItemState.SpeedDown); break;
                case "bonusarrow":
                    NotifyAll(balloonState, ItemState.BonusArrow);
                    if (balloonState== BalloonState.bursted)
                    {
                        prepareDoAnimation();
                        isBonusArrow = true;
                    }
                    break;
            }
            Destroy(transform.GetChild(1).gameObject);
        }
    }


    private void ResetPosition()
    {
        animator.SetBool("isBursted", false);
        isBursting = false;
        isAnimationScore = false;
        SetPosition(startPosition);
        isFly = false;
        //gameObject.SetActive(false);
    }

    private void scoreAnimation()
    {
        if (isAnimationScore)
        {
            Vector3 temp = transform.GetChild(0).gameObject.transform.position;
            float y = temp.y;
            y += 0.075f;
            transform.GetChild(0).gameObject.transform.position = new Vector3(temp.x, y, temp.z);
        }
    }



    private void prepareDoAnimation()
    {
        startPosAnimation = transform.position;
        myArrowBonus = Instantiate(ArrowBonus, transform.position, Quaternion.identity) as GameObject;
        myArrowBonus.transform.Rotate(0f, 0f, 40.89f);
        Invoke("OffFlatIsBonusArrow", 1f);
    }

    private void doBonusArrowAnim()
    {
        if (isBonusArrow)
        {
            rate = 1.0f / time;
            if (i < 1.0)
            {
                i += Time.deltaTime * rate;
                myArrowBonus.gameObject.transform.position = Vector3.Lerp(new Vector3(startPosAnimation.x, startPosAnimation.y, 0f),
                new Vector3(-11.8f, 8.36f, 0f), i);
            }
        }
    }

    private void OffFlatIsBonusArrow()
    {
        isBonusArrow = false;
        time = 1.0f; i = 0.0f; rate = 0.0f;
        Destroy(myArrowBonus);
    }

}
