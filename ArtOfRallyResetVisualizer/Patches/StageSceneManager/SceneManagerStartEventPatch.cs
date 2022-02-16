using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;

namespace ArtOfRallyResetVisualizer.Patches.StageSceneManager
{
    [HarmonyPatch(typeof(global::StageSceneManager), nameof(global::StageSceneManager.StartEvent))]
    public class SceneManagerStartEventPatch
    {
        public static void Prefix()
        {
            ResetVisualizer.HardResetMode = Main.Settings.HardResetMode;
            ResetVisualizer.IsLeaderboardDisabled = Main.Settings.RenderMode == RenderMode.Always
                                                    || ResetVisualizer.HardResetMode != HardResetMode.Intersect;
        }
    }
}