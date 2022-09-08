using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVisualChangerManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private List<Sprite> ballSkins;
    [SerializeField] private Image curSkin;
    private SpriteRenderer _ballPrefabsSprite;

    private void Awake()
    {
        _ballPrefabsSprite = ballPrefab.gameObject.GetComponent<SpriteRenderer>();
        curSkin.sprite = _ballPrefabsSprite.sprite;
    }    
    private void ChangeBallSkin(int number)
    {
        _ballPrefabsSprite.sprite = ballSkins[number];
        curSkin.sprite = _ballPrefabsSprite.sprite;
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
