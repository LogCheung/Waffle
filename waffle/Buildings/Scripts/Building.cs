using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Building : ProductionObject
{
    protected Dictionary<Colony.ResourceType,int> storageDict = new Dictionary<Colony.ResourceType, int>();

    public void AddResourceToInventory(Colony.ResourceType type, int amt)
    {
        storageDict[type] += amt;
    }
}   
