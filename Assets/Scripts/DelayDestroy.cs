using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    [SerializeField] private float delay;
    
    void Start()
    {
        StartCoroutine(DelayAndDestroy());
    }

    IEnumerator DelayAndDestroy()
    {
        yield return new WaitForSeconds(delay);
        
        Destroy(gameObject);
    }
}
