using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : BaseCollectable
{
    [SerializeField] int gemValue = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 45 * Time.deltaTime, 0);
    }

    public override void Collected()
    {
        Debug.Log("Collected");
        ResourceTracker.instance.AddGems(gemValue);
        AudioManager.instance.PlayGemPickupSFX();
    }
}
