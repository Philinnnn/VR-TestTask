using UnityEngine;

namespace _Project.Scripts.Scenario.Data
{
    [CreateAssetMenu(fileName = "NewStep", menuName = "Scenario/Step")]
    public class StepSO : ScriptableObject
    {
        [SerializeField] private int id;
        [TextArea] [SerializeField] private string description;
        [SerializeField] private ActionType expectedAction;
        [SerializeField] private string targetId;

        public int Id => id;
        public string Description => description;
        public ActionType ExpectedAction => expectedAction;
        public string TargetId => targetId;
    }
}