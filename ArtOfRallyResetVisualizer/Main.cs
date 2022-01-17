﻿using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static Settings Settings;
        private static bool _patched = false;

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            SceneManager.sceneLoaded += (arg0, mode) =>
            {
                if (_patched) return;
                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                _patched = true;
            };
                
            Settings = new Settings();
            modEntry.OnGUI = entry => Settings.Draw(entry);
            modEntry.OnSaveGUI = entry => Settings.Save(entry);

            return true;
        }
    }
}