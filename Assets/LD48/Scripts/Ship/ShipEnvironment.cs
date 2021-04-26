using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnvironment : MonoBehaviour
{
    // non-100% starting systems:
    private static Dictionary<ShipSystems, float> systemDefaults =
        new Dictionary<ShipSystems, float>() {
            { ShipSystems.Warp, 0f },
            { ShipSystems.Cargo, 0f }
        };

    public static float fullPowerGravityForce = 120;

    public List<ShipSystemState> systems = new List<ShipSystemState>();

    public Dictionary<ShipSystems, int> systemLookup = new Dictionary<ShipSystems, int>();

    private ShipEnvironment parentEnvironment;

    private void Start()
    {
        // look for parent
        if (transform.parent != null && parentEnvironment == null)
            parentEnvironment = GetComponentInParent<ShipEnvironment>();

        // register all systems
        Array systemsTypes = Enum.GetValues(typeof(ShipSystems));
        foreach (ShipSystems systemType in systemsTypes)
        {
            float baseValue = systemDefaults.ContainsKey(systemType)
                ? systemDefaults[systemType] : 1f;
            systems.Add(new ShipSystemState(systemType, baseValue));
            systemLookup[systemType] = systems.Count - 1;
        }
    }

    /// <summary>
    /// warp charge, starts at 0f
    /// </summary>
    public float warpPercent
    {
        get
        {
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.warpPercent;
            return inherited * systems[systemLookup[ShipSystems.Warp]].value;
        }
    }

    /// <summary>
    /// cargo capacity, starts at 0f
    /// </summary>
    public float cargoPercent
    {
        get
        {
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.cargoPercent;
            return inherited * systems[systemLookup[ShipSystems.Cargo]].value;
        }
    }

    /// <summary>
    /// hull percent
    /// </summary>
    public float hullPercent
    {
        get
        {
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.hullPercent;
            return inherited * systems[systemLookup[ShipSystems.Hull]].value;
        }
    }

    /// <summary>
    /// power bank charge level, starts 1f
    /// </summary>
    public float powerPercent
    {
        get
        {
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.powerPercent;
            return inherited * systems[systemLookup[ShipSystems.Power]].value;
        }
    }

    /// <summary>
    /// shields percent
    /// </summary>
    public float shiledsPercent
    {
        get
        {
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.shiledsPercent;
            return inherited * systems[systemLookup[ShipSystems.Shields]].value;
        }
    }

    /// <summary>
    /// sensors percent
    /// </summary>
    public float sensorsPercent
    {
        get
        {
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.sensorsPercent;
            return inherited * systems[systemLookup[ShipSystems.Sensors]].value;
        }
    }

    /// <summary>
    /// gravity system percent
    /// </summary>
    public float gravityPercent
    {
        get
        {
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.gravityPercent;
            return inherited * systems[systemLookup[ShipSystems.Gravity]].value;
        }
    }

    /// <summary>
    /// life support system percent
    /// </summary>
    public float lifeSupportPercent
    {
        get
        {
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.lifeSupportPercent;
            return inherited * systems[systemLookup[ShipSystems.LifeSupport]].value;
        }
    }

    /// <summary>
    /// whether or not any power is available
    /// </summary>
    public bool isPowered
    {
        get
        {
            return powerPercent > 0f;
        }
    }

    /// <summary>
    /// current gravity force
    /// </summary>
    public float gravityForce
    {
        get
        {
            //return fullPowerGravityForce;
            return gravityPercent == 0f ? 0f : (0.5f + 0.5f * gravityPercent) * fullPowerGravityForce;
        }
    }
}
