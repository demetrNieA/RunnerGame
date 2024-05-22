using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] GameObject gemUI;
    [SerializeField] GameObject soulsUI;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    
}
