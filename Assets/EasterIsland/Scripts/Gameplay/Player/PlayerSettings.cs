///-----------------------------------------------------------------
///   Author :                     
///   Date   : 06/09/2022 11:04
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player {

    [CreateAssetMenu(menuName = "Isartdigital/PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        public string playerName = string.Empty;

        public Vector3 size = Vector3.zero;
        public TerrainType terrain = TerrainType.AllTerrain;

        public float speed = 0f;
        public float jumpHeight = 0f;

        public KillType killType = KillType.HeadButt;
        public float damage = 0f;

        public float life = 0f;
    }

    public enum KillType
    {
        HeadButt,
        Jump
    }

    public enum TerrainType
    {
        AllTerrain,
        OnlyGrass
    }
}
