using System;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.Interactions
{
    public interface IInteractable
    {
        string TargetId { get; }
        ActionType ActionType { get; }
        
        event Action<IInteractable> OnInteracted;
    }
}