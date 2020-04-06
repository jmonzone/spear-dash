using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectedAbilityDisplay : MonoBehaviour
{
    public void Init(Sprite sprite, Action onClick)
    {
        var image = GetComponent<Image>();
        image.sprite = sprite;

        var button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick?.Invoke());
    }

}
