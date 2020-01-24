using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DelayAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    void Start()
    {
        StartCoroutine(DelayAndAnimate());
    }

    // Update is called once per frame
    IEnumerator DelayAndAnimate()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.5f));

        _animator.enabled = true;
        _animator.playbackTime = 0;
    }
}