public class Base : GameUnit
{
    public new void Die()
    {
        // display die animation
    }

    public new void TakeDmg(float dmg)
    {
        GameController.INSTANCE.OnBaseTakeDmg(this);
        base.TakeDmg(dmg);
    }
}
