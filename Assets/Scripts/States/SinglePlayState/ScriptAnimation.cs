using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScriptAnimation : MonoBehaviour
{
    public List<Sprite> Sprites;
    public float speed = 1f;
    private float interval = 0f, timeToChange;
    private int currentIndex = 0;
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = Sprites[currentIndex];
        interval = speed / Sprites.Count;
        timeToChange = interval;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToChange > 0f)
        {
            timeToChange -= Time.deltaTime;
        }
        else
        {
            timeToChange = interval;
            currentIndex += 1;
            currentIndex %= Sprites.Count;
            _image.sprite = Sprites[currentIndex];
        }
    }
}
