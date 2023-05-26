public class EggSpoiled : Egg
{
    public override void DoEffect(Player player)
    {
        base.DoEffect(player);
        player.DoIntoxicatedEffect();
    }
}
