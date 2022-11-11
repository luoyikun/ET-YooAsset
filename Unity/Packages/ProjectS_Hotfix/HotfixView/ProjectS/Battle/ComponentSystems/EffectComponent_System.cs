// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月25日, 星期一
// --------------------------

using UnityEngine;
using UnityEngine.VFX;

namespace ET
{
    public class EffectComponent_AwakeSystem0 : AwakeSystem<EffectComponent>
    {
        public override void Awake(EffectComponent self)
        {
        }
    }

    public class EffectComponent_DestroySystem0 : DestroySystem<EffectComponent>
    {
        public override void Destroy(EffectComponent self)
        {
        }
    }


    public static class EffectComponent_Utilities
    {
        /// <summary>
        /// 添加一个特效
        /// </summary>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        public static void Add(this EffectComponent self, string name, GameObject unit)
        {
            if (self.AllEffects.ContainsKey(name))
            {
                return;
            }

            self.AllEffects.Add(name, unit);
        }

        /// <summary>
        /// 移除一个特效
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(this EffectComponent self, string name)
        {
            if (self.AllEffects.TryGetValue(name, out var tempUnit))
            {
                if (tempUnit.GetComponent<ParticleSystem>() != null)
                {
                    tempUnit.GetComponent<ParticleSystem>().Stop();
                }
                else
                {
                    tempUnit.GetComponent<VisualEffect>().Stop();
                }

                GameObjectPoolComponent.Instance.RecycleGameObject(name, tempUnit);
                self.AllEffects.Remove(name);
            }
        }

        /// <summary>
        /// 播放一个特效
        /// </summary>
        /// <param name="name"></param>
        public static void Play(this EffectComponent self, string name, GameObject unit)
        {
            //处理特效冲突
            self.HandleConflict(name);

            //播放特效
            if (self.AllEffects.TryGetValue(name, out var tempUnit))
            {
                if (tempUnit.GetComponent<ParticleSystem>() != null)
                {
                    tempUnit.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    tempUnit.GetComponent<VisualEffect>().Play();
                }
            }
            else
            {
                self.Add(name, unit);
                if (unit.GetComponent<ParticleSystem>() != null)
                {
                    unit.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    unit.GetComponent<VisualEffect>().Play();
                }
            }
        }

        /// <summary>
        /// 检查一个特效的状态，如果正在播放就返回True
        /// </summary>
        /// <param name="effectNameToBeChecked"></param>
        /// <returns></returns>
        public static bool CheckState(this EffectComponent self, string effectNameToBeChecked)
        {
            if (self.AllEffects.TryGetValue(effectNameToBeChecked, out var unit))
            {
                if (unit.GetComponent<ParticleSystem>() != null)
                {
                    return unit.GetComponent<ParticleSystem>().isPlaying;
                }
                else if (unit.GetComponent<VisualEffect>() != null)
                {
                    return !unit.GetComponent<VisualEffect>().pause;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// 处理特效冲突
        /// </summary>
        /// <param name="name"></param>
        public static void HandleConflict(this EffectComponent self, string name)
        {
            //如果互斥列表中不包含此项，说明不与其他特效互斥
            if (!self.effectGroup.Contains(name)) return;
            //查看他是否与特效组里面的一些特效冲突，如果是就移除当前冲突的特效，而播放他
            foreach (var vfxName in self.effectGroup)
            {
                //是同一个特效，就不需要做操作
                if (vfxName == name)
                {
                    continue;
                }

                //如果当前播放的特效中不含VARIABLE，就不需要做操作
                if (!self.AllEffects.ContainsKey(vfxName))
                {
                    continue;
                }

                //如果它并没有在播放，就不需要操作
                if (!self.CheckState(vfxName))
                {
                    continue;
                }

                //将目标特效移除
                self.Remove(vfxName);
                //Log.Info($"停止了{VARIABLE1}");
            }
        }
    }
}