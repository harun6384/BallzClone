using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class SpawnableBase : MonoBehaviour
{
    protected bool destroyable;
    protected int hitsRemaining;
    public abstract void SetProperties();

}
