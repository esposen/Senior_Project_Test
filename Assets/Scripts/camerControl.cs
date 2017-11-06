using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class camerControl : MonoBehaviour {

    public bool usingEpoc;
    public float gyroSensitivity;
    public float buttonSensitivity;
    public float range;
    public float processInterval = 1;
    public bool hasCube;
    public float timeLag;

    EmoEngine engine;


    private CubeControl cubeScript;
    private int x, y;
    private float bx, by;

    void Start () 
    {
        engine = EmoEngine.Instance;
        hasCube = false;
	}

    //Use asdw or arrows to control camera
    void buttonControll(){

        bx = Input.GetAxis("Vertical") * -1;
        by = Input.GetAxis("Horizontal");

        transform.Rotate(new Vector3(0f, by * buttonSensitivity, 0),Space.World);
        transform.Rotate(new Vector3(bx * buttonSensitivity, 0f, 0f), Space.Self);
    }

    void emotivControl(){
        
		try //Get change in gyro info from Emotiv
		{
			engine.HeadsetGetGyroDelta(TestControl.userId, out x, out y);
		}
		catch (EmoEngineException e)
		{
			Console.WriteLine("{0}", e.ToString());
		}

		//Apply change in gyro to camera rotation. 
        transform.Rotate(new Vector3(0f, x * gyroSensitivity, 0),Space.World);
        transform.Rotate(new Vector3(y * gyroSensitivity, 0f, 0f), Space.Self);

    }
    void checkCube(){
        if (Input.GetKeyDown(KeyCode.Space) || EmoFacialExpression.isBlink && timeLag > processInterval)
		{
			timeLag = 0f;

			if (!hasCube) // Connect gyro to cube to allow movement
			{
				RaycastHit hit;
				//Debug.DrawRay(transform.position, transform.forward, Color.magenta);
				if (Physics.Raycast(transform.position, transform.forward, out hit, range))
				{
					if (hit.collider.gameObject.CompareTag("CUBE"))
					{
						cubeScript = hit.collider.gameObject.GetComponent<CubeControl>();
						cubeScript.aquire();
						hasCube = true;


					}
				}
			}
			else //Drop cube
			{
				cubeScript.drop();
				hasCube = false;
			}
		}
    }
    void Update()
    {

        timeLag += Time.deltaTime; //moniters amount of time since last cube pick up/drop

        if (usingEpoc)
        {
            emotivControl();
        }
        buttonControll();

        checkCube();

        if(Input.GetKeyDown(KeyCode.Y)){
            LoggerCSV.GetInstance().SaveCSV();
        }

	}
}
