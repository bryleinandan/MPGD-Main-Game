using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// delete when text prompt is working.


public class InteractionPromptUI : MonoBehaviour
{
    // UI look at camera - parent some text to the camera

    [SerializeField] private Camera _mainCam;
    [SerializeField] private TextMeshProUGUI _promptText;

    public bool isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main;
        //private TextMeshProUGUI _promptText;
        //text.SetActive(false)
    }

    void LateUpdate() {
        var rotation = _mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public void SetUp(string promptText) {
        _promptText.text = promptText;
        isVisible = true;
    }

    public void Close() {
        isVisible = false;
    }
}
