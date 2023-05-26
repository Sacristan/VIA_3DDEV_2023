public class EggExplosive : Egg
{
    public override void DoEffect(Player player)
    {
        base.DoEffect(player);
        player.DoExplosionEffect();
    }
}
