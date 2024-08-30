using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class CoolerParticle : Particle
    {
        public CoolerParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
            isHot = false;
            isCold = true;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            // Ensure the cooler particle stays in place
            if (grid[X, Y] != this)
            {
                grid[X, Y] = this;
            }
        }
        public override void MoveSelf(Particle[,] grid, int newX, int newY) { return; }
    }
}
