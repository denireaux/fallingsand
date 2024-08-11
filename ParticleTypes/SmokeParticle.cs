using Microsoft.Xna.Framework;
using System;
using System.Data;
using System.Xml.Schema;

namespace FallingSand.ParticleTypes
{
    public class SmokeParticle : Particle
    {
        // Factor by which velocity is reduced upon bouncing
        private float energyLossFactor = 0.8f;

        public SmokeParticle(int x, int y) : base(x, y)
        {
            Velocity = -0.2f; // Very slow upward movement
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            Random rand = new Random();

            int newY = (int)(Y + Velocity);

            if (newY < 0)
            {
                newY = 0;

                int direction = rand.Next(0, 2) * 2 - 1;
                if (X + direction >= 0 && X + direction < Game1.gridWidth && grid[X + direction, Y] == null)
                {
                    grid[X, Y] = null;
                    X += direction;
                    grid[X, Y] = this;
                }
                else if (X - direction >= 0 && X - direction < Game1.gridWidth && grid[X - direction, Y] == null)
                {
                    grid[X, Y] = null;
                    X -= direction;
                    grid[X, Y] = this;
                }
            }
            else if (newY >= Game1.gridHeight)
            {
                newY = Game1.gridHeight - 1;
                Velocity = -Velocity * energyLossFactor;
            }

            if (grid[X, newY] == null)
            {
                grid[X, Y] = null;
                Y = newY;
                grid[X, Y] = this;
            }
            else if (grid[X, newY] is SmokeParticle)
            {
                int direction = rand.Next(0, 2) * 2 - 1;

                if (X + direction >= 0 && X + direction < Game1.gridWidth && grid[X + direction, Y] == null)
                {
                    grid[X, Y] = null;
                    X += direction;
                    grid[X, Y] = this;
                }
                else if (X - direction >= 0 && X - direction < Game1.gridWidth && grid[X - direction, Y] == null)
                {
                    grid[X, Y] = null;
                    X -= direction;
                    grid[X, Y] = this;
                }
            }

            Velocity *= 0.999f;
        }

    }
}
