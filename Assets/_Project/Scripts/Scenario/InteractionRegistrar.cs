using System;
using System.Collections.Generic;
using _Project.Scripts.Interactions;
using UnityEngine;

namespace _Project.Scripts.Scenario
{
    /// <summary>
    /// This class is responsible for registering all interactable objects
    /// in the scene and notifying when any interaction occurs.
    /// </summary>
    public class InteractionRegistrar : MonoBehaviour
    {
        public event Action<IInteractable> OnAnyInteraction;
        
        private readonly HashSet<IInteractable> _registeredInteractables = new HashSet<IInteractable>();

        private void Start()
        {
            var monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var mb in monoBehaviours)
            {
                if (mb is IInteractable interactable)
                {
                    Register(interactable);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var interactable in _registeredInteractables)
            {
                if (interactable is UnityEngine.Object unityObj && unityObj != null)
                {
                    interactable.OnInteracted -= HandleInteraction;
                }
            }
            _registeredInteractables.Clear();
        }
        
        /// <summary>
        /// Method to register dynamically created interactable objects
        /// </summary>
        /// <param name="interactable"></param>
        public void Register(IInteractable interactable)
        {
            if (interactable == null || _registeredInteractables.Contains(interactable)) 
                return;

            _registeredInteractables.Add(interactable);
            interactable.OnInteracted += HandleInteraction;
        }

        /// <summary>
        /// Method to unregister interactable objects
        /// </summary>
        /// <param name="interactable"></param>
        public void Unregister(IInteractable interactable)
        {
            if (interactable == null || !_registeredInteractables.Contains(interactable)) 
                return;

            interactable.OnInteracted -= HandleInteraction;
            _registeredInteractables.Remove(interactable);
        }

        private void HandleInteraction(IInteractable interactable)
        {
            OnAnyInteraction?.Invoke(interactable);
        }
    }
}