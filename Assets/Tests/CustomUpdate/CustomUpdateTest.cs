using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FenrirPack.CustomTime;

public class CustomUpdateTest : MonoBehaviour
{
	float time;

    void Start()
    {
		CustomUpdateManager.Instance.AddListenerForInterval(0.5f, OnCustomUpdate);
		time = Time.timeSinceLevelLoad;
	}

	private void OnCustomUpdate()
	{
		time = Time.timeSinceLevelLoad - time;
		Debug.Log("Update called again. time since level load: " + time+" seconds");
	}
}
