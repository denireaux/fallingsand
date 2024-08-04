namespace FallingSand.ParticleTypes
{
    public abstract class Particle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Velocity { get; set; }

        protected Particle(int x, int y)
        {
            X = x;
            Y = y;
            Velocity = 0f;
        }

        public abstract void Update(float gravity, Particle[,] grid);
    }
}
