using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class StoneParticle : Particle
    {
        public StoneParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
        }

        public override void Update(float gravity, Particle[,] grid) 
        {
            {
            // Apply gravity to the velocity, but slower than dry sand
            Velocity += gravity * 0.5f;
            int newY = (int)(Y + Velocity);

            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            if (newY < Game1.gridHeight)
                {
                    if (grid[X, newY] == null || grid[X, newY] is WaterParticle)
                    {
                        // Swap positions with water or move down if empty
                        if (grid[X, newY] is WaterParticle)
                        {
                            // Swap positions
                            grid[X, Y] = grid[X, newY];
                            grid[X, newY].X = X;
                            grid[X, newY].Y = Y;
                        }
                        else
                        {
                            grid[X, Y] = null;
                        }

                        Y = newY;
                        grid[X, Y] = this;
                    }
                    else
                    {
                        // Try sliding diagonally
                        Random rand = new Random();
                        int direction = rand.Next(0, 2) * 2 - 1;

                        if (X + direction >= 0 && X + direction < Game1.gridWidth && Y + 1 < Game1.gridHeight)
                        {
                            if (grid[X + direction, Y + 1] == null || grid[X + direction, Y + 1] is WaterParticle)
                            {
                                if (grid[X + direction, Y + 1] is WaterParticle)
                                {
                                    // Swap positions with water
                                    grid[X, Y] = grid[X + direction, Y + 1];
                                    grid[X + direction, Y + 1].X = X;
                                    grid[X + direction, Y + 1].Y = Y;
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
        public override void MoveSelf(Particle[,] grid, int newX, int newY) { return; }
    }
}
