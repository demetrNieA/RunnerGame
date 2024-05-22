using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemUI : PickupDisplayUI
{
    [SerializeField] TextMeshProUGUI gemCount;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ResourceTracker.instance.onGemsChangedEvent?.AddListener(UpdateText);
    }

    void UpdateText()
    {
        TriggerComeOnScreen();
        gemCount.text = "x " + ResourceTracker.instance.GetGems().ToString();
    }
}
