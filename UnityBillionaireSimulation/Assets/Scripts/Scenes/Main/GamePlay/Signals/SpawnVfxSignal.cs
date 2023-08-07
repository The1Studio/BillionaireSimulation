namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using UnityEngine;

    public class SpawnVfxSignal
    {
        public string    VfxID;
        public Vector3   Position;
        public float     LifeTime;
        public Transform Parent;

        public SpawnVfxSignal(string vfxID, Vector3 position, float lifeTime = 2f, Transform parent= null)
        {
            this.VfxID    = vfxID;
            this.Position = position;
            this.LifeTime = lifeTime;
            this.Parent   = parent;
        }
    }
}