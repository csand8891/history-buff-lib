namespace HistoryBuff.Models
{
    public class SpecCodeChange
    {
        /// <summary>
        /// The type of change.
        /// </summary>
        public SpecChangeIndicator ChangeType { get; set; }

        /// <summary>
        /// The section of the spec code that was changed.
        /// </summary>
        public string SpecCodeSection { get; set; }

        /// <summary>
        /// The spec number.
        /// </summary>
        public string SpecNumber { get; set; }

        /// <summary>
        /// The spec bit.
        /// </summary>
        public string SpecBit { get; set; }

        /// <summary>
        /// The name of the spec.
        /// </summary>
        public string SpecName { get; set; }

        /// <summary>
        /// The old value.
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// The new value.
        /// </summary>
        public string NewValue { get; set; }
    }
}