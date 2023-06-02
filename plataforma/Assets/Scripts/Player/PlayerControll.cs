using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControll : MonoBehaviour
{

    Rigidbody2D rigi;
    [SerializeField] private float speedX;
   public float forceY;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private LayerMask layerMask;
    private BoxCollider2D boxColider;




    // Start is called before the first frame update 

    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        boxColider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        doubleJump();
    }


    private void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rigi.velocity = new Vector2(move * speedX, rigi.velocity.y);

        if (move > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (move < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }

    private void Jump()
    {

        if (Input.GetButtonDown("Jump") && onGround())
        {
            rigi.AddForce(Vector2.up * forceY, ForceMode2D.Impulse);
            //rigi.velocity = Vector2.up * forceY;
            isJumping = true;
        }
    }

    private void doubleJump()
    {
        if (Input.GetButtonDown("Jump") && isJumping && !onGround())
        {
            rigi.AddForce(Vector2.up * forceY, ForceMode2D.Impulse);
            //rigi.velocity = Vector2.up * forceY;
            isJumping = false;
        }
    }

    // verifica se o box colider esta tocando o chão    
    private bool onGround()
    {
        RaycastHit2D isGrounded = Physics2D.BoxCast(boxColider.bounds.center, boxColider.bounds.size, 0, Vector2.down, 0.1f, layerMask);
        return isGrounded.collider != null;
    }

}
