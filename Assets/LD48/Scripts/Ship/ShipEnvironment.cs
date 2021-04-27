using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnvironment : MonoBehaviour
{
    public static float fullPowerGravityForce = 120;

    // non-100% starting systems:
    private static Dictionary<ShipSystems, float> systemDefaults =
        new Dictionary<ShipSystems, float>() {
            { ShipSystems.Warp, 0f },
            { ShipSystems.Cargo, 0f }
        };

    [SerializeField]
    public Dictionary<ShipSystems, ShipSystemState> systems = new Dictionary<ShipSystems, ShipSystemState>();

    private ShipEnvironment parentEnvironment;

    private void Start()
    {
        // look for parent
        if (transform.parent != null && parentEnvironment == null)
            parentEnvironment = transform.parent.GetComponent<ShipEnvironment>();

        // register all systems
        Array systemsTypes = Enum.GetValues(typeof(ShipSystems));
        foreach (ShipSystems systemType in systemsTypes)
        {
            float baseValue = systemDefaults.ContainsKey(systemType)
                ? systemDefaults[systemType] : 1f;
            systems[systemType] = new ShipSystemState(systemType, baseValue);
        }
    }

    public bool isInitialized
    {
        get
        {
            return systems.Count > 0;
        }
    }

    public float warpPercent
    {
        get
        {
            if (!isInitialized) return 1f;
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.warpPercent;
            return inherited * systems[ShipSystems.Warp].value;
        }
    }

    public float cargoPercent
    {
        get
        {
            if (!isInitialized) return 1f;
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.cargoPercent;
            return inherited * systems[ShipSystems.Cargo].value;
        }
    }

    public float hullPercent
    {
        get
        {
            if (!isInitialized) return 1f;
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.hullPercent;
            return inherited * systems[ShipSystems.Hull].value;
        }
    }

    public float powerPercent
    {
        get
        {
            if (!isInitialized) return 1f;
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.powerPercent;
            return inherited * systems[ShipSystems.Power].value;
        }
    }

    public float shiledsPercent
    {
        get
        {
            if (!isInitialized) return 1f;
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.shiledsPercent;
            return inherited * systems[ShipSystems.Shields].value;
        }
    }

    public float sensorsPercent
    {
        get
        {
            if (!isInitialized) return 1f;
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.sensorsPercent;
            return inherited * systems[ShipSystems.Sensors].value;
        }
    }

    public float gravityPercent
    {
        get
        {
            if (!isInitialized) return 1f;
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.gravityPercent;
            return inherited * systems[ShipSystems.Gravity].value;
        }
    }

    public float lifeSupportPercent
    {
        get
        {
            if (!isInitialized) return 1f;
            float inherited = 1f;
            if (parentEnvironment != null)
                inherited = parentEnvironment.lifeSupportPercent;
            return inherited * systems[ShipSystems.LifeSupport].value;
        }
    }

    public bool isPowered
    {
        get
        {
            return powerPercent > 0f;
        }
    }

    public float gravityForce
    {
        get
        {
            return gravityPercent == 0f ? 0f : (0.5f + 0.5f * gravityPercent) * fullPowerGravityForce;
        }
    }
}
