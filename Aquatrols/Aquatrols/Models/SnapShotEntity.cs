using System;
using System.Collections.Generic;
using System.Text;
namespace Aquatrols.Models
{
    public class SnapShotEntity
    {
        
    }
    public class TmSnapshotSignUpCountVm
    {
        public string territoryNumber { get; set; }
        public string territoryName { get; set; }
        public int countSignUps { get; set; }
        public int countCommitments { get; set; }
    }

    public class SnapShotCountVm
    {
        public List<TmSnapshotSignUpCountVm> tmSnapshotSignUpCountVm { get; set; }
    }
}
