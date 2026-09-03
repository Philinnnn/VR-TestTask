using UnityEngine;

namespace _Project.Scripts.Interactions
{
    [RequireComponent(typeof(Collider))]
    public class ZoneTarget : BaseInteractable
    {
        private bool _playerInside;

        private void OnTriggerEnter(Collider other)
        {
            if (_playerInside) return;
            if (!other.CompareTag("Player")) return;

            _playerInside = true;
            TriggerInteraction();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                _playerInside = false;
        }
    }
}