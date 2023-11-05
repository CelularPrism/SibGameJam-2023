namespace Assets.Scripts.Fire
{
    public class Burning
    {
        public HealthSystem Health;
        public float Time;

        public Burning(HealthSystem health, float time = 1)
        {
            Health = health;
            Time = time;
        }
    }
}