namespace TrueTooltips;

internal class TTModSystem : ModSystem
{
    public override void PostUpdateInput()
    {
        if(GetInstance<Config>()?.TextPulse == false)
            mouseTextColor = byte.MaxValue;
    }

    public override void PreSaveAndQuit()
    {
        try
        {
            Directory.CreateDirectory(TTMod.directory);
            File.WriteAllText(TTMod.path, JsonSerializer.Serialize(TTGlobalItem.blackList));
        }
        catch
        {
            TTMod.Instance.Logger.Error("Failed to save blacklist");
            throw;
        }
    }
}
