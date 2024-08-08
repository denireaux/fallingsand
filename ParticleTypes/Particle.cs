namespace FallingSand.ParticleTypes
{
    public abstract class Particle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Velocity { get; set; }
        public bool isHot { get; protected set; }
        public bool isCold { get; protected set; }

        protected Particle(int x, int y)
        {
            X = x;
            Y = y;
            Velocity = 0f;
            isHot = false;
            isCold = false;
        }

        public abstract void Update(float gravity, Particle[,] grid);

        public Particle[] GetSurroundingParticles(Particle[,] grid)
        {
            Particle left = (X > 0) ? grid[X - 1, Y] : null;
            Particle right = (X < grid.GetLength(0) - 1) ? grid[X + 1, Y] : null;
            Particle above = (Y > 0) ? grid[X, Y - 1] : null;
            Particle below = (Y < grid.GetLength(1) - 1) ? grid[X, Y + 1] : null;

            return new Particle[] { left, right, above, below };
        }
    }
}
