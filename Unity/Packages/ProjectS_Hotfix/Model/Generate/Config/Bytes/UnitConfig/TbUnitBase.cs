//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;

namespace ET.cfg.UnitConfig
{
   
public partial class TbUnitBase
{
    private readonly Dictionary<int, UnitConfig.UnitBaseConfig> _dataMap;
    private readonly List<UnitConfig.UnitBaseConfig> _dataList;
    
    public TbUnitBase(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, UnitConfig.UnitBaseConfig>();
        _dataList = new List<UnitConfig.UnitBaseConfig>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            UnitConfig.UnitBaseConfig _v;
            _v = UnitConfig.UnitBaseConfig.DeserializeUnitBaseConfig(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, UnitConfig.UnitBaseConfig> DataMap => _dataMap;
    public List<UnitConfig.UnitBaseConfig> DataList => _dataList;

    public UnitConfig.UnitBaseConfig GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public UnitConfig.UnitBaseConfig Get(int key) => _dataMap[key];
    public UnitConfig.UnitBaseConfig this[int key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}