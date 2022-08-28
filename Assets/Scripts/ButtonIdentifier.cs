using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIdentifier : MonoBehaviour
{
    [SerializeField] private ButtonNumber buttonNumber;
    public ButtonNumber ButtonNumber => buttonNumber;

    public GameVisualChangerManager gameVisualChangerManager;
    public void ChangeSkin()
    {
        gameVisualChangerManager.BallSkinButton(buttonNumber);
    }
}
