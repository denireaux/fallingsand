using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class WaterParticle : Particle
    {
        public WaterParticle(int x, int y) : base(x, y)
        {
            Velocity = 0.1f;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            Velocity += gravity * 0.5f;
            int newY = (int)(Y + Velocity);

            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            // Check all surrounding spaces for particle.isHot == true, and convert water to smoke if so
            if (HeatDetected(grid))
            {
                EmitVapor(grid);
                return; // Exit early since the water particle is now smoke
            }

            // Handle interactions with particles below
            if (newY < Game1.gridHeight)
            {
                HandleVerticalMovement(grid, newY);

                // Handle lateral sliding
                HandleLateralMovement(grid);
            }
        }

        private void HandleVerticalMovement(Particle[,] grid, int newY)
        {
            Particle belowParticle = grid[X, newY];

            if (belowParticle == null)
            {
                MoveParticle(grid, X, newY);
            }
            else if (belowParticle is SandParticle)
            {
                grid[X, newY] = new WetSandParticle(X, newY);
                grid[X, Y] = null;
            }
            else if (belowParticle is FireParticle)
            {
                EmitVapor(grid);
            }
            else if (belowParticle is LavaParticle)
            {
                grid[X, newY] = new StoneParticle(X, newY);
                EmitVapor(grid);
            }
        }

        private void HandleLateralMovement(Particle[,] grid)
        {
            Random rand = new Random();
            int direction = rand.Next(0, 2) * 2 - 1;

            // Check if the water can slide diagonally down-left or down-right
            if (X + direction >= 0 && X + direction < Game1.gridWidth && Y + 1 < Game1.gridHeight)
            {
                Particle diagonalParticle = grid[X + direction, Y + 1];
                Particle sideParticle = grid[X + direction, Y];

                if (diagonalParticle == null)
                {
                    MoveParticle(grid, X + direction, Y + 1);
                }
                else if (diagonalParticle is SandParticle)
                {
                    grid[X + direction, Y + 1] = new WetSandParticle(X + direction, Y + 1);
                    grid[X, Y] = null;
                }
                else if (diagonalParticle is FireParticle)
                {
                    EmitVapor(grid);
                    grid[X + direction, Y + 1] = null; // Remove the fire particle
                }
                else if (sideParticle == null)
                {
                    MoveParticle(grid, X + direction, Y);
                }
            }
        }

        // Check if there is a FireParticle Left, Right, Above, or Below
        private bool HeatDetected(Particle[,] grid)
        {
            Particle[] particlesNear = GetSurroundingParticles(grid);

            foreach (Particle particle in particlesNear)
            {
                if (particle != null && particle.isHot)
                {
                    return true;
                }
            }
            return false;
        }

        // Move the particle to a new location
        private void MoveParticle(Particle[,] grid, int newX, int newY)
        {
            grid[X, Y] = null;
            X = newX;
            Y = newY;
            grid[X, Y] = this;
        }

        // Turn WaterParticle into VaporParticle
        private void EmitVapor(Particle[,] grid)
        {
            grid[X, Y] = new VaporParticle(X, Y);
        }
    }
}
