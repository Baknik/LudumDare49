using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionPush : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
             body.velocity += hit.controller.velocity / 2f;
    }
}
