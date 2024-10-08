using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        transform.LookAt(player);
    }
}
