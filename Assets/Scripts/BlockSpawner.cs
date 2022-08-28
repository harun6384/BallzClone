using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnableBase> _spawnableBase;
    [SerializeField] private List<GameObject> _spawnedObject;
    [SerializeField] private List<GameObject> spawnables;
    [SerializeField] private Transform spawnableParent;
    private int playWidth = 7;
    private float _distanceBetweenBlocks = 0.7f;
    private int _rowsSpawned;
    public List<GameObject> SpawnedObject 
    {
        get { return _spawnedObject; }
        set { _spawnedObject = value; }
    }
    public int RowsSpawned => _rowsSpawned;
    public float DistanceBetweenBlocks => _distanceBetweenBlocks;
    private void Awake()
    {
        foreach (var spawnables in _spawnableBase)
        {
            spawnables.SetProperties();
        }
    }
    private void OnEnable()
    {
        EventManager.OnGameLoad += OnGameLoadHandler;
        EventManager.OnGameStart += OnGameStartHandler;
        EventManager.OnBallReturn += InstantiateSpawnables;
    }

    private void OnDisable()
    {
        EventManager.OnGameLoad -= OnGameLoadHandler;
        EventManager.OnGameStart -= OnGameStartHandler;
        EventManager.OnBallReturn -= InstantiateSpawnables;
    }
    public void InstantiateSpawnables()
    {
        for (int i = 0; i < playWidth; i++)
        {
            var chance = Random.Range(0, 100);
            if (chance > 50) { continue; }
            var randomValue = Random.Range(0, spawnables.Count);
            Instantiate(spawnables[randomValue], GetPosition(i), Quaternion.identity, spawnableParent);
        }
        _rowsSpawned++;
    }
    private Vector3 GetPosition(int i)
    {
        Vector3 position = transform.position;
        position += Vector3.right * i * _distanceBetweenBlocks;
        return position;
    }
    private void DestroyAllSpawnedObjects()
    {
        foreach (Transform child in spawnableParent)
        {
            _spawnedObject.Remove(child.gameObject);
            Destroy(child.gameObject);
        }
    }
    private void OnGameLoadHandler()
    {
        DestroyAllSpawnedObjects();
        _rowsSpawned = 0;
    }
    private void OnGameStartHandler()
    {
        for (int i = 0; i < 1; i++)
        {
            InstantiateSpawnables();
        }
    }
}
