using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Scenario.Data
{
    [CreateAssetMenu(fileName = "NewStep", menuName = "Scenario/Step")]
    public class StepSO : ScriptableObject
    {
        [SerializeField] private int id;
        [TextArea] [SerializeField] private string description;
        [SerializeField] private List<StepAction> expectedActions = new List<StepAction>();

        public int Id => id;
        public string Description => description;

        /// <summary>
        /// All actions the user must perform (in any order) to complete this step.
        /// Most steps have exactly one entry; steps that need several actions
        /// (e.g. "enter zone" + "press button") list all of them here.
        /// </summary>
        public IReadOnlyList<StepAction> ExpectedActions => expectedActions;
    }
}