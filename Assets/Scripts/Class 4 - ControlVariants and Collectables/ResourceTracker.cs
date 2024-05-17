using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceTracker : MonoBehaviour
{
    public static ResourceTracker instance;

    int gems = 0;
    int souls = 0;
    int orbs = 0;

    public UnityEvent onGemsChangedEvent;
    public UnityEvent onSoulsChangedEvent;
    public UnityEvent onOrbsChangedEvent;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    #region Gems
    public void AddGems(int gemsToAdd)
    {
        gems += gemsToAdd;
        onGemsChangedEvent.Invoke();
    }

    public int GetGems()
    {
        return gems;
    }

    public void SpendGems(int gemsToSpend)
    {
        gems -= gemsToSpend;
        onGemsChangedEvent.Invoke();
    }
    #endregion

    #region Souls
    public void AddSouls(int soulsToAdd)
    {
        souls += soulsToAdd;
        onSoulsChangedEvent.Invoke();
    }
    public int GetSouls()
    {
        return souls;
    }
    public void ResetSouls()
    {
        souls = 0;
        onSoulsChangedEvent.Invoke();
    }
    #endregion

    #region Orbs
    public void AddOrbs(int orbsToAdd)
    {
        orbs += orbsToAdd;
        onOrbsChangedEvent.Invoke();
    }
    public int GetOrbs()
    {
        return orbs;
    }
    #endregion
}
