﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public int Health = 4;
    public int Lives = 2;
    public int Coins = 0;
    public Image HealthBar;
    public Sprite[] HealthBarLevels;
    public GameObject PlumberSprite;
    public GameObject PlumberBase;
    public Text Lifebox;
    public Text Coinbox;
    public AudioSource AudioSource;
    public AudioClip respawnSound;

    // Use this for initialization
    void Start () {
        AudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Lifebox.text = "LIFE x " + Lives.ToString();
        Coinbox.text = "COIN x " + Coins.ToString();
        HealthBar.sprite = HealthBarLevels[Health];

        if (Health == 0)
        {
            StartCoroutine(Die());
        }

        if (Coins > 99)
        {
            Lives++;
            Coins = 0;
        }
	}

    public IEnumerator Die ()
    {
        Lives--;
        PlumberSprite.GetComponent<Animator>().SetTrigger("Die");
        PlumberBase.GetComponent<Rigidbody2D>().isKinematic = true;
        
        if (Lives < 0)
        {
            SceneManager.LoadSceneAsync("GameOver");
        }

        Health = 4;

        yield return new WaitForSeconds(3);
        AudioSource.PlayOneShot(respawnSound);

        PlumberSprite.transform.position = new Vector3(0, 0, 0);
        PlumberBase.GetComponent<Rigidbody2D>().isKinematic = false;
        PlumberBase.GetComponent<PlumberController>().dead = false;
    }
}
