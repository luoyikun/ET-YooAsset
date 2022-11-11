using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ET.Client;
using UnityEngine;

namespace ET
{
    public class GameObjectPoolComponent : Entity, IAwake, IDestroy
    {
        public static GameObjectPoolComponent Instance { get; set; }

        /// <summary>
        /// 所有Prefab的缓存
        /// </summary>
        public Dictionary<string, GameObject> AllPrefabs = new Dictionary<string, GameObject>();

        public Dictionary<string, Queue<GameObject>> AllCachedGos = new Dictionary<string, Queue<GameObject>>();
    }
}