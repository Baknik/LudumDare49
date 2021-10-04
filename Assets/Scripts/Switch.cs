using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool On;
    public Material OffMaterial;
    public Material OnMaterial;
    public bool TurnedOn;
    public bool TurnedOff;
    public AudioSource SFX;
    public AudioClip SFXOn;
    public AudioClip SFXOff;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = this.GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        On = false;
        TurnedOn = false;
        TurnedOff = false;
    }

    private void Update()
    {
        _meshRenderer.sharedMaterial = On ? OnMaterial : OffMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!On)
        {
            SFX.PlayOneShot(SFXOn);
        }

        On = true;
        TurnedOn = true;
    }

    private void OnTriggerStay(Collider other)
    {
        On = true;
        TurnedOff = false;
    }

    private void OnTriggerExit(Collider other)
    {
        On = false;
        TurnedOff = true;
    }
}
