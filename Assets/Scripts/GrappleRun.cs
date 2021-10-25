using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleRun : MonoBehaviour
{
    public static GrappleRun Grapple { get; private set; }
    public bool Play => _play;

    public PlayerController Player;
    public DeathWall DeathWall;

    public Transform GrappleStartFrom;
    public Transform PlayerSpawnPoint;

    public GameObject GrapplePointPrefab;
    public PlayerController PlayerPrefab;

    public float MinSpawnLevel;
    public float MaxSpawnLevel;
    public float SpaceBetween;
    public int GrapplePointsLimit;
    public int Score;
    public int StartAfterSeconds;

    public CinemachineVirtualCamera _cinemachineVirtualCamera;
    private List<GameObject> _grapplePoints;
    private int _moveIndex = 0;
    private int _grappleCount = 0;

    [SerializeField]
    private bool _play;

    private IEnumerator coroutine;

    void Start()
    {
        Grapple = this;
        _grapplePoints = new List<GameObject>();
        var start = GrappleStartFrom.position;
        for (int i = 0; i < GrapplePointsLimit; i++)
        {
            _grapplePoints.Add(Instantiate(GrapplePointPrefab, start, new Quaternion(), GrappleStartFrom));

            start.x += SpaceBetween;
            start.y = Random.Range(GrappleStartFrom.position.y - MinSpawnLevel, GrappleStartFrom.position.y + MaxSpawnLevel);
        }
        _grappleCount = GrapplePointsLimit;

        Player = Instantiate(PlayerPrefab, PlayerSpawnPoint.position, new Quaternion());

        _cinemachineVirtualCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();
        _cinemachineVirtualCamera.Follow = Player.transform;

        coroutine = StartGame(StartAfterSeconds);
        StartCoroutine(coroutine);

    }

    void Update()
    {
        if (!_play)
        {
            return;
        }

        for (int i = 0; i < GrapplePointsLimit; i++)
        {
            if (_grapplePoints[i].transform.position.x < (Player.transform.position.x - 40))
            {
                _grappleCount++;
                var pos = _grapplePoints[i].transform.position;
                pos.x = _grappleCount * SpaceBetween;
                _grapplePoints[i].transform.position = pos;
            }
        }
    }

    IEnumerator StartGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Player.Init();
        DeathWall.Init();
        _play = true;
    }
}
