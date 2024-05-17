using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathCombatInterface : CombatInterface
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null) transform.rotation = transform.parent.rotation;
    }
}
