using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class SoilParticle : Particle
    {
        public SoilParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            Velocity += gravity;
            int newY = (int)(Y + Velocity);

            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            if (newY < Game1.gridHeight)
            {
                grid[X, newY] = this;
                grid[X, Y] = null;
            }
        }
    }
}