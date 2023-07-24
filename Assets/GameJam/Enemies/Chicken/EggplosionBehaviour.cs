using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggplosionBehaviour : MonoBehaviour
{    
    public float totalDuration;
    float currentDuration = 0;

    // Update is called once per frame
    void Update()
    {
        currentDuration += Time.deltaTime;
        
        if (currentDuration >= totalDuration)
        {
            Destroy(this.gameObject);
        }
    }
}
