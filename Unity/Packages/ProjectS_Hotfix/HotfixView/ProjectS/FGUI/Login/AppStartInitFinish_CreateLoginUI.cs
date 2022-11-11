using Cysharp.Threading.Tasks;
using ET.EventType;

namespace ET
{
    [Event(SceneType.Client)]
    public class OpenLoginUI_AppStartInitFinish : AEvent<Scene, AppStartInitFinish>
    {
        protected override async UniTask Run(Scene entity, AppStartInitFinish a)
        {
            var scene = entity;
            
            await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.Login);
            FUI_Login fuiLogin = await FUI_Login.CreateInstanceAsync(scene);
            fuiLogin.MakeFullScreen();
            
            FUI_LoginComponent fuiLoginComponent = fuiLogin.AddChild<FUI_LoginComponent, FUI_Login>(fuiLogin);
            
            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();
            fuiManagerComponent.Add(FUIPackage.Login, fuiLogin, fuiLoginComponent);
        }
    }
}