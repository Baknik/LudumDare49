using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Transform PlayerTransform;

    private void FixedUpdate()
    {
        this.transform.position = PlayerTransform.position;
    }
}
