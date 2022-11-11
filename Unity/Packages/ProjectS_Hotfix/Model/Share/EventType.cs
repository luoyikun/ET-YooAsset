using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        public struct SceneChangeStart
        {
        }

        public struct SceneChangeFinish
        {
        }

        public struct PingChange
        {
            public long Ping;
        }

        public struct AfterCreateClientScene
        {
        }
        
        public struct AfterCreateSingleGameScene_Logic
        {
        }


        public struct AfterCreateCurrentScene
        {
        }

        public struct AfterCreateLoginScene
        {
        }

        public struct AppStartInitFinish
        {
        }

        public struct LoginFinish
        {
        }

        public struct LoadingBegin
        {
            public string SceneName;
            public List<string> ResList;
        }

        public struct LoadingFinish
        {
        }

        public struct EnterGameMapFinish
        {
        }

        public struct AfterUnitCreate_Logic
        {
            public bool IsMonest;
            public int UnitConfigId;
            public string UnitName;
        }

        public struct LoadConfig
        {
            public Dictionary<string, byte[]> configBytes;
        }
        
        public struct NumericChange
        {
            public NumericComponent NumericComponent;
            public NumericType NumericType;
            public float Result;
        }
    }
}