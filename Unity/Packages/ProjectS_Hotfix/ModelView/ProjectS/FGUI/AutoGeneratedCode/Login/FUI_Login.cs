/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using Cysharp.Threading.Tasks;

namespace ET
{
    public class FUI_LoginAwakeSystem : AwakeSystem<FUI_Login, GObject>
    {
        public override void Awake(FUI_Login self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public class FUI_LoginDestroySystem : DestroySystem<FUI_Login>
    {
        public override void Destroy(FUI_Login self)
        {
            self.Destroy();
        }
    }
        
    public sealed class FUI_Login : FUI
    {	
        public const string UIPackageName = "Login";
        public const string UIResName = "Login";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GImage m_n0;
    	public FUI_Btn_StartGame m_Btn_Login;
    	public FUI_Btn_ContinueGame m_n22;
    	public GGroup m_Gro_LoginInfo;
    	public Transition m_Tween_LoginPanelFlyIn;
    	public const string URL = "ui://2jxt4hn8pdjl9";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Login CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Login, GObject>(CreateGObject());
        }
        
       
        public static UniTask<FUI_Login> CreateInstanceAsync(Entity parent)
        {
            UniTaskCompletionSource<FUI_Login> tcs = new UniTaskCompletionSource<FUI_Login>();
    
            CreateGObjectAsync((go) =>
            {
                tcs.TrySetResult(parent.AddChild<FUI_Login, GObject>(go));
            });
    
            return tcs.Task;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Login Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Login, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Login GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Login>();
        
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
                
    			m_n0 = (GImage)com.GetChildAt(0);
    			m_Btn_Login = FUI_Btn_StartGame.Create(this, com.GetChildAt(1));
    			m_n22 = FUI_Btn_ContinueGame.Create(this, com.GetChildAt(2));
    			m_Gro_LoginInfo = (GGroup)com.GetChildAt(3);
    			m_Tween_LoginPanelFlyIn = com.GetTransitionAt(0);
    		}
    	}
           
        public override void Destroy()
        {            
            base.Destroy();
            
            self.Remove();
            self = null;
            
    		m_n0 = null;
    		m_Btn_Login.Dispose();
    		m_Btn_Login = null;
    		m_n22.Dispose();
    		m_n22 = null;
    		m_Gro_LoginInfo = null;
    		m_Tween_LoginPanelFlyIn = null;
    	}
    }
}