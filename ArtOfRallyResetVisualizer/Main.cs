using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static ResetVisualizerSettings ResetVisualizerSettings;
        // private static bool _patched = false;

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            ResetVisualizerSettings = UnityModManager.ModSettings.Load<ResetVisualizerSettings>(modEntry);
            modEntry.OnGUI = entry => ResetVisualizerSettings.Draw(entry);
            modEntry.OnSaveGUI = entry => ResetVisualizerSettings.Save(entry);

            return true;
        }
    }
}