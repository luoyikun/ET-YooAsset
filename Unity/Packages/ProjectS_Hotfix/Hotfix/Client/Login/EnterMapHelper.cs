using System;
using Cysharp.Threading.Tasks;


namespace ET.Client
{
    public static class EnterMapHelper
    {
        public static async UniTaskVoid EnterMapAsync(Scene clientScene)
        {
            try
            {
                G2C_EnterMap g2CEnterMap = await clientScene.GetComponent<SessionComponent>().Session.Call(new C2G_EnterMap()) as G2C_EnterMap;
                clientScene.GetComponent<PlayerComponent>().MyId = g2CEnterMap.MyId;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }	
        }
    }
}