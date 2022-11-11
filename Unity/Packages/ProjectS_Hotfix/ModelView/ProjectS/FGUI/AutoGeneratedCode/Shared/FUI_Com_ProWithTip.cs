/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using Cysharp.Threading.Tasks;

namespace ET
{
    public class FUI_Com_ProWithTipAwakeSystem : AwakeSystem<FUI_Com_ProWithTip, GObject>
    {
        public override void Awake(FUI_Com_ProWithTip self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public class FUI_Com_ProWithTipDestroySystem : DestroySystem<FUI_Com_ProWithTip>
    {
        public override void Destroy(FUI_Com_ProWithTip self)
        {
            self.Destroy();
        }
    }
        
    public sealed class FUI_Com_ProWithTip : FUI
    {	
        public const string UIPackageName = "Shared";
        public const string UIResName = "Com_ProWithTip";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public FUI_Pro_Common m_Pro_Load;
    	public GTextField m_Txt_LoadInfo;
    	public GTextField m_Txt_LoadTip;
    	public const string URL = "ui://btak13q28nuw5";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Com_ProWithTip CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Com_ProWithTip, GObject>(CreateGObject());
        }
        
       
        public static UniTask<FUI_Com_ProWithTip> CreateInstanceAsync(Entity parent)
        {
            UniTaskCompletionSource<FUI_Com_ProWithTip> tcs = new UniTaskCompletionSource<FUI_Com_ProWithTip>();
    
            CreateGObjectAsync((go) =>
            {
                tcs.TrySetResult(parent.AddChild<FUI_Com_ProWithTip, GObject>(go));
            });
    
            return tcs.Task;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Com_ProWithTip Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Com_ProWithTip, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Com_ProWithTip GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Com_ProWithTip>();
        
            if(fui == null)
            {
                fui = Create(domain, go);
            }
        
            fui.isFromFGUIPool = true;
        
            return fui;
        }
            
        public void Awake(GObject go)
        {
            if(go == null)
            {
                return;
            }
            
            GObject = go;	
            
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Id.ToString();
            }
            
            self = (GComponent)go;
            
            self.Add(this);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                
    			m_Pro_Load = FUI_Pro_Common.Create(this, com.GetChildAt(0));
    			m_Txt_LoadInfo = (GTextField)com.GetChildAt(1);
    			m_Txt_LoadTip = (GTextField)com.GetChildAt(2);
    		}
    	}
           
        public override void Destroy()
        {            
            base.Destroy();
            
            self.Remove();
            self = null;
            
    		m_Pro_Load.Dispose();
    		m_Pro_Load = null;
    		m_Txt_LoadInfo = null;
    		m_Txt_LoadTip = null;
    	}
    }
}