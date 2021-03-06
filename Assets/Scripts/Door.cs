using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public Switch Switch;
    public float ClosedHeight;
    public float OpenHeight;
    public float Speed;

    private void Start()
    {
        this.transform.DOMoveY(ClosedHeight, 0.01f);
    }

    private void Update()
    {
        if (Switch.TurnedOn)
        {
            Switch.TurnedOn = false;
            this.transform.DOMoveY(OpenHeight, Speed);
        }
        else if (Switch.TurnedOff)
        {
            Switch.TurnedOff = false;
            this.transform.DOMoveY(ClosedHeight, Speed);
        }
    }
}
