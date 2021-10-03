using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool On;
    public Material OffMaterial;
    public Material OnMaterial;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = this.GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        On = false;
    }

    private void Update()
    {
        _meshRenderer.sharedMaterial = On ? OnMaterial : OffMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        On = true;
    }

    private void OnTriggerStay(Collider other)
    {
        On = true;
    }

    private void OnTriggerExit(Collider other)
    {
        On = false;
    }
}
