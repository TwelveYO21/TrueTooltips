﻿namespace TrueTooltips
{
    using Microsoft.Xna.Framework;
    using Terraria;
    using Terraria.UI;

    class MyMod : Terraria.ModLoader.Mod
    {
        readonly UserInterface ui = new UserInterface();

        public override void Load() => ui.SetState(new MyUIState());

        public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(l => l.Name == "Vanilla: Mouse Text");

            if(index >= 0)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer("", () =>
                {
                    ui.Draw(Main.spriteBatch, new GameTime());

                    return true;
                }, InterfaceScaleType.UI));
            }
        }

        public override void UpdateUI(GameTime gameTime) => ui?.Update(gameTime);

        public override void PostUpdateInput()
        {
            if(Terraria.ModLoader.ModContent.GetInstance<Config>() != null)
                if(!Terraria.ModLoader.ModContent.GetInstance<Config>().textPulse)
                {
                    Main.cursorAlpha = 0.6f;
                    Main.mouseTextColor = 255;
                }
        }
    }
}
