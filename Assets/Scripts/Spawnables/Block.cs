using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : SpawnableBase
{
    [SerializeField] private GameObject particleEffect;
    private SpriteRenderer _spriteRenderer;
    private TMP_Text _text;
    private BlockSpawner _spawner;
    private bool _interacted = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _text = GetComponentInChildren<TMP_Text>();
        _spawner = FindObjectOfType<BlockSpawner>().GetComponent<BlockSpawner>();
        UpdateVisualState();
        SetProperties();
        AddListSetHits();
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
    private void AddListSetHits()
    {
        var hits = Random.Range(1, 3) + _spawner.RowsSpawned;
        SetHits(hits);
    }
    private void UpdateVisualState()
    {
        _text.text = hitsRemaining.ToString();
        _spriteRenderer.color = Color.Lerp(Color.white,Color.red, hitsRemaining / 10f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BallReturn ballReturn))
        {
            InteractWithBallReturn(ballReturn);
        }
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            InteractWithBall(ball);
            Interact();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out LineDestroyer lineDestroyer))
        {
            InteractWithLineDestroyer(lineDestroyer);
        }
    }
    private void InteractWithBallReturn(BallReturn ballReturn)
    {
        RaiseEvent();
    }

    private void RaiseEvent()
    {
        if (!_interacted) EventManager.RaiseOnGameLose();
        _interacted = true;
    }

    private void InteractWithLineDestroyer(LineDestroyer lineDestroyer)
    {
        Interact();
    }
    private void SetHits(int hits)
    {
        hitsRemaining = hits;
        UpdateVisualState();
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
        for (float i = 0; i < sure; i+=0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPos, i / sure);
            yield return null;
        }
    }
    private void Interact()
    {
        hitsRemaining--;
        if (hitsRemaining > 0)
        {
            UpdateVisualState();
        }
        else if (destroyable)
        {
            SoundManager.PlaySound("blockDestroy");
            Instantiate(particleEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity);
            Destroy(gameObject);
            _spawner.SpawnedObject.Remove(gameObject);
        }
    }
    private void InteractWithBall(Ball ball)
    {
        ball.Counter = 0;
    }
}
