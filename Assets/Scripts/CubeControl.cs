using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeControl : MonoBehaviour {

    public bool pickUp;
    public GameObject cam;

    private Rigidbody rb;
    private float distance;

    EmoEngine engine;
	// Use this for initialization
	void Start () {
        engine = EmoEngine.Instance;
        pickUp = false;
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (pickUp){
            gameObject.transform.position = cam.transform.position + cam.transform.forward * distance;

        }
	}

    public void aquire(float dist){
        pickUp = true;
        rb.isKinematic = true;
        distance = dist + .5f; //to account for the width of the box
        Debug.Log(gameObject.name + " has been picked up");    
    }
    public void drop(){
        pickUp = false;
        rb.isKinematic = false;
        Debug.Log(gameObject.name + " has been dropped");
    }
}
