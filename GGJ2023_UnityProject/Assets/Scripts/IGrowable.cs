namespace LemonBerry
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IGrowable
    {
        public int GrowCost { get; }
        bool IsGrown { get; set; }
        int RemainingGrowCost { get; }
        int PendingWater { get; set; }
        Transform Transform { get; }
        void Grow();
        void UnGrow();
        void AddWater(WaterDroplet waterDroplet);
        List<WaterDroplet> RemoveWater();
    }
}