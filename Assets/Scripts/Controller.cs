using UnityEngine;
using System;
using System.Collections;

public class Controller : MonoBehaviour {
    EmoEngine engine;
    public float processInterval = 0.1f;
	bool lock_device = false;
	void Start () {
        engine = EmoEngine.Instance;
        Debug.Log(engine.ToString());
        engine.Connect();
        engine.UserAdded += onUserAdded;
        engine.UserRemoved += onUserRemoved;
        StartCoroutine(process());
	}

    private void onUserAdded(object sender, EmoEngineEventArgs args){
        Debug.Log("user added");
    }
    private void onUserRemoved(object sender, EmoEngineEventArgs args){
        Debug.Log("user removed");
		lock_device = false;
    }

    IEnumerator process(){
        yield return new WaitForSeconds(processInterval);
		connectDevice();
        engine.ProcessEvents();
        StartCoroutine(process());
    }

	void connectDevice(){
		try
		{
			engine.ProcessEvents(10);
			#if UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID
			int number_insight = EdkDll.Plugin_IEE_GetInsightDeviceCount();
			int number_epoc_plus = EdkDll.Plugin_IEE_GetEpocPlusDeviceCount();
			if(number_insight + number_epoc_plus > 0) {
				if(!lock_device) {
					lock_device = true;
					if(number_insight > 0) {
						for(int i = 0; i < number_insight; i++) {
							Int32 state = -1;
							EdkDll.Plugin_IEE_GetInsightDeviceState(out state, i);
							if(state == 1) {
								EdkDll.Plugin_IEE_ConnectInsightDevice(i);
								return;
							}
						}
						EdkDll.Plugin_IEE_ConnectInsightDevice(0);
					}
					else {
						for(int i = 0; i < number_epoc_plus; i++) {
							Int32 state = -1;
							EdkDll.Plugin_IEE_GetEpocPlusDeviceState(out state, i);
							if(state == 1) {
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
	void OnApplicationQuit()
	{
		Debug.Log("Application ending after " + Time.time + " seconds");
	}
}
