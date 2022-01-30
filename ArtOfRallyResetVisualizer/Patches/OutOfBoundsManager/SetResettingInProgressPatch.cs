using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;

namespace ArtOfRallyResetVisualizer.Patches.OutOfBoundsManager
{
    [HarmonyPatch(typeof(global::OutOfBoundsManager), "FixedUpdate")]
    public class SetResettingInProgressPatch
    {
        private static bool _isResettingInProgress;

        // ReSharper disable once InconsistentNaming
        public static void Prefix(bool ___isResettingInProgress)
        {
            if (Main.ResetVisualizerSettings.RenderMode != RenderMode.OnHit ||
                _isResettingInProgress == ___isResettingInProgress) return;
            _isResettingInProgress = ___isResettingInProgress;
            ResetVisualizer.UpdateAllComponents(_isResettingInProgress);
        }
    }
}