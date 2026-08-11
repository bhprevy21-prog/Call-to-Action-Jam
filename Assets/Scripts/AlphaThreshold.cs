using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Improves button click accuracy for UI images with irregular transparent shapes
public class AlphaThreshold : MonoBehaviour
{
    private Image _resourceImage;

    private void Awake() {
        _resourceImage = GetComponent<Image>();
        
        // Prevents fully transparent parts of an image from triggering a button click event
        _resourceImage.alphaHitTestMinimumThreshold = 0.001f; 
    }
}