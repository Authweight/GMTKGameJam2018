using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value;

    public int Collect()
    {
        Destroy(gameObject);
        return value;        
    }

}
