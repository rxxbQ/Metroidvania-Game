using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Status
{
    [SerializeField]
    private StatusBar bar;

    [SerializeField]
    private float maxValue;

    public float MaxValue
    {
        get
        {
            return maxValue;
        }
        set
        {
            this.maxValue = value;
            bar.MaxBarValue = maxValue;
        }
    }

    [SerializeField]
    private float currentValue;

    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set 
        {
            this.currentValue = Mathf.Clamp(value, 0, MaxValue);
            bar.Value = currentValue;
        }
    }

    public void Initialize()
    {
        this.MaxValue = maxValue;
        this.CurrentValue = currentValue;
    }
}
