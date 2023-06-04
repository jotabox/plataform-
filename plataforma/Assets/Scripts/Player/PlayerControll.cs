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
    [SerializeField] private float forceY;
    [SerializeField] private float wallJumpForceY;
    [SerializeField] private float runX;
    [SerializeField] private float wallSlideForce;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isWallSlide;
    [SerializeField] private LayerMask layerMask;
    private BoxCollider2D boxColider;
    public Transform pointWall;





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
        wallSlide();
        slideWallJump();
    }


    private float Horizontal()
    {
        float move = Input.GetAxisRaw("Horizontal");
        return move;

    }

    private void Move()
    {

        // identifica se o player esta correndo
        if ((Horizontal() > 0 || Horizontal() < 0) && Input.GetButton("Fire1"))
        {
            isRunning = true;
            rigi.velocity = new Vector2(Horizontal() * runX, rigi.velocity.y);

        }
        else
        {
            isRunning = false;
            rigi.velocity = new Vector2(Horizontal() * speedX, rigi.velocity.y);

        }


        if (Horizontal() > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (Horizontal() < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }

    private void Jump()
    {

        if (Input.GetButtonDown("Jump") && onGround())
        {
            if (!isRunning)
            {
                rigi.AddForce(Vector2.up * forceY, ForceMode2D.Impulse);
                //rigi.velocity = Vector2.up * forceY;
                isJumping = true;
                isRunning = false;


            }
            else
            {            
                rigi.AddForce(Vector2.up * (forceY + 2), ForceMode2D.Impulse);
                isJumping = true;


            }
        }
    }

    private void doubleJump()
    {
        if (Input.GetButtonDown("Jump") && isJumping && !onGround())
        {
            rigi.AddForce(Vector2.up * forceY, ForceMode2D.Impulse);
            //rigi.velocity = Vector2.up * forceY;
            isJumping = false;
            isRunning = false;

        }
    }

    private void slideWallJump()
    {
        if (isWallSlide && Input.GetButtonDown("Jump"))
        {
            //rigi.AddForce(Vector2.up * wallJumpForceY, ForceMode2D.Impulse);
            rigi.velocity = Vector2.up * wallJumpForceY;

            isJumping = true;
            isRunning = false;

        }
    }

    // verifica se o box colider esta tocando o chão    
    private bool onGround()
    {
        RaycastHit2D isGrounded = Physics2D.BoxCast(boxColider.bounds.center, boxColider.bounds.size, 0, Vector2.down, 0.1f, layerMask);
        return isGrounded.collider != null;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(pointWall.position, radius);
    //}

    private bool isWalled()
    {


        bool isWall = Physics2D.OverlapBox(pointWall.position, new Vector2(.8f, 0.88f), 0 , layerMask);

        if(isWall && Horizontal() != 0 && !onGround())
        {
            return isWall;
        }
        else
        {
            return false;
        }

    }


    private void wallSlide()
    {
        if (isWalled())
        {
            rigi.velocity = new Vector2(rigi.velocity.x, Math.Clamp(rigi.velocity.y, -wallSlideForce, float.MaxValue));
            isWallSlide = true;
            isRunning = false;


        }
        else
        {
            isWallSlide = false;
        }
    }

}
