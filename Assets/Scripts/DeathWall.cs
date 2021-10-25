using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    public float Speed;

    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!GrappleRun.Grapple.Play)
        {
            return;
        }

        var distance = Vector2.Distance(new Vector2(gameObject.transform.position.x, 0), new Vector2(GrappleRun.Grapple.Player.gameObject.transform.position.x, 0 ));
        var newX = distance;
        var vel = _rigidbody2D.velocity;
        vel.x = newX;
        _rigidbody2D.velocity = vel;
    }

    public void Init()
    {

    }
}
