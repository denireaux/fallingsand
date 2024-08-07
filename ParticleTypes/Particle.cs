namespace FallingSand.ParticleTypes
{
    public abstract class Particle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Velocity { get; set; }
        public bool isHot { get; protected set; }

        protected Particle(int x, int y)
        {
            X = x;
            Y = y;
            Velocity = 0f;
            isHot = false;
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
/* 
        public bool IsInactive()
        {
            // A particle is considered inactive if it reaches the top or bottom of the grid
            // or if it is stuck and not moving (optional condition).
            return Y == 0 || Y >= Game1.gridHeight;
        } */
    }
}
