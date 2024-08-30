using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class AcidParticle : Particle
    {
        private static readonly Random rand = new Random();

        public AcidParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
            isHot = false;
            isCold = false;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            Velocity += gravity;
            int newY = (int)(Y + Velocity);

            // Try to move down first
            MoveSelf(grid, X, Y + 1);
        }

        public override void MoveSelf(Particle[,] grid, int newX, int newY)
        {
            // Boundary check to ensure we're not going out of grid bounds
            if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1))
            {
                return;
            }

            // Take our particles nearby [left, right, above, below]
            Particle[] particlesNear = GetSurroundingParticles(grid);

            // Isolate our left, right, and below
            Particle particleLeft = particlesNear[0];
            Particle particleRight = particlesNear[1];
            Particle particleBelow = particlesNear[3];

            // Check the space directly below
            if (particleBelow == null)
            {
                // Move downwards
                grid[newX, newY] = this;
                grid[X, Y] = null;
                X = newX;
                Y = newY;
            }
            // Check if the particle can move diagonally down-right
            else if (particleRight == null && grid[X + 1, Y + 1] == null)
            {
                grid[X, Y] = null;
                grid[X + 1, Y + 1] = this;
                X++;
                Y++;
            }
            // Check if the particle can move diagonally down-left
            else if (particleLeft == null && grid[X - 1, Y + 1] == null)
            {
                grid[X, Y] = null;
                grid[X - 1, Y + 1] = this;
                X--;
                Y++;
            }
            // Otherwise, the particle should remain in place
            else
            {
                // The particle is in a stable position, so it doesn't move
                return;
            }
        }
    }
}





            // If the space below is occupied, and the left side is occupied, move right
/*             else if (particleLeft != null && particleRight == null)
            {
                // Move right
                grid[X + 1, Y] = this;
                grid[X, Y] = null;
                X++;
            } */

            // If the space below is occupied, and the right side is occupied, move left
/*             else if (particleRight != null && particleLeft == null)
            {
                // Move left
                grid[X - 1, Y] = this;
                grid[X, Y] = null;
                X--;
            } */