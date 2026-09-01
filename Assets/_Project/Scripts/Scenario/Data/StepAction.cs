using System;
using UnityEngine;

namespace _Project.Scripts.Scenario.Data
{
    /// <summary>
    /// A single expected user action within a step.
    /// A Step can require one action, or several (in any order) —
    /// e.g. "walk into the zone" + "press the button" — before it's considered done.
    /// </summary>
    [Serializable]
    public class StepAction
    {
        [SerializeField] private ActionType actionType;
        [SerializeField] private string targetId;

        public ActionType ActionType => actionType;
        public string TargetId => targetId;
    }
}