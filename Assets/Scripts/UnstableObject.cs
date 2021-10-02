using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class UnstableObject : MonoBehaviour
{
    public Material StableMaterial;
    public Material EmissiveMaterial;

    public bool Emissive;

    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private Outline _outline;
    private Light _light;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        _outline = this.GetComponent<Outline>();
        _light = this.GetComponentInChildren<Light>();
    }

    private void Start()
    {
        Untarget();
        UpdateProperties();
    }

    private void UpdateMaterial()
    {
        if (Emissive)
        {
            _meshRenderer.sharedMaterial = EmissiveMaterial;
        }
        else
        {
            _meshRenderer.sharedMaterial = StableMaterial;
        }
    }

    private void UpdateLight()
    {
        _light.enabled = Emissive;
    }

    private void UpdateProperties()
    {
        UpdateMaterial();
        UpdateLight();
    }

    public void Target()
    {
        _outline.enabled = true;
    }

    public void Untarget()
    {
        _outline.enabled = false;
    }

    public void FiredAt(PropertyType property)
    {
        Debug.Log($"{this.gameObject.name} fired on with {property}");

        Emissive = !Emissive;
        // TODO CHANGE STATE

        UpdateProperties();
    }
}
