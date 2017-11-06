using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeControl : MonoBehaviour {

    public GameObject cam;

    private Rigidbody rb;
    private float distance = 8;
    private bool pickUp;
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

    public void aquire(){
        pickUp = true;
        rb.isKinematic = true;
        LoggerCSV.GetInstance().AddEventLog(Time.time, "CubeAquire");
    }
    public void drop(){
        pickUp = false;
        rb.isKinematic = false;
        LoggerCSV.GetInstance().AddEventLog(Time.time, "CubeDrop");
    }
}
