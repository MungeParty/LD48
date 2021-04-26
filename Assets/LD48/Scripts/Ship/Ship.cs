using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float SystemReadModifier(float baseValue, float modifiedValue);
public delegate float SystemEndTurnModifier(float baseValue, float modifiedValue);

public struct SystemModifier
{
    public SystemReadModifier onRead;
    public SystemEndTurnModifier onEndTurn;

    public SystemModifier(SystemReadModifier read, SystemEndTurnModifier turn)
    {
        onRead = read;
        onEndTurn = turn;
    }

    public float Read(float baseValue, float modifiedValue)
    {
        if (onRead == null) return modifiedValue;
        return onRead(baseValue, modifiedValue);
    }

    public float EndTurn(float oldValue, float modifiedValue)
    {
        if (onEndTurn == null) return modifiedValue;
        return onEndTurn(oldValue, modifiedValue);
    }
}

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

    public ShipSystemState(ShipSystems _system, float _baseValue, List<SystemModifier> _modifiers = null)
    {
        system = _system;
        _value = Mathf.Clamp01(_baseValue);
        _lastValue = _value;
        //modifiers = _modifiers != null ? _modifiers : new List<SystemModifier>();
    }

    public float value
    {
        get
        {
            float _base = baseValue;
            float value = _base;
            //foreach (SystemModifier modifier in modifiers)
            //    value = Mathf.Clamp01(modifier.Read(_base, value));
            return value;
        }
    }

    public void EndTurn()
    {
        //float _base = baseValue;
        //float value = _base;
        //foreach (SystemModifier modifier in modifiers)
            //value = Mathf.Clamp01(modifier.EndTurn(_base, value));
    }
}

public class Ship : MonoBehaviour
{
    //public ShipEnvironment environment;

    private void Start()
    {
        //environment = GetComponent<ShipEnvironment>();
    }
}