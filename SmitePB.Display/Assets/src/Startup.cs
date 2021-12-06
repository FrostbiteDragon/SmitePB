using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain;

public class Startup : MonoBehaviour
{
    private void Awake()
    {
        GodService.OnGodPicked += (_, god) => Debug.Log(god.name);
    }
}
