namespace HistoryBuff.Models
{
    public class SpecCodeChange
    {
        SpecChangeIndicator ChangeType { get; set; }
        string SpecCodeSection { get; set; }
        string SpecNumber { get; set; }
        string SpecBit { get; set; }
        string SpecName { get; set; }
        string OldValue { get; set; }
        string NewValue { get; set; }
    }
}