using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bank
{
    private static int resource;

    public static event EventHandler onResourceAmtChange;

    public static void AddResource(int amt)
    {
        resource += amt;
        if (onResourceAmtChange != null) onResourceAmtChange(null, EventArgs.Empty);
    }

    public static int GetResourceAmt()
    {
        return resource;
    }
}
