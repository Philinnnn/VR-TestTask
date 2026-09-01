using System;
using _Project.Scripts.Scenario.Data;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string targetId;
        [SerializeField] private ActionType actionType;

        public string TargetId => targetId;
        public ActionType ActionType => actionType;

        public event Action<IInteractable> OnInteracted;

        protected void TriggerInteraction()
        {
            OnInteracted?.Invoke(this);
        }
    }
}