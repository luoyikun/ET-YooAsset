//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using Cysharp.Threading.Tasks;



using ET;

namespace ET.cfg
{
   
public partial class Tables
{
    public LuBanSample.TbLuBanSample TbLuBanSample {get; private set; }
    public SkillConfig.TbSkillCanvas TbSkillCanvas {get; private set; }
    public UnitConfig.TbUnitRes TbUnitRes {get; private set; }
    public UnitConfig.TbUnitBase TbUnitBase {get; private set; }
    public UnitConfig.TbUnitAttribute TbUnitAttribute {get; private set; }
    public SceneConfig.TbSceneBase TbSceneBase {get; private set; }

    public async UniTask LoadAsync(System.Func<string, UniTask<ByteBuf>> loader)
    {
        var tables = new System.Collections.Generic.Dictionary<string, object>();
        TbLuBanSample = new LuBanSample.TbLuBanSample(await loader("lubansample_tblubansample")); 
        tables.Add("LuBanSample.TbLuBanSample", TbLuBanSample);
        TbSkillCanvas = new SkillConfig.TbSkillCanvas(await loader("skillconfig_tbskillcanvas")); 
        tables.Add("SkillConfig.TbSkillCanvas", TbSkillCanvas);
        TbUnitRes = new UnitConfig.TbUnitRes(await loader("unitconfig_tbunitres")); 
        tables.Add("UnitConfig.TbUnitRes", TbUnitRes);
        TbUnitBase = new UnitConfig.TbUnitBase(await loader("unitconfig_tbunitbase")); 
        tables.Add("UnitConfig.TbUnitBase", TbUnitBase);
        TbUnitAttribute = new UnitConfig.TbUnitAttribute(await loader("unitconfig_tbunitattribute")); 
        tables.Add("UnitConfig.TbUnitAttribute", TbUnitAttribute);
        TbSceneBase = new SceneConfig.TbSceneBase(await loader("sceneconfig_tbscenebase")); 
        tables.Add("SceneConfig.TbSceneBase", TbSceneBase);

        PostInit();
        TbLuBanSample.Resolve(tables); 
        TbSkillCanvas.Resolve(tables); 
        TbUnitRes.Resolve(tables); 
        TbUnitBase.Resolve(tables); 
        TbUnitAttribute.Resolve(tables); 
        TbSceneBase.Resolve(tables); 
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        TbLuBanSample.TranslateText(translator); 
        TbSkillCanvas.TranslateText(translator); 
        TbUnitRes.TranslateText(translator); 
        TbUnitBase.TranslateText(translator); 
        TbUnitAttribute.TranslateText(translator); 
        TbSceneBase.TranslateText(translator); 
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}