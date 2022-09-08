using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDestroyer : SpawnableBase
{
    [SerializeField] private SpriteRenderer childImage;
    [SerializeField] private List<Sprite> image;
    private SpriteRenderer _spriteRenderer;
    private Color _initialColor;
    private Vector3 _initialScale;
    private Vector3 _multiplier;
    private float type;
    private Vector3 _xMultiplier = new Vector3(40, 1, 1);
    private Vector3 _yMultiplier = new Vector3(1, 80, 1);
    private Sprite _initialSprite;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialScale = transform.localScale;
        SetType();
    }
    private void Start()
    {
        _initialColor = _spriteRenderer.color;
    }
    private void OnEnable()
    {
        EventManager.OnBallReturn += OnBallReturnHandler;
    }

    private void OnDisable()
    {
        EventManager.OnBallReturn -= OnBallReturnHandler;
    }
    public override void SetProperties()
    {
        destroyable = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            InteractWithBall(ball);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = _initialScale;
        _spriteRenderer.color = _initialColor;
        childImage.sprite = _initialSprite;
    }
    private void SetType()
    {
        var rand = Random.Range(0, 2);
        _multiplier = rand == 0 ? _xMultiplier : _yMultiplier;
        type = rand == 0 ? transform.localScale.x : transform.localScale.y;
        childImage.sprite = image[rand];
        _initialSprite = childImage.sprite;
    }
    private void InteractWithBall(Ball ball)
    {
        SoundManager.PlaySound("specialHit");
        ExtendLength();
    }
    private void ExtendLength()
    {
        var targetScale = transform.localScale = type * _multiplier;
        transform.localScale = Vector3.Lerp(_initialScale, targetScale, 1f);
        childImage.sprite = null;
        UpdateVisualState();
    }
    private void OnBallReturnHandler()
    {
        DestroyObject();
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
    private void UpdateVisualState()
    {
        _spriteRenderer.color = Color.Lerp(_initialColor, Color.yellow, transform.localScale.sqrMagnitude / 10f);
    }
}
