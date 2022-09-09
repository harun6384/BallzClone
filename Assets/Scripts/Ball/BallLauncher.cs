using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private List<GameObject> balls;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballParent;
    [SerializeField] private List<GameObject> instBalls;
    [SerializeField] private BallReturn ballReturn;
    private Vector3 _startDragPosition;
    private Vector3 _endDragPosition;
    private LaunchPreview _launchPreview;
    private bool _isBallsLaunched = false;
    private Vector3 _initialTransform;
    private Vector3 _currentPos;

    private void Awake()
    {
        _launchPreview = GetComponent<LaunchPreview>();
        _initialTransform = transform.position;
        _currentPos = transform.position;
    }

    private void OnEnable()
    {
        EventManager.OnAddBall += OnAddBallHandler;
        EventManager.OnGameLoad += OnGameLoadHandler;
        EventManager.OnGameStart += OnGameStartHandler;
        EventManager.OnGamePause += OnGamePauseHandler;
        EventManager.OnGameContinue += OnGameContinueHandler;
        StopAllCoroutines();
        _isBallsLaunched = false;
        ClearSpawnedBalls();
        instBalls.Clear();
    }
    private void OnDisable()
    {
        EventManager.OnAddBall -= OnAddBallHandler;
        EventManager.OnGameLoad -= OnGameLoadHandler;
        EventManager.OnGameStart -= OnGameStartHandler;
        EventManager.OnGamePause -= OnGamePauseHandler;
        EventManager.OnGameContinue -= OnGameContinueHandler;
    }
    private void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameState.INGAME)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10;
            if (Input.GetMouseButtonDown(0))
            {
                StartDrag(worldPosition);
            }
            else if (Input.GetMouseButton(0))
            {
                ContinueDrag(worldPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isBallsLaunched = false;
        }
    }
    private IEnumerator LaunchBalls()
    {
        Vector3 direction = _endDragPosition - _startDragPosition;
        direction.Normalize();
        foreach (var ball in balls)
        {
            ball.transform.position = transform.position;
            ball.gameObject.SetActive(true);
            instBalls.Add(ball);
            ball.GetComponent<Rigidbody2D>().AddForce(-direction);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void StartDrag(Vector3 worldPosition)
    {
        _startDragPosition = worldPosition;
        _launchPreview.SetStartPoint(transform.position);
    }
    private void ContinueDrag(Vector3 worldPosition)
    {
        _endDragPosition = worldPosition;

        Vector3 direction = _endDragPosition - _startDragPosition;
        _launchPreview.SetStartPoint(transform.position);
        _launchPreview.SetEndPoint(transform.position - direction);
    }
    private void EndDrag()
    {
        if (_isBallsLaunched == false && Vector3.Distance(_startDragPosition, _endDragPosition) > 0.05f)
        {
            StartCoroutine(LaunchBalls());
            _isBallsLaunched = true;
        }
        CloseLaunchPreview();
    }
    public IEnumerator CreateBall()
    {
        yield return new WaitUntil(() => _isBallsLaunched == false);
        LevelEnd();
    }
    public void LevelEnd()
    {
        var ball = Instantiate(ballPrefab, ballParent);
        balls.Add(ball);
    }
    public void ReturnBall()
    {
        instBalls.Remove(instBalls[0]);
        if (instBalls.Count == 0)
        {
            EventManager.RaiseOnBallReturn();
            StartCoroutine(ChangeBallLauncherPosition(1f));
            ballReturn.IsPositionCached = false;
            LevelEnd();
        }
    }
    private void CloseLaunchPreview()
    {
        _launchPreview.SetStartPoint(transform.position);
        _launchPreview.SetEndPoint(transform.position);
        _endDragPosition = Vector3.zero;
        _startDragPosition = Vector3.zero;
    }
    private void OnAddBallHandler()
    {
        StartCoroutine(CreateBall());
    }
    private void ClearSpawnedBalls()
    {
        foreach (Transform child in ballParent)
        {
            balls.Remove(child.gameObject);
            Destroy(child.gameObject);
        }
    }
    private void OnGameLoadHandler()
    {
        ClearSpawnedBalls();
    }
    private void OnGameStartHandler()
    {
        ResetPositions();
        CloseLaunchPreview();
        LevelEnd();
    }
    private void ResetPositions()
    {
        transform.position = _initialTransform;
        _currentPos = _initialTransform;
    }
    private void OnGamePauseHandler()
    {
        _isBallsLaunched = true;
    }
    private void OnGameContinueHandler()
    {
        if (instBalls.Count != 0)
        {
            _isBallsLaunched = true;
        }
        else
        {
            _isBallsLaunched = false;
        }
    }
    private IEnumerator ChangeBallLauncherPosition(float duration)
    {
        float time = 0;
        var targetPos = new Vector3(ballReturn.BallPosition.x, transform.position.y, ballReturn.BallPosition.z);
        while (time < duration)
        {
            transform.position = Vector3.Lerp(_currentPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _currentPos = targetPos;
        _isBallsLaunched = false;
    }
}
