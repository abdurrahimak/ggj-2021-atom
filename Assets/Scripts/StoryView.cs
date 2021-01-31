using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryView : MonoBehaviour
{
    public Button NextButton;

    public event Action SkipEvent;

    // Start is called before the first frame update
    void Start()
    {
        NextButton.onClick.AddListener(() =>
        {
            SkipEvent?.Invoke();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
