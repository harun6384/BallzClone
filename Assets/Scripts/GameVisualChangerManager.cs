using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVisualChangerManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private List<Sprite> ballSkins;
    private SpriteRenderer _ballPrefabsSprite;

    private void Awake()
    {
        _ballPrefabsSprite = ballPrefab.gameObject.GetComponent<SpriteRenderer>();
    }    
    private void ChangeBallSkin(int number)
    {
        _ballPrefabsSprite.sprite = ballSkins[number];
    }
    public void BallSkinButton(ButtonNumber button)
    {
        ChangeBallSkin((int)button);
    }
}
public enum ButtonNumber
{
    First,
    Second,
    Third,
    Fourth,
    Fifth
}
