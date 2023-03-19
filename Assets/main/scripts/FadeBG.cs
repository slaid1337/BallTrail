using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeBG : Singletone<FadeBG>
{
    private Image _image;

    public void Fade()
    {
        _image = GetComponent<Image>();

        _image.DOColor(new Color(0f, 0f, 0f, 0.6f), 0.5f);
    }
}
