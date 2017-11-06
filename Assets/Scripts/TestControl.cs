using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestControl : MonoBehaviour {
	EmoEngine engine;
	public float processInterval = 0.1f;
	bool lock_device = false;
    public static uint userId;
	void Start()
	{
		engine = EmoEngine.Instance;
		engine.Connect();
		engine.UserRemoved += onUserRemoved;
        engine.UserAdded += onUserAdded;
		StartCoroutine(process());
		
	}

	private void onUserAdded(object sender, EmoEngineEventArgs args)
	{
        userId = args.userId;
		Debug.Log("user added");
        Debug.Log("Current User: " + userId.ToString());
	}
	private void onUserRemoved(object sender, EmoEngineEventArgs args)
	{
		Debug.Log("user removed");
		lock_device = false;
	}

	IEnumerator process()
	{
		yield return new WaitForSeconds(processInterval);
		connectDevice();
		engine.ProcessEvents();
		StartCoroutine(process());
	}

	void connectDevice()
	{
		try
		{
			engine.ProcessEvents(10);
#if UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID
			int number_insight = EdkDll.Plugin_IEE_GetInsightDeviceCount();
			int number_epoc_plus = EdkDll.Plugin_IEE_GetEpocPlusDeviceCount();
			if (number_insight + number_epoc_plus > 0)
			{
				if (!lock_device)
				{
					lock_device = true;
					if (number_insight > 0)
					{
						for (int i = 0; i < number_insight; i++)
						{
							Int32 state = -1;
							EdkDll.Plugin_IEE_GetInsightDeviceState(out state, i);
							if (state == 1)
							{
								EdkDll.Plugin_IEE_ConnectInsightDevice(i);
								return;
							}
						}
						EdkDll.Plugin_IEE_ConnectInsightDevice(0);
					}
					else
					{
						for (int i = 0; i < number_epoc_plus; i++)
						{
							Int32 state = -1;
							EdkDll.Plugin_IEE_GetEpocPlusDeviceState(out state, i);
							if (state == 1)
							{
								EdkDll.Plugin_IEE_ConnectEpocPlusDevice(i);
								return;
							}
						}
						EdkDll.Plugin_IEE_ConnectEpocPlusDevice(0);
					}
				}
			}
			else
				lock_device = false;
#endif
		}
		catch (EmoEngineException e)
		{
			Console.WriteLine("{0}", e.ToString());
		}
		catch (Exception e)
		{
			Console.WriteLine("{0}", e.ToString());
		}
	}
    public void connectButton(Button btn){
        Text t = btn.GetComponentInChildren<Text>();
        ColorBlock cb = btn.colors;
        if (t.text.Equals("CONNECT")){

            t.text = "DISCONNECT";
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            btn.colors = cb;
        }
        else{
            engine.Disconnect();
            t.text = "CONNECT";
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            btn.colors = cb;
        }
    }
}
