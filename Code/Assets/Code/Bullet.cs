﻿using UnityEngine;

public class Bullet: MonoBehaviour
{
    public float m_Speed = 5.0f;
    public float lifeDuration = 2f;
    float lifeTimer;

    private void Start()
    {
        lifeTimer = lifeDuration;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * m_Speed * Time.fixedDeltaTime;
    }

}