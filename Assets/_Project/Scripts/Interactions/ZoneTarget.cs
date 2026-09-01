using UnityEngine;

namespace _Project.Scripts.Interactions
{
    [RequireComponent(typeof(Collider))]
    public class ZoneTarget : BaseInteractable
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TriggerInteraction();
            }
        }
    }
}