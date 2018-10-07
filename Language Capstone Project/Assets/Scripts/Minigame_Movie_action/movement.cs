using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class movement : MonoBehaviour {

    public Vector2 direction;
    public float speed = 0f;

    private void Start()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        direction = new Vector2(x, y).normalized;
    }

    private void Update()
    {
        transform.Translate(direction * speed * 0.05f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = Vector2.Reflect(direction, collision.contacts[0].normal).normalized;
    }
}
