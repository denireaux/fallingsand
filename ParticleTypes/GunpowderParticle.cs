using FallingSand.ParticleTypes;
using System;

namespace FallingSand.ParticleTypes
{
    public class GunpowderParticle : Particle
    {
        public GunpowderParticle(int x, int y) : base(x, y)
        {
            isHot = false;
            isCold = false;
            Velocity = 0f;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            Velocity += gravity;

            // Check if the GunpowderParticle should combust
            bool combustionReady = shouldCombust(grid);

            if (combustionReady)
            {
                Explode(grid);
            }

            // Try to move downwards based on current velocity
            for (int i = 0; i < (int)Math.Floor(Velocity); i++)
            {
                int newY = Y + 1;

                if (newY < grid.GetLength(1) && grid[X, newY] == null)
                {
                    grid[X, newY] = this;
                    grid[X, Y] = null;
                    Y = newY;
                }
                else
                {
                    Velocity = 0f;

                    int direction = (new Random().Next(2) == 0) ? -1 : 1;
                    int newX = X + direction;

                    if (newX >= 0 && newX < grid.GetLength(0) && grid[newX, Y] == null)
                    {
                        grid[newX, Y] = this;
                        grid[X, Y] = null;
                        X = newX;
                    }

                    break;
                }
            }

            // If Gunpowder is heated, trigger explosion or burn
            if (isHot)
            {
                Explode(grid);
            }
        }

        private void Explode(Particle[,] grid)
        {
            // Get surrounding particles
            Particle[] surrounding = GetSurroundingParticles(grid);

            foreach (var particle in surrounding)
            {
                if (particle != null)
                {
                    if (particle.isHot)
                    {
                        grid[particle.X, particle.Y] = new FireParticle(particle.X, particle.Y);
                    }
                    else if (particle is GunpowderParticle gunpowder)
                    {
                        // Ignite neighboring gunpowder particles
                        gunpowder.isHot = true;
                    }
                }
            }

            // Emit smoke (VaporParticle) around the gunpowder particle's location
            EmitSmoke(grid);

            // Remove the gunpowder particle after explosion
            grid[X, Y] = null;
        }

        private void EmitSmoke(Particle[,] grid)
        {
            Particle[] surrounding = GetSurroundingParticles(grid);

            foreach (var spot in surrounding)
            {
                if (spot == null)
                {
                    int smokeX = (spot == surrounding[0]) ? X - 1 : (spot == surrounding[1]) ? X + 1 : X;
                    int smokeY = (spot == surrounding[2]) ? Y - 1 : (spot == surrounding[3]) ? Y + 1 : Y;

                    if (smokeX >= 0 && smokeX < grid.GetLength(0) && smokeY >= 0 && smokeY < grid.GetLength(1))
                    {
                        grid[smokeX, smokeY] = new VaporParticle(smokeX, smokeY);
                    }
                }
            }
        }

        private bool shouldCombust(Particle[,] grid)
        {
            Particle[] particlesNear = GetSurroundingParticles(grid);

            foreach (Particle particle in particlesNear)
            {
                if (particle != null && particle.isHot) { return true; }
            }
            return false;
        }
        public override void MoveSelf(Particle[,] grid, int newX, int newY) {}
    }
}
