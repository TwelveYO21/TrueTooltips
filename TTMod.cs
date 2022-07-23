global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using System.Collections.Generic;
global using Terraria;
global using Terraria.GameContent;
global using Terraria.ModLoader;
global using static Terraria.Main;
global using static Terraria.ModLoader.ModContent;

namespace TrueTooltips;

internal class TTMod : Mod
{
    public override void Load() => SettingsEnabled_OpaqueBoxBehindTooltips = false;
}
