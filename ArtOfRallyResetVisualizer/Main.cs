using System.Reflection;
using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static ResetVisualizerSettings ResetVisualizerSettings;

        public static UnityModManager.ModEntry.ModLogger Logger;

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            ResetVisualizerSettings = UnityModManager.ModSettings.Load<ResetVisualizerSettings>(modEntry);
            modEntry.OnGUI = entry => ResetVisualizerSettings.Draw(entry);
            modEntry.OnSaveGUI = entry => ResetVisualizerSettings.Save(entry);
            modEntry.OnFixedGUI = LeaderboardDisabledNotice.Draw;
            ResetVisualizer.IsLeaderboardDisabled = ResetVisualizerSettings.RenderMode == RenderMode.Always;

            return true;
        }
    }
}