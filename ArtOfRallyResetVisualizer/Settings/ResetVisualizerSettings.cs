using UnityEngine;
using UnityModManagerNet;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global

namespace ArtOfRallyResetVisualizer.Settings
{
    public enum RenderMode
    {
        Off,
        OnHit,
        Always
    }

    public enum HardResetMode
    {
        Intersect,
        Distance
    }

    public enum HardResetDistanceCompensation
    {
        None,
        Car,
        Static
    }

    public class ResetVisualizerSettings : UnityModManager.ModSettings, IDrawable
    {
        [Header("Modes")] [Draw(DrawType.ToggleGroup)]
        public RenderMode RenderMode = RenderMode.Off;

        [Header("Visualizers")] [Draw("Reset Zones")]
        public bool ShowResetZones = true;

        [Draw("Waypoints")] public bool ShowWaypoints = true;

        [Header("Colors")] [Draw("Reset Color")]
        public Color ResetColor = new Color(1f, 0f, 0f, 0.7f);

        [Draw("Waypoint Color")] public Color WaypointColor = new Color(1f, 0.8f, 0.8f, 0.1f);

        [Header("Hard Reset Mode (Anything but Intersect disables leaderboards)")] [Draw(DrawType.ToggleGroup)]
        public HardResetMode HardResetMode = HardResetMode.Intersect;

        [Header("Hard Reset Distance Compensation")] [Draw(DrawType.ToggleGroup)]
        public HardResetDistanceCompensation HardResetDistanceCompensation = HardResetDistanceCompensation.None;

        [Draw("Manual Distance Compensation", VisibleOn = "HardResetDistanceCompensation|Static")]
        public float ManualDistanceCompensation = 1.0f;

        [Draw("Actual Reset Color")] public Color ActualResetColor = new Color(1f, 0.78f, 0f, 0.1f);

        public void OnChange()
        {
            ResetVisualizer.UpdateAllComponents();
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}