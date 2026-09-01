using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Scenario.Data
{
    [CreateAssetMenu(fileName = "NewStepGroup", menuName = "Scenario/Step Group")]
    public class StepGroupSO : ScriptableObject
    {
        [SerializeField] private string groupName;
        [SerializeField] private List<StepSO> steps = new List<StepSO>();

        public string GroupName => groupName;
        public IReadOnlyList<StepSO> Steps => steps;
    }
}