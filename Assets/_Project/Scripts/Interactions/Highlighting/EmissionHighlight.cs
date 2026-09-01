using UnityEngine;

namespace _Project.Scripts.Interactions.Highlighting
{
    [RequireComponent(typeof(Renderer))]
    public class EmissionHighlight : MonoBehaviour, IHighlightable
    {
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private float intensity = 2f;

        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private Renderer[] _renderers;
        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _propertyBlock = new MaterialPropertyBlock();
            
            foreach (var rend in _renderers)
            {
                if (rend.sharedMaterial != null)
                {
                    rend.sharedMaterial.EnableKeyword("_EMISSION");
                }
            }
        }

        public void SetHighlighted(bool isHighlighted)
        {
            var color = isHighlighted ? highlightColor * intensity : Color.black;

            foreach (var rend in _renderers)
            {
                rend.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor(EmissionColorId, color);
                rend.SetPropertyBlock(_propertyBlock);
            }
        }
    }
}