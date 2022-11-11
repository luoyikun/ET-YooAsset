//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月29日 9:41:17
//------------------------------------------------------------

using UnityEngine;

namespace ET
{
    public class FUILoginComponentAwakeSystem : AwakeSystem<FUI_LoginComponent, FUI_Login>
    {
        public override void Awake(FUI_LoginComponent self, FUI_Login fuiLogin)
        {
            fuiLogin.m_Btn_Login.self.onClick.Add(() =>
            {
                FUI_LoginUtilities.OnLogin(self).Forget();
            });

            self.FuiUIPanelLogin = fuiLogin;
            self.FuiUIPanelLogin.m_Tween_LoginPanelFlyIn.Play();
        }
    }
    
    
    public class FUILoginComponentDestroySystem : DestroySystem<FUI_LoginComponent>
    {
        public override void Destroy(FUI_LoginComponent self)
        {
            self.FuiUIPanelLogin = null;
        }
    }
}