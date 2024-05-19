namespace Assets.Scripts.Environment.Traps.Aoe
{
    public class AoeEntity
    {
        public float CurrentTiming;
        public string Name;
        public ICanBeDamaged Entity;

        public AoeEntity()
        {
        }

        public AoeEntity(string name, ICanBeDamaged entity, float tickCD)
        {
            Name = name;
            Entity = entity;
            CurrentTiming = tickCD;
        }
    }
}
