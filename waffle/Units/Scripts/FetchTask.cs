using Godot;
using System;

public partial class FetchTask : Task
{
    // source building wants this resource
    // target will be storage building pos or ground pos (if harvested)
    // task will also handle returning resource to source building
    public FetchTask(ProductionObject source, Type type) : base(source,type)
    {
        
    }
}