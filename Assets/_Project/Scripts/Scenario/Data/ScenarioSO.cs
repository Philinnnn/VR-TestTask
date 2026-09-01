using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Scenario.Data
{
    [CreateAssetMenu(fileName = "NewScenario", menuName = "Scenario/Scenario Data")]
    public class ScenarioSO : ScriptableObject
    {
        [SerializeField] private List<StepGroupSO> stepGroups = new List<StepGroupSO>();

        public IReadOnlyList<StepGroupSO> StepGroups => stepGroups;
    }
}