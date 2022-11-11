/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using Cysharp.Threading.Tasks;

namespace ET
{
    public class FUI_LoadingAwakeSystem : AwakeSystem<FUI_Loading, GObject>
    {
        public override void Awake(FUI_Loading self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public class FUI_LoadingDestroySystem : DestroySystem<FUI_Loading>
    {
        public override void Destroy(FUI_Loading self)
        {
            self.Destroy();
        }
    }
        
    public sealed class FUI_Loading : FUI
    {	
        public const string UIPackageName = "Loading";
        public const string UIResName = "Loading";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GImage m_n2;
    	public FUI_Com_ProWithTip m_Pro_Load;
    	public Transition m_t0;
    	public const string URL = "ui://enltropwpxxk0";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Loading CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Loading, GObject>(CreateGObject());
        }
        
       
        public static UniTask<FUI_Loading> CreateInstanceAsync(Entity parent)
        {
            UniTaskCompletionSource<FUI_Loading> tcs = new UniTaskCompletionSource<FUI_Loading>();
    
            CreateGObjectAsync((go) =>
            {
                tcs.TrySetResult(parent.AddChild<FUI_Loading, GObject>(go));
            });
    
            return tcs.Task;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Loading Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Loading, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Loading GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Loading>();
        
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
                
    			m_n2 = (GImage)com.GetChildAt(0);
    			m_Pro_Load = FUI_Com_ProWithTip.Create(this, com.GetChildAt(1));
    			m_t0 = com.GetTransitionAt(0);
    		}
    	}
           
        public override void Destroy()
        {            
            base.Destroy();
            
            self.Remove();
            self = null;
            
    		m_n2 = null;
    		m_Pro_Load.Dispose();
    		m_Pro_Load = null;
    		m_t0 = null;
    	}
    }
}