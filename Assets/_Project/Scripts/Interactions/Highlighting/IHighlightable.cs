namespace _Project.Scripts.Interactions.Highlighting
{
    /// <summary>
    /// Implemented by anything that can visually show "this is what the user
    /// should interact with right now"
    /// </summary>
    public interface IHighlightable
    {
        void SetHighlighted(bool isHighlighted);
    }
}