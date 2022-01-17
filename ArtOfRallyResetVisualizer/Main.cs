using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static Settings Settings;
        // private static bool _patched = false;

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Settings = new Settings();
            modEntry.OnGUI = entry => Settings.Draw(entry);
            modEntry.OnSaveGUI = entry => Settings.Save(entry);

            return true;
        }
    }
}