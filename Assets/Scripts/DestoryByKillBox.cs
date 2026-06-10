using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByKillBox : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
