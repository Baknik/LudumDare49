using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Gun : SerializedMonoBehaviour
{
    public LayerMask TargetableObjects;
    public Transform GunMuzzleEnd;
    public Color LaserOnColor;
    public Color LaserOffColor;
    public PropertyType[] SelectableProperties;
    public SelectionArch[] SelectionArchs;
    public Transform SelectionWheel;
    public int WheelRotationDegrees = 60;
    public Material NeutralSelectionMaterial;
    public Material OnSelectionMaterial;
    public Material OffSelectionMaterial;
    public Dictionary<int, PropertyType> SelectionAngles;

    private Camera _mainCamera;

    private UnstableObject _targetedObject;
    private LineRenderer _lineRenderer;

    private Vector3 _laserHitPoint;
    private int _selectorWheelTargetRotationAngle;
    private PropertyType _selectedProperty;
    private float _lastSelectionWheelRotation;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _lineRenderer = this.GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        _lineRenderer.startColor = LaserOffColor;
        _lineRenderer.endColor = LaserOffColor;

        _selectorWheelTargetRotationAngle = 0;

        _selectedProperty = SelectableProperties[0];

        _lastSelectionWheelRotation = 0f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Sequence lineColorSequence = DOTween.Sequence();
            lineColorSequence.Append(_lineRenderer.DOColor(new Color2() { ca = LaserOffColor, cb = LaserOffColor }, new Color2() { ca = LaserOnColor, cb = LaserOnColor }, 0.01f));
            lineColorSequence.Append(_lineRenderer.DOColor(new Color2() { ca = LaserOnColor, cb = LaserOnColor }, new Color2() { ca = LaserOffColor, cb = LaserOffColor }, 0.2f));
            lineColorSequence.Play();

            if (_targetedObject != null)
            {
                _targetedObject.FiredAt(_selectedProperty);

                UpdateSelectionArchs();
            }
        }

        _lastSelectionWheelRotation += Time.deltaTime;
        if (_lastSelectionWheelRotation > 0.2f)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.mouseScrollDelta.y > 0f)
            {
                _selectorWheelTargetRotationAngle -= WheelRotationDegrees;
                if (_selectorWheelTargetRotationAngle < 0)
                {
                    _selectorWheelTargetRotationAngle = 300;
                }

                //_selectedProperty -= 1;
                //if (_selectedProperty < 0)
                //{
                //    _selectedProperty = SelectableProperties.Length - 1;
                //}
                _selectedProperty = SelectionAngles[_selectorWheelTargetRotationAngle];

                SelectionWheel.DOLocalRotate(new Vector3(0f, (float)_selectorWheelTargetRotationAngle, 0f), 0.15f);

                _lastSelectionWheelRotation = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.mouseScrollDelta.y < 0f)
            {
                _selectorWheelTargetRotationAngle += WheelRotationDegrees;
                if (_selectorWheelTargetRotationAngle >= 360)
                {
                    _selectorWheelTargetRotationAngle = 0;
                }

                //_selectedProperty += 1;
                //if (_selectedProperty > SelectableProperties.Length - 1)
                //{
                //    _selectedProperty = 0;
                //}
                _selectedProperty = SelectionAngles[_selectorWheelTargetRotationAngle];

                SelectionWheel.DOLocalRotate(new Vector3(0f, (float)_selectorWheelTargetRotationAngle, 0f), 0.19f);

                _lastSelectionWheelRotation = 0f;
            }
        }
    }

    private void LateUpdate()
    {
        _lineRenderer.SetPosition(0, GunMuzzleEnd.position);
        _lineRenderer.SetPosition(1, _laserHitPoint);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        UnstableObject targetedObject = null;
        if (Physics.Raycast(_mainCamera.ScreenPointToRay(new Vector3(_mainCamera.pixelWidth / 2, _mainCamera.pixelHeight / 2, 0f)), out hit, 100f))
        {
            targetedObject = hit.collider.gameObject.GetComponent<UnstableObject>();

            _laserHitPoint = hit.point;
        }

        UpdateTargetedObject(targetedObject);
    }

    private void UpdateTargetedObject(UnstableObject targetedObject)
    {
        if (_targetedObject != null) _targetedObject.Untarget();
        if (targetedObject != null)  targetedObject.Target();

        _targetedObject = targetedObject;

        UpdateSelectionArchs();
    }

    private void UpdateSelectionArchs()
    {
        foreach (SelectionArch arch in SelectionArchs)
        {
            if (_targetedObject == null)
            {
                arch.MeshRenderer.sharedMaterial = NeutralSelectionMaterial;
                continue;
            }

            switch(arch.Property)
            {
                case PropertyType.EMISSION:
                    arch.MeshRenderer.sharedMaterial = _targetedObject.Emissive ? OnSelectionMaterial : OffSelectionMaterial;
                    break;
                case PropertyType.GRAVITY:
                    arch.MeshRenderer.sharedMaterial = _targetedObject.Gravity ? OnSelectionMaterial : OffSelectionMaterial;
                    break;
                case PropertyType.MOVABLE:
                    arch.MeshRenderer.sharedMaterial = _targetedObject.Movable ? OnSelectionMaterial : OffSelectionMaterial;
                    break;
                case PropertyType.BUOYANT:
                    arch.MeshRenderer.sharedMaterial = _targetedObject.Buoyant ? OnSelectionMaterial : OffSelectionMaterial;
                    break;
                case PropertyType.FRICTION:
                    arch.MeshRenderer.sharedMaterial = _targetedObject.Friction ? OnSelectionMaterial : OffSelectionMaterial;
                    break;
            }
        }
    }

    public struct SelectionArch
    {
        public PropertyType Property;
        public MeshRenderer MeshRenderer;
    }
}