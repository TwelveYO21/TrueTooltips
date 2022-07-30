global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using System.Collections.Generic;
global using System.IO;
global using System.Text.Json;
global using Terraria;
global using Terraria.GameContent;
global using Terraria.ModLoader;
global using static Terraria.Main;
global using static Terraria.ModLoader.ModContent;

namespace TrueTooltips;

internal class TTMod : Mod
{
    internal static readonly string directory = Path.Combine(SavePath, "TrueTooltips");
    internal static readonly string path = Path.Combine(directory, "blackList.json");

    internal static Mod Instance { get; private set; }
    internal static ModKeybind ToggleTooltipKey { get; private set; }

    public override void Load()
    {
        Instance = this;
        if(File.Exists(path))
            try
            {
                TTGlobalItem.blackList = JsonSerializer.Deserialize<List<int>>(File.ReadAllText(path)) ?? new List<int>();
            }
            catch
            {
                Logger.Error("Failed to load blacklist");
                throw;
            }
        ToggleTooltipKey = KeybindLoader.RegisterKeybind(this, "Toggle Tooltip", Microsoft.Xna.Framework.Input.Keys.T);
        SettingsEnabled_OpaqueBoxBehindTooltips = false;
    }

    public override void Unload() => ToggleTooltipKey = null;
}
