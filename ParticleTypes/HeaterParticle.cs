using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class HeaterParticle : Particle
    {
        public HeaterParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
            isHot = true;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            // Ensure the heater particle stays in place
            if (grid[X, Y] != this)
            {
                grid[X, Y] = this;
            }
        }
        public override void MoveSelf(Particle[,] grid, int newX, int newY) {}

    }
}
