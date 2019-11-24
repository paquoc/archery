using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Bow : MonoBehaviour
{
    // References to the gameobjects / prefabs
    public GameObject bowString;
    public GameObject arrowPooler;
    float timeRespawnArrow = 0.3f;

    // the bowstring is a line renderer
    private List<Vector3> bowStringPosition;
    LineRenderer bowStringLinerenderer;

    // some status vars
    bool bowShot;
    bool bowPrepared;
    BowState state;

    //position of the line renderers middle part 
    Vector3 stringPullout;
    Vector3 stringRestPosition = new Vector3(-0.44f, -0.06f, 2f);

    GameObject arrow;
    ArrowPooler arrowFactory;

    // Use this for initialization
    void Start()
    {
        bowShot = false;
        bowPrepared = false;
        
        SetupLineRender();

        stringPullout = stringRestPosition;
        arrowFactory = arrowPooler.GetComponent<ArrowPooler>();
        CreateArrow();
    }

    private void SetupLineRender()
    {
        bowStringLinerenderer = bowString.AddComponent<LineRenderer>();
        bowStringLinerenderer.SetVertexCount(3);
        bowStringLinerenderer.SetWidth(0.05F, 0.05F);
        bowStringLinerenderer.useWorldSpace = false;
        bowStringLinerenderer.material = Resources.Load("Materials/bowStringMaterial") as Material;
        bowStringPosition = new List<Vector3>();
        bowStringPosition.Add(new Vector3(-0.44f, 1.43f, 2f));
        bowStringPosition.Add(new Vector3(-0.44f, -0.06f, 2f));
        bowStringPosition.Add(new Vector3(-0.43f, -1.32f, 2f));
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, bowStringPosition[1]);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
    }

    void Update()
    {
        
      

        if (Input.GetKeyDown(KeyCode.Escape))
        {
        }

        // game is steered via mouse
        // (also works with touch on android)
        if (Input.GetMouseButtonDown(0) && MainManager.Instance.gameState == MainManager.GameStates.game)
        {
            if (IsPointerOverUIObject())
                return;
            AudioManager.Instance.bowPlaySFX("BowDraw");
        }

        if (Input.GetMouseButton(0))
        {
            if (IsPointerOverUIObject())
                return;
            if (state == BowState.noArrow)
                return;
            if (!GameData.Instance.IsEnoughArrow(1))
                return;
            PullBowString(Input.mousePosition);
        }

        // ok, player released the mouse
        // (player released the touch on android)
        if (Input.GetMouseButtonUp(0) && bowPrepared)
        {
            if (!GameData.Instance.IsEnoughArrow(1))
                return;
            PlayStringSound();
            Shoot();
        }
        // in any case: update the bowstring line renderer
        DrawBowString();
    }

    private void PlayStringSound()
    {
    }

    public void Shoot()
    {
        AudioManager.Instance.bowPlaySFX("ArrowShoosh");
        MainManager.Instance.AdjustArrow(-1);

        bowPrepared = false;
        float x = stringPullout.x, y = stringPullout.y;
        float force = Mathf.Sqrt(x * x + y * y);
        stringPullout = stringRestPosition;
        arrow.GetComponent<Arrow>().Fly(force * 200);
        Invoke("CreateArrow", timeRespawnArrow);
        SetState(BowState.noArrow);
    }

    public void PullBowString(Vector3 mousePos)
    {
        if (bowShot == false)
        {
            Vector3 mousePosConverted = Camera.main.ScreenToWorldPoint(mousePos);
            RotateBow(mousePosConverted);
        }
        bowPrepared = true;
    }

    public void DrawBowString()
    {
        bowStringLinerenderer = bowString.GetComponent<LineRenderer>();
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, stringPullout);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
    }

    public void SetBowStringPos(Vector3 mousePos)
    {
        Vector2 mouse = new Vector2(transform.position.x - mousePos.x, transform.position.y - mousePos.y);
        float length = mouse.magnitude / 3f;
        length = Mathf.Clamp(length, 0, 1);
        stringPullout = new Vector3(-(0.5f + length), -0.06f, 2f);

        arrow.GetComponent<Arrow>().SetTailPosition(stringPullout);
    }

    public void RotateBow(Vector3 mousePos)
    {
        Vector3 lookAt = mousePos;
        float AngleRad = Mathf.Atan2(lookAt.y - transform.position.y, lookAt.x - this.transform.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        transform.rotation = Quaternion.Euler(180, 180, AngleDeg);
        SetBowStringPos(mousePos);
    }

    public Vector3 getPulloutPos()
    {
        return stringPullout;
    }

    public void CreateArrow()
    {
        arrow = arrowFactory.GetNewArrow();

        arrow.transform.rotation = transform.localRotation;
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetParent(transform);
        arrowScript.SetTailPosition(stringRestPosition);

        this.SetState(BowState.available);
    }

    private void SetState(BowState s)
    {
        this.state = s;
    }

    private enum BowState
    {
        noArrow,
        available,
        pulling
    }

    private bool IsPointerOverUIObject()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
