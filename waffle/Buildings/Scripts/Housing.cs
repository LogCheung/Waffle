using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Housing : Building
{
    [Export] public int housingCapacity {get;set;}
    public List<Citizen> residents {get;set;}

    private PopulationManager populationManager;

    public override void _Ready()
    {
        populationManager = (PopulationManager) GetTree().GetFirstNodeInGroup("POPULATIONMANAGER");
        populationManager.households.Add(this);

        TakeInCitizens();
    }

    private void TakeInCitizens()
    {
        List<Citizen> citizens = populationManager.citizens;
        citizens = citizens.Where(c => c.home == null).ToList();
        
        foreach (Citizen resident in citizens)
        {
            if (residents.Count >= housingCapacity) return;
            
            residents.Add(resident);
            resident.home = this;
        }
    }

}
