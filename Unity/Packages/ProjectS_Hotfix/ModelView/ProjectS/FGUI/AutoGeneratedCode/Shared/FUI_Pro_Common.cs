/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using Cysharp.Threading.Tasks;

namespace ET
{
    public class FUI_Pro_CommonAwakeSystem : AwakeSystem<FUI_Pro_Common, GObject>
    {
        public override void Awake(FUI_Pro_Common self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public class FUI_Pro_CommonDestroySystem : DestroySystem<FUI_Pro_Common>
    {
        public override void Destroy(FUI_Pro_Common self)
        {
            self.Destroy();
        }
    }
        
    public sealed class FUI_Pro_Common : FUI
    {	
        public const string UIPackageName = "Shared";
        public const string UIResName = "Pro_Common";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GProgressBar self;
            
    	public GImage m_n0;
    	public GImage m_bar;
    	public const string URL = "ui://btak13q28nuw2";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Pro_Common CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Pro_Common, GObject>(CreateGObject());
        }
        
       
        public static UniTask<FUI_Pro_Common> CreateInstanceAsync(Entity parent)
        {
            UniTaskCompletionSource<FUI_Pro_Common> tcs = new UniTaskCompletionSource<FUI_Pro_Common>();
    
            CreateGObjectAsync((go) =>
            {
                tcs.TrySetResult(parent.AddChild<FUI_Pro_Common, GObject>(go));
            });
    
            return tcs.Task;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Pro_Common Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Pro_Common, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Pro_Common GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Pro_Common>();
        
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
            
            self = (GProgressBar)go;
            
            self.Add(this);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                
    			m_n0 = (GImage)com.GetChildAt(0);
    			m_bar = (GImage)com.GetChildAt(1);
    		}
    	}
           
        public override void Destroy()
        {            
            base.Destroy();
            
            self.Remove();
            self = null;
            
    		m_n0 = null;
    		m_bar = null;
    	}
    }
}