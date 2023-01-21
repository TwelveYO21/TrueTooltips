namespace TrueTooltips;

internal class MyModPlayer : ModPlayer
{
    public override void ProcessTriggers(Terraria.GameInput.TriggersSet triggersSet)
    {
        if(MyMod.ToggleTooltipKey.JustPressed && HoverItem.type != 0)
            MyGlobalItem.ToggleTooltip(HoverItem.type);
    }
}
