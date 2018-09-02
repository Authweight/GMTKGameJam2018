using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value;

    public int Collect()
    {
        SoundEvents.Play("ChipCrunch");
        Destroy(gameObject);
        return value;        
    }

}
