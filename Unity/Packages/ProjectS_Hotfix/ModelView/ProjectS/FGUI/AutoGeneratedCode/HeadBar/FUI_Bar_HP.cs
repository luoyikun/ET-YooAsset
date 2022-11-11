/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using Cysharp.Threading.Tasks;

namespace ET
{
    public class FUI_Bar_HPAwakeSystem : AwakeSystem<FUI_Bar_HP, GObject>
    {
        public override void Awake(FUI_Bar_HP self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public class FUI_Bar_HPDestroySystem : DestroySystem<FUI_Bar_HP>
    {
        public override void Destroy(FUI_Bar_HP self)
        {
            self.Destroy();
        }
    }
        
    public sealed class FUI_Bar_HP : FUI
    {	
        public const string UIPackageName = "HeadBar";
        public const string UIResName = "Bar_HP";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GProgressBar self;
            
    	public GImage m_bar;
    	public const string URL = "ui://ny5r4rwcu6tfi";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Bar_HP CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Bar_HP, GObject>(CreateGObject());
        }
        
       
        public static UniTask<FUI_Bar_HP> CreateInstanceAsync(Entity parent)
        {
            UniTaskCompletionSource<FUI_Bar_HP> tcs = new UniTaskCompletionSource<FUI_Bar_HP>();
    
            CreateGObjectAsync((go) =>
            {
                tcs.TrySetResult(parent.AddChild<FUI_Bar_HP, GObject>(go));
            });
    
            return tcs.Task;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Bar_HP Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Bar_HP, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Bar_HP GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Bar_HP>();
        
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
                
    			m_bar = (GImage)com.GetChildAt(0);
    		}
    	}
           
        public override void Destroy()
        {            
            base.Destroy();
            
            self.Remove();
            self = null;
            
    		m_bar = null;
    	}
    }
}