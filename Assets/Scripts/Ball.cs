using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int _counter;
    private Vector2 randomSpeed;
    public int Counter
    {
        get { return _counter; }
        set { _counter = value; }
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * moveSpeed;
    }
    private void ChangeBallDirection(Ball ball)
    {
        GenerateRandomSpeed();
        ball._rigidbody2D.velocity = randomSpeed.normalized;
    }
    private void GenerateRandomSpeed()
    {
        randomSpeed = new Vector2(Random.Range(-1f, 2f), 1f);
    }
    public void ChangeBallDirectionMethod(Ball ball) => ChangeBallDirection(this);
}
