using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBall : SpawnableBase
{
    private BlockSpawner _spawner;
    private void Awake()
    {
        SetProperties();
        _spawner = FindObjectOfType<BlockSpawner>().GetComponent<BlockSpawner>();
    }
    private void OnEnable()
    {
        EventManager.OnBallReturn += OnBallReturnHandler;
    }
    private void OnDisable()
    {
        EventManager.OnBallReturn -= OnBallReturnHandler;
    }
    private void Start()
    {
        _spawner.SpawnedObject.Add(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            InteractWithBall(ball);
        }
    }
    public override void SetProperties()
    {
        destroyable = true;
    }
    private void OnBallReturnHandler()
    {
        StartCoroutine(Kaydir());
    }
    private IEnumerator Kaydir()
    {
        var desiredPos = transform.position + Vector3.down * _spawner.DistanceBetweenBlocks;
        float sure = 1f;
        for (float i = 0; i < sure; i += 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPos, i / sure);
            yield return null;
        }
    }
    private void InteractWithBall(Ball ball)
    {
        if (destroyable)
        {
            _spawner.SpawnedObject.Remove(gameObject);
            EventManager.RaiseOnAddBall();
            Destroy(gameObject);
        }
    }
}
