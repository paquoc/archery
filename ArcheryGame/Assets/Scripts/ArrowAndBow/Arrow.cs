using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefineAndConstant;

public class Arrow : MonoBehaviour
{
    int id;
    Rigidbody2D rigidBody;
    List<ArrowObserver> arrowObservers = new List<ArrowObserver>();
    ArrowState state;

    public int Id { get => id; set => id = value; }

    public enum ArrowState
    {
        Idle,
        Flying,
        Hit,
        Outside
    }

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    void Start()
    {
        state = ArrowState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ArrowState.Flying)
        {
            Vector3 v = rigidBody.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void Fly(float force)
    {
        gameObject.AddComponent<Rigidbody2D>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();        
        Vector3 vectorForce = Quaternion.Euler(new Vector3(
                    transform.rotation.eulerAngles.x,
                    transform.rotation.eulerAngles.y,
                    transform.rotation.eulerAngles.z)) * new Vector3(50f * force, 0, 0);
        rigidBody.AddForce(new Vector2(vectorForce.x, vectorForce.y));
        transform.parent = null;
        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<TrailRenderer>().enabled = true;
        state = ArrowState.Flying;
        NotifyAll(state);       
    }

    internal void SetEnabledState(bool enable)
    {
        if (enable)
        {
            gameObject.SetActive(true);
            state = ArrowState.Idle;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
	
	internal void InitState(){
		Destroy(rigidBody);
		gameObject.GetComponent<TrailRenderer>().enabled = false;
		GetComponent<PolygonCollider2D>().enabled = false;
		state = ArrowState.Idle;
		gameObject.SetActive(true);
	}

    internal void SetTailPosition(Vector3 pos)
    {
        float lengthArrow = 2.7f;
        float angle = transform.rotation.z * Mathf.Deg2Rad;
        float dx = lengthArrow / 2 * Mathf.Cos(angle);
        float dy = lengthArrow / 2 * Mathf.Sin(angle);
        transform.localPosition = new Vector3(pos.x + dx, pos.y + dy, 0);
    }

    public void AttachObserver(ArrowObserver gameObject)
    {
        arrowObservers.Add(gameObject);
    }

    public void SetParent(Transform parent)
    {
        gameObject.transform.parent = parent;
    }

    private void NotifyAll(ArrowState state)
    {
        for (var i = 0; i < arrowObservers.Count; i++)
            arrowObservers[i].NotifyArrowStateChanged(gameObject, state);
    }

    private void OnBecameInvisible()
    {
        state = ArrowState.Outside;
        NotifyAll(state);
        SetEnabledState(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NotifyAll(ArrowState.Hit);
    }
}
