using System.Drawing;
using ArtOfRallyResetVisualizer.Settings;
using UnityEngine;
using UnityModManagerNet;
using Color = UnityEngine.Color;
using FontStyle = UnityEngine.FontStyle;

namespace ArtOfRallyResetVisualizer
{
    public static class LeaderboardDisabledNotice
    {
        private static GUIStyle _leaderBoardDisabledStyle;
        
        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            if (!ResetVisualizer.IsLeaderboardDisabled && Main.Settings.RenderMode != RenderMode.Always) return;

            if (_leaderBoardDisabledStyle == null)
            {
                _leaderBoardDisabledStyle = new GUIStyle
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 48,
                    padding = new RectOffset(8, 8, 8, 8),
                    normal =
                    {
                        textColor = Color.red,
                        background = Texture2D.grayTexture
                    }
                };
            }
                
            GUI.Label(Rect.zero, Main.Settings.RenderMode == RenderMode.Always
                    ? ResetVisualizer.IsLeaderboardDisabled
                        ? "LEADERBOARD DISABLED"
                        : "Start new game to enable visualizers"
                    : "LEADERBOARD DISABLED, START NEW GAME TO RE-ENABLE",
                _leaderBoardDisabledStyle
            );
        }
    }
}