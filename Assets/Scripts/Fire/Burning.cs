namespace Assets.Scripts.Fire
{
    public class Burning
    {
        public HealthSystem Health;
        public float Time;

        public Burning(HealthSystem health, float time = 0)
        {
            Health = health;
            Time = time;
        }
    }
}