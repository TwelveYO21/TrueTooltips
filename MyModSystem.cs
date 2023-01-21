namespace TrueTooltips;

internal class MyModSystem : ModSystem
{
    public override void PostUpdateInput()
    {
        if(GetInstance<MyModConfig>()?.textPulse == false)
            mouseTextColor = byte.MaxValue;
    }

    public override void PreSaveAndQuit()
    {
        try
        {
            Directory.CreateDirectory(MyMod.directory);
            File.WriteAllText(MyMod.path, JsonSerializer.Serialize(MyGlobalItem.blackList));
        }
        catch
        {
            MyMod.Instance.Logger.Error("Failed to save blacklist");
            throw;
        }
    }
}
