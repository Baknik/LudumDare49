using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Collider))]
public class UnstableObject : MonoBehaviour
{
    public Material StableMaterial;
    public Material EmissiveMaterial;
    public PhysicMaterial NeutralPhysicsMaterial;
    public PhysicMaterial FrictionlessPhysicsMaterial;

    public bool Emissive;
    public bool Gravity;
    public bool Movable;
    public bool Buoyant;
    public bool Friction;

    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private Outline _outline;
    private Light _light;
    private Collider _collider;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        _outline = this.GetComponent<Outline>();
        _light = this.GetComponentInChildren<Light>();
        _collider = this.GetComponent<Collider>();
    }

    private void Start()
    {
        Untarget();
        UpdateProperties();
    }

    private void FixedUpdate()
    {
        if (Buoyant)
        {
            _rigidbody.AddForce(Physics.gravity * _rigidbody.mass * -1.01f);
        }
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

    private void UpdateRigidbody()
    {
        _rigidbody.useGravity = Gravity;
        _rigidbody.isKinematic = !Movable;
        _collider.sharedMaterial = Friction ? NeutralPhysicsMaterial : FrictionlessPhysicsMaterial;
        _rigidbody.drag = Friction ? 0.2f : 0f;
        _rigidbody.angularDrag = Friction ? 0.2f : 0f;

        _rigidbody.WakeUp();
    }

    private void UpdateProperties()
    {
        UpdateMaterial();
        UpdateLight();
        UpdateRigidbody();
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
        switch (property)
        {
            case PropertyType.EMISSION:
                Emissive = !Emissive;
                break;
            case PropertyType.GRAVITY:
                Gravity = !Gravity;
                break;
            case PropertyType.MOVABLE:
                Movable = !Movable;
                break;
            case PropertyType.BUOYANT:
                Buoyant = !Buoyant;
                break;
            case PropertyType.FRICTION:
                Friction = !Friction;
                break;
        }

        UpdateProperties();
    }
}
