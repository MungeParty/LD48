using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rooms
{
    Bridge,
    Rest,
    Security,
    Navigation,
    Engine,
    Server,
    Cargo,
    WarpEngine,
    Electrical,
    Kitchen,
    MedBay,
    AnyRoom
}

public enum ShipSystems
{
    Warp,
    Cargo,
    Hull,
    Power,
    Shields,
    Sensors,
    Gravity,
    LifeSupport
}

[System.Serializable]
public struct ShipSystemState
{
    private float _value;
    private float _lastValue;
    public ShipSystems system;

    public float baseValue
    {
        get
        {
            if (_value != _lastValue)
            {
                _value = Mathf.Clamp01(_value);
                _lastValue = _value;
            }
            return _value;
        }
        set
        {
            _value = Mathf.Clamp01(value);
            _lastValue = value;
        }
    }

    public ShipSystemState(ShipSystems _system, float _baseValue)
    {
        system = _system;
        _value = Mathf.Clamp01(_baseValue);
        _lastValue = _value;
    }

    public float value
    {
        get
        {
            float _base = baseValue;
            float value = _base;
            return value;
        }
    }

}

public class Ship : MonoBehaviour
{
    public ShipEnvironment environment;

    private void Start()
    {
        if (environment == null) environment = GetComponent<ShipEnvironment>();
    }
}