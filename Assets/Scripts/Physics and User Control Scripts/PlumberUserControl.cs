﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlumberUserControl : MonoBehaviour {
    public GameObject Ground;
    public bool Jump;
    public bool CanMoveLeft;
    public bool CanMoveRight;
    public Manager HUD;
    public Animator SpriteControl;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Ground != null)
        {
            //Quaternion.RotateTowards(transform.rotation, Ground.transform.rotation, 1);
        }

        if (Input.GetAxisRaw("Horizontal") > 0.05 && Ground != null && CanMoveLeft == true)
        {
            transform.RotateAround(Ground.transform.position, Vector3.forward, 40 * Time.deltaTime * -1);
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetAxisRaw("Horizontal") < -0.05 && Ground != null && CanMoveRight == true)
        {
            transform.RotateAround(Ground.transform.position, Vector3.forward, 40 * Time.deltaTime * 1);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKeyDown("space") && Jump == false)
        {
            Jump = true;
            Debug.Log("ADD");
            GetComponent<Rigidbody2D>().AddForce(transform.TransformDirection(Vector3.up) * Time.deltaTime * 40000);
            SpriteControl.SetBool("Jump", true);
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            Ground = collision.gameObject;
            Jump = false;
            SpriteControl.SetBool("Jump", false);
        }

        if (collision.gameObject.tag == "NonPlanetGround")
        {
            Jump = false;
            SpriteControl.SetBool("Jump", false);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Jump = true;
            Debug.Log("ADD");
            GetComponent<Rigidbody2D>().AddForce(transform.TransformDirection(Vector3.up) * Time.deltaTime * 5000);
            SpriteControl.SetBool("Jump", true);
            //Destroy(collision.gameObject);
            HUD.Health--;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
          
        }
    }
}
