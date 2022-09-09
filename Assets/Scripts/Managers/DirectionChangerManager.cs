using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChangerManager : MonoBehaviour
{
    [SerializeField] private GameObject directionChanger;
    [SerializeField] private GameObject directionChangerParent;
    private Vector3 newPos;
    private HashSet<GameObject> collidedGameObjects = new HashSet<GameObject>();
    private HashSet<GameObject> instDirectionChanger = new HashSet<GameObject>();
    [SerializeField] private List<GameObject> instantiatedDirectionChanger;
    [SerializeField] private List<GameObject> colObjects;
    private void OnEnable()
    {
        EventManager.OnBallReturn += OnBallReturnHandler;
        EventManager.OnGameLoad += OnGameLoadHandler;
    }
    private void OnDisable()
    {
        EventManager.OnBallReturn -= OnBallReturnHandler;
        EventManager.OnGameLoad -= OnGameLoadHandler;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            
            InteractWithBall(ball);
            if (collidedGameObjects.Add(ball.gameObject))
            {
                colObjects.Add(ball.gameObject);
            }
            if (ball.Counter >= 15)
            {
                InstantiateDirectionChanger(ball);
            }
        }
    }
    private void InteractWithBall(Ball ball)
    {
        ball.Counter++;
    }
    private void InstantiateDirectionChanger(Ball ball)
    {
        var insDir = Instantiate(directionChanger, GetPosition(ball), Quaternion.identity, directionChangerParent.transform);
        if (instDirectionChanger.Add(insDir))
        {
            instantiatedDirectionChanger.Add(insDir);
        }
        ResetCounter();
    }
    private Vector3 GetPosition(Ball ball)
    {
        newPos = new Vector3(0, ball.transform.position.y, 0);
        return newPos;
    }
    private void DestroyInstantiated()
    {
        foreach (var item in instantiatedDirectionChanger)
        {
            Destroy(item.gameObject);
        }
        instantiatedDirectionChanger.Clear();
    }
    private void ResetCounter()
    {
        foreach (var ball in collidedGameObjects)
        {
            ball.GetComponent<Ball>().Counter = 0;
        }
    }
    private void DestroyObjectsOnLoad()
    {
        collidedGameObjects.Clear();
        instantiatedDirectionChanger.Clear();
        DestroyInstantiated();
    }
    private void OnBallReturnHandler()
    {
        ResetCounter();
        DestroyInstantiated();
    }
    private void OnGameLoadHandler()
    {
        DestroyObjectsOnLoad();
    }
}
