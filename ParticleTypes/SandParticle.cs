using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class SandParticle : Particle
    {
        public SandParticle(int x, int y) : base(x, y)
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
                if (grid[X, newY] == null)
                {
                    grid[X, Y] = null;
                    Y = newY;
                    grid[X, Y] = this;
                }
                else if (grid[X, newY] is WaterParticle)
                {
                    // Transform into WetSandParticle when it comes into contact with water
                    grid[X, Y] = new WetSandParticle(X, Y);
                }
                else
                {
                    // Sliding and other behaviors for sand
                    Random rand = new Random();
                    int direction = rand.Next(0, 2) * 2 - 1;

                    if (X + direction >= 0 && X + direction < Game1.gridWidth && Y + 1 < Game1.gridHeight)
                    {
                        if (grid[X + direction, Y + 1] == null || grid[X + direction, Y + 1] is WaterParticle)
                        {
                            if (grid[X + direction, Y + 1] is WaterParticle)
                            {
                                grid[X, Y] = new WetSandParticle(X, Y);
                            }
                            else
                            {
                                grid[X, Y] = null;
                            }

                            X += direction;
                            Y += 1;
                            grid[X, Y] = this;
                        }
                    }
                }
            }
        }
    }
}
