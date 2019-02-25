using UnityEngine;
using System.Collections.Generic;

namespace Factories
{
    public static class RelicFactory
    {

        static RelicFactory()
        {
            EnemyDeath.OnDeathDropRelic += CreateItem;
        }

        public static Dictionary<string, bool> PlayerHasRelic = new Dictionary<string, bool>()
        {
            {"Trident",false},
            {"BatWings",false}
        };


        private static void CreateItem(GameObject relic, Vector3 deadEnemyPosition)
        {
            Object.Instantiate(relic, deadEnemyPosition, Quaternion.identity);
        }
    }
}
