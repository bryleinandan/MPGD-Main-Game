using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    int count;

    void Awake() {
        text = GetComponent<TMPro.TMP_Text>();
    }

    void Start() => UpdateCount();

    void OnEnable() => RocketOverworldItem.OnCollected += OnCollectibleCollected;
    void OnDisable() => RocketOverworldItem.OnCollected -= OnCollectibleCollected;
    
    void OnCollectibleCollected() {
        count++;
        UpdateCount();
    }

    void UpdateCount() {
        if (count < 8) {
            text.text = $"{count} / {RocketOverworldItem.total}";
        } else {
            text.text = "Congratulations! You've collected all the rockets!";
        }
        
    }
}
