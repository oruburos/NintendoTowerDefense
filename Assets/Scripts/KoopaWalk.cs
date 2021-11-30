using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]
public class KoopaWalk : MonoBehaviour
{

    private Rigidbody2D rb;
    public float speed = 50f;
    public float amplitude = 100;
    private Vector3 originalScale;
    public float limitX = 400f;
    public bool toRight = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        MoveToDirection(Vector2.left);
    }

    private void FixedUpdate()
    {
        if (transform.position.x < limitX)
        {
            MoveToDirection(Vector2.right);
        }
        else if (transform.position.x > limitX + amplitude)
        {

            MoveToDirection(Vector2.left);
        }
    }

    private void MoveToDirection(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }
}
