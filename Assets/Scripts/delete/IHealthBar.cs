using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IHealthBar
{
    public Slider slider { get; set; }
    public Gradient gradient { get; set; }
    public Image fill { get; set; }

    public void SetMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health) {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void TakeDamage(int damage) {
        slider.value = slider.value - damage;
        //slider.value = Mathf.Lerp(minimum, maximum, t), 0, 0);
    }
}