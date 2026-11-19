using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Housing : Building
{
    [Export] public int housingCapacity {get;set;}
    public List<Citizen> residents {get;set;}


    public override void _Ready()
    {
        PopulationManager.households.Add(this);

        TakeInCitizens();
    }
    
    private void TakeInCitizens()
    {
        List<Citizen> citizens = PopulationManager.citizens;
        citizens = citizens.Where(c => c.home == null).ToList();
        
        foreach (Citizen resident in citizens)
        {
            if (residents.Count >= housingCapacity) return;
            
            residents.Add(resident);
            resident.home = this;
        }
    }
    

}
