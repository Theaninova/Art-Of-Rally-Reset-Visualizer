using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static bool ShowNoGoZones;
        public static bool ShowWaypoints;

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label("Reset Visualizer");

            if (ShowNoGoZones != GUILayout.Toggle(ShowNoGoZones, "Reset Zones"))
            {
                ShowNoGoZones = !ShowNoGoZones;
                GameObject.Find(ResetVisualizer.NoGoVisualizersName)?.SetActive(ShowNoGoZones);
            }

            if (ShowWaypoints != GUILayout.Toggle(ShowWaypoints, "Waypoints"))
            {
                ShowWaypoints = !ShowWaypoints;
                GameObject.Find(ResetVisualizer.WaypointVisualizersName)?.SetActive(ShowWaypoints);
            }
        }
    }
}