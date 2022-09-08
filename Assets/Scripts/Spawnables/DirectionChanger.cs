using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            InteractWithBall(ball);
        }
    }
    private void InteractWithBall(Ball ball)
    {
        ball.ChangeBallDirectionMethod(ball);
    }
}
