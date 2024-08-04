using Microsoft.Xna.Framework;

namespace FallingSand.ParticleTypes
{
    public class StoneParticle : Particle
    {
        public StoneParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
        }

        public override void Update(float gravity, Particle[,] grid) {}
    }
}