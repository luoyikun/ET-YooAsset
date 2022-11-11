using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace ET
{
    public class CodeLoader : IDisposable
    {
        public static CodeLoader Instance = new CodeLoader();

        public Action Update;
        public Action FixedUpdate;
        public Action LateUpdate;
        public Action OnApplicationQuit;

        private Assembly assembly;

        private CodeLoader()
        {
            
        }

        public void Dispose()
        {
            
        }

        public async UniTask Start()
        {
            byte[] assBytes = (await YooAssetProxy.GetRawFileAsync("Code_ProjectS_Hotfix.dll")).GetRawBytes();
            byte[] pdbBytes = (await YooAssetProxy.GetRawFileAsync("Code_ProjectS_Hotfix.pdb")).GetRawBytes();

            assembly = Assembly.Load(assBytes, pdbBytes);

            Dictionary<string, Type> types =
                AssemblyHelper.GetAssemblyTypes(typeof(Game).Assembly, this.assembly);
            Game.EventSystem.Add(types);

            IStaticMethod start = new MonoStaticMethod(assembly, "ET.Client.Entry", "Start");
            start.Run();
        }
    }
}