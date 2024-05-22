using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulsUI : PickupDisplayUI
{
    [SerializeField] TextMeshProUGUI soulsCount;

    public override void Start()
    {
        base.Start();
        ResourceTracker.instance.onSoulsChangedEvent?.AddListener(UpdateText);
    }

    void UpdateText()
    {
        TriggerComeOnScreen();
        soulsCount.text = "x " + ResourceTracker.instance.GetSouls().ToString();
    }
}
