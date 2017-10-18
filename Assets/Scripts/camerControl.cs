using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class camerControl : MonoBehaviour {

    public float gyroSensitivity;
    public float range;
    public float processInterval = 1;

    EmoEngine engine;

    private bool hasCube;
    private CubeControl cubeScript;
    private float timeLag;

    void Start () 
    {
        engine = EmoEngine.Instance;
        hasCube = false;
	}

    void Update()
    {
        int x = 0, y = 0;

        try //Get change in gyro info from Emotiv
        {
            engine.HeadsetGetGyroDelta(TestControl.userId, out x, out y);
        }
        catch (EmoEngineException e)
        {
            Console.WriteLine("{0}", e.ToString());
        }

        //Apply change in gyro to camera rotation. 
        transform.Rotate(new Vector3(y*gyroSensitivity, x*gyroSensitivity, 0));

		//Force rotation around z to remain at 0
		Quaternion temp = transform.rotation;
        transform.rotation = new Quaternion(temp.x, temp.y, 0, temp.w);

        timeLag += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A)|| EmoFacialExpression.isBlink && timeLag > processInterval)
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
                        cubeScript.aquire(hit.distance);
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
}
