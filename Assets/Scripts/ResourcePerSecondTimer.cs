using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles passive income generation over time
public class ResourcePerSecondTimer : MonoBehaviour {
    public float TimerDuration = 1f; // Runs every 1 second by default
   
    public double ResourcePerSecond { get; set; }

    private float _counter;

    // Update is called once per frame
    private void Update() {
        // Accumulate time passed since last frame
        _counter += Time.deltaTime;
      
        // When accumulated time reaches duration target, award resources and reset timer
        if (_counter >= TimerDuration) {
            GameManager.instance.SimpleResourceIncrease(ResourcePerSecond);
            _counter = 0;
        }
    }
}