using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    IEnumerator callFunc;
    float timeAnimHitDamage = 2.0f / 6;

    void Start()
    {
        callFunc = StopAnimHitDamage(timeAnimHitDamage);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "balloon")
        {
            StopCoroutine(callFunc);
            GetComponent<Animator>().SetBool("GetDamage", true);
            callFunc = StopAnimHitDamage(timeAnimHitDamage);
            StartCoroutine(callFunc);
        }
    }

    IEnumerator StopAnimHitDamage(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Animator>().SetBool("GetDamage", false);

    }
}
