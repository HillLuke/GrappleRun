using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 Velocity => _rigidbody2D.velocity;

    public bool Move;
    public float MoveSpeed;
    public float JumpPower;
    public float ColliderRange;
    public LayerMask GrappleLayer;

    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint2D;
    private LineRenderer _lineRenderer;

    private Collider2D[] _grapplePointsInRange;

    [SerializeField]
    private bool _jump;
    [SerializeField]
    private bool _grapple;
    [SerializeField]
    private bool _started;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _springJoint2D = GetComponent<SpringJoint2D>();
        _lineRenderer = GetComponent<LineRenderer>();

        _springJoint2D.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!GrappleRun.Grapple.Play)
        {
            return;
        }

        var vel = _rigidbody2D.velocity;

        if (!_started)
        {
            vel.x = MoveSpeed;
        }

        if (_jump && !_started)
        {
            vel.y = JumpPower;
            vel.x = JumpPower;
            _jump = false;
            _started = true;
        }

        _rigidbody2D.velocity = vel;
        _grapplePointsInRange = Physics2D.OverlapCircleAll(gameObject.transform.position, ColliderRange, GrappleLayer);
    }

    private void Update()
    {
        if (!GrappleRun.Grapple.Play)
        {
            return;
        }

        if (Input.GetButtonDown("Jump") && !_jump)
        {
            _jump = true;
        }

        if (Input.GetMouseButton(0))
        {
            StartGrapple();
        }
        else
        {
            StopGrapple();
        }

        _lineRenderer.SetPosition(0, gameObject.transform.position);
    }

    public void Init()
    {
        Move = true;
    }

    private Vector2 NextPoint()
    {
        if (_grapplePointsInRange == null || _grapplePointsInRange.Length == 0)
        {
            return Vector2.zero;
        }

        var closest = Vector2.zero;
        var last = _grapplePointsInRange.OrderByDescending(x => x.gameObject.transform.position.x).Select(y => y.transform.position).First();

        return last;
    }

    private void StartGrapple()
    {
        if (!_grapple)
        {
            var closest = NextPoint();
            if (closest != Vector2.zero)
            {
                _started = true;
                _springJoint2D.enabled = true;
                _springJoint2D.connectedAnchor = closest;

                var distance = Vector2.Distance(gameObject.transform.position, closest);

                _springJoint2D.distance = distance;

                _lineRenderer.enabled = true;
                _lineRenderer.SetPosition(0, gameObject.transform.position);
                _lineRenderer.SetPosition(1, closest);


                var vel = _rigidbody2D.velocity * 1.2f;
                _rigidbody2D.velocity = vel;

                _grapple = true;
            }
        }
    }


    private void StopGrapple()
    {
        if (_grapple)
        {
            //var vel = _rigidbody2D.velocity;// * 1.5f;
            //_rigidbody2D.velocity = vel;

            _springJoint2D.enabled = false;
            _lineRenderer.enabled = false;
            _grapple = false;
        }
    }

    
}
