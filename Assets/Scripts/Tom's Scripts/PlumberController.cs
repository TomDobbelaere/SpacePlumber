﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlanetGravity))]
[RequireComponent(typeof(AudioSource))]
public class PlumberController : MonoBehaviour {
    private Rigidbody2D rigidbody2d;
    private Transform sprite;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private PlanetGravity pg;
    private AudioSource audioSource;

    public bool dead = false;
    public AudioClip jumpSound;
    public Manager Manager;

    private float jumpForce = 100f;
    private float jumpTime;
    private float jumpTimeMax = 0.15f;
    private bool grounded;
    
	// Use this for initialization
	void Start () {
        rigidbody2d = GetComponent<Rigidbody2D>();
        sprite = transform.Find("SpriteContainer");
        anim = sprite.Find("Sprite").GetComponent<Animator>();
        spriteRenderer = sprite.Find("Sprite").GetComponent<SpriteRenderer>();
        pg = GetComponent<PlanetGravity>();
        audioSource = GetComponent<AudioSource>();
	}
	
	//Remember, physics related stuff always in FixedUpdate
	void FixedUpdate () {
        if (!dead) FixedMovementUpdate();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    public void TakeDamage()
    {
        Manager.Health--;

        if (!dead && Manager.Health < 1)
        {
            dead = true;
            anim.SetTrigger("Die");
        }
    }

    void UpdateAnimation()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") < 0f;
        }

        anim.SetBool("Jump", !grounded);
    }

    void FixedMovementUpdate()
    {
        Vector2 forward = Vector2.Perpendicular(pg.Gravity.normalized);

        if (grounded)
        {
            jumpTime = 0f;
            rigidbody2d.AddForce(forward * Input.GetAxisRaw("Horizontal") * 20f);
        }
        else
        {
            rigidbody2d.AddForce(forward * Input.GetAxisRaw("Horizontal") * 20f);
        }

        Vector2 castPosition = (Vector2)transform.position + pg.Gravity.normalized * 0.6f;
        RaycastHit2D groundCast = Physics2D.Raycast(castPosition, pg.Gravity.normalized, 0.05f);
        grounded = groundCast.collider != null;

        Debug.DrawRay(castPosition, pg.Gravity * 0.1f, grounded ? Color.cyan : Color.yellow);
        
        if (jumpTime < jumpTimeMax && Input.GetButton("Jump"))
        {
            rigidbody2d.AddForce(-pg.Gravity.normalized * jumpForce, ForceMode2D.Force);
        }
        jumpTime += Time.fixedDeltaTime;

        if (Input.GetButtonDown("Jump") && grounded) audioSource.PlayOneShot(jumpSound);

        //Limits the character from going as fast as sonic
        rigidbody2d.velocity = Vector2.ClampMagnitude(rigidbody2d.velocity, 8f);

        Debug.DrawRay(transform.position, forward * 1f, Color.green);
    }

    
}
