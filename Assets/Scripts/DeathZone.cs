using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float cameraFollowOverflow = 12f;
    
    private Transform cameraTransform;

    private bool isMoving;
    private float maxY;
    
    void Start()
    {
        cameraTransform = Camera.main.transform;
        isMoving = true;
    }
    
    void Update()
    {
        if (!isMoving)
            return;
        
        Vector2 newPosition = transform.position;
        
        if (cameraTransform.position.y - cameraFollowOverflow > transform.position.y)
            // move with camera
            newPosition = new Vector2(transform.position.x, cameraTransform.position.y - cameraFollowOverflow);

        // move slowly up
        newPosition.y += speed * Time.deltaTime;

        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            isMoving = false;
            EventManager.TriggerEvent(Events.ATTACK_PLAYER);
        }
    }
}
