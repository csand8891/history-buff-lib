using System;
using System.Collections.Generic;


namespace HistoryBuff.Models
{
    public class HistoryRecord
    {
        DateTime Date { get; set; }
        string OldRevisionNumber { get; set; }
        string NewRevisionNumber { get; set; }
        List<HistoryLine> Lines { get; set; }
        List<SpecCodeChange> SpecCodeChanges { get; set; }
    
        
    }
}