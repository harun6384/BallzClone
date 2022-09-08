using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private GameObject blockParent;
    [SerializeField] private Block[] blocks;
    [SerializeField] private List<float> distances;
    [SerializeField] private GameObject endLine;
    private Color _initialColor;

    private void Awake()
    {
        _initialColor = background.color;
    }
    private void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameState.INGAME)
        {
            UpdateList();
            ChangeBackgroundColor();
        }
    }
    private void UpdateList()
    {
        blocks = blockParent.GetComponentsInChildren<Block>();
    }
    private void ChangeBackgroundColor()
    {
        var distance = GetDistances() / endLine.transform.position.magnitude;
        background.color = Color.Lerp(Color.red, _initialColor, distance);
    }
    private float GetDistance(Block block)
    {
        return Vector3.Distance(transform.position, block.transform.position);
    }
    private float GetDistances()
    {
        if (blocks.Length != 0) 
        {
            ClearDistanceList();
            foreach (var child in blocks)
            {
                if (!distances.Contains(GetDistance(child)))
                {
                    distances.Add(GetDistance(child));
                }
            }
            return distances.Min();
        }
        else
        {
            return endLine.transform.position.magnitude;
        }
    }
    private void ClearDistanceList()
    {
        distances.Clear();
    }
}
