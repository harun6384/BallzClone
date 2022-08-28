using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReturn : MonoBehaviour
{
    [SerializeField] private BallLauncher _ballLauncher;
    private bool _isPositionCached = false;
    private Vector3 _ballPosition;

    public Vector3 BallPosition => _ballPosition;
    public bool IsPositionCached
    {
        get { return _isPositionCached; }
        set { _isPositionCached = value; }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            InteractWithBall(ball);
        }
    }
    private void InteractWithBall(Ball ball)
    {
        if (_isPositionCached == false)
        {
            _ballPosition = GetBallPosition(ball);
            _isPositionCached = true;
        }
        _ballLauncher.ReturnBall();
        ball.gameObject.SetActive(false);
    }
    private Vector3 GetBallPosition(Ball ball)
    {
        return ball.transform.position;
    }
}
