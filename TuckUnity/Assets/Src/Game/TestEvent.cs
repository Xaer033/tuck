﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;

public class TestEvent : EventDispatcherBehavior 
{
	// Use this for initialization
	void Awake () 
	{
        AddListener(GameEventType.CARD_DROPPED, (e) =>
        {
            Debug.Log("TestEvent: " + name);
        });
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
