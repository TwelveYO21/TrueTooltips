namespace TrueTooltips;

internal class TTModSystem : ModSystem
{
    public override void PostUpdateInput()
    {
        if(GetInstance<Config>()?.TextPulse == false) mouseTextColor = byte.MaxValue;
    }
}
