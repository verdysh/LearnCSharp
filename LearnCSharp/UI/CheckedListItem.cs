namespace LearnCSharp.UI
{
    public class CheckedListItem<TItem>
    {
        public bool IsChecked { get; set; }
        public TItem Item { get; set; }
    }
}
