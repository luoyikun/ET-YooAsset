//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 11:05:41
//------------------------------------------------------------

using System;
using ET;
using UnityEngine;

namespace ET
{
    public class UserInputComponentStartSystem : AwakeSystem<UserInputComponent>
    {
        public override void Awake(UserInputComponent self)
        {
            self.startTime = TimeHelper.ClientNow();
        }
    }
    
    public class UserInputComponentUpdateSystem : UpdateSystem<UserInputComponent>
    {
        public override void Update(UserInputComponent self)
        {
            self.Update();
        }
    }

    public class UserInputComponent : Entity, IAwake, IUpdate
    {
        public bool RightMouseDown { get; set; }
        public bool RightMouseUp { get; set; }
        
        public bool LeftMouseDown { get; set; }
        public bool LeftMouseUp { get; set; }

        public bool ADown_long { get; set; }
        public bool ADown { get; set; }
        public bool AUp { get; set; }

        public bool ADouble { get; set; }
        public double ALastClickTime { get; set; }

        public double DLastClickTime { get; set; }
        public bool DDown { get; set; }
        public bool DUp { get; set; }

        public bool DDouble { get; set; }
        public bool DDown_long { get; set; }

        public double QLastClickTime { get; set; }
        public bool QDown { get; set; }
        public bool QUp { get; set; }

        public bool QDouble { get; set; }
        public bool QDown_long { get; set; }

        public bool WDown_long { get; set; }
        public bool WDown { get; set; }
        public bool WUp { get; set; }
        public bool WDouble { get; set; }
        public double WLastClickTime { get; set; }

        public bool EDown_long { get; set; }
        public bool EDown { get; set; }
        public bool EUp { get; set; }
        public bool EDouble { get; set; }
        public double ELastClickTime { get; set; }

        public bool RDown_long { get; set; }
        public bool RDown { get; set; }
        public bool RUp { get; set; }
        public bool RDouble { get; set; }
        public double RLastClickTime { get; set; }

        public bool JDown_long { get; set; }
        public bool JDown { get; set; }
        public bool JUp { get; set; }
        public bool JDouble { get; set; }
        public double JLastClickTime { get; set; }

        public bool SDown_long { get; set; }
        public bool SDown { get; set; }
        public bool SUp { get; set; }
        public bool SDouble { get; set; }
        public double SLastClickTime { get; set; }

        public bool SpaceDown_long { get; set; }
        public bool SpaceDown { get; set; }
        public bool SpaceUp { get; set; }
        public double SpaceLastClickTime { get; set; }

        public long currentTime;

        public long startTime;

        public void Update()
        {
            ResetAllState();

            this.currentTime = TimeHelper.ClientNow() - this.startTime;

            this.CheckKey();
            this.CheckKeyUp();
            this.CheckKeyDown();
        }

        /// <summary>
        /// 检查按键抬起
        /// </summary>
        private void CheckKeyUp()
        {
            if (Input.GetMouseButtonUp(0))
            {
                this.LeftMouseDown = false;
                this.LeftMouseUp = true;
            }
            
            if (Input.GetMouseButtonUp(1))
            {
                this.RightMouseDown = false;
                this.RightMouseUp = true;
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                ADown_long = false;
                this.ADouble = false;
                this.AUp = true;
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                DDown_long = false;
                this.DDouble = false;
                this.DUp = true;
            }

            if (Input.GetKeyUp(KeyCode.J))
            {
                this.JDown_long = false;
                this.JDouble = false;
                this.JUp = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                this.SpaceDown_long = false;
                this.SpaceUp = true;
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                this.WDown_long = false;
                this.WUp = true;
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                this.QDown_long = false;
                this.QUp = true;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                this.EDown_long = false;
                this.EUp = true;
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                this.RDown_long = false;
                this.RUp = true;
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                this.SDown_long = false;
                this.SUp = true;
            }
        }

        /// <summary>
        /// 检查按键落下
        /// </summary>
        void CheckKeyDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.LeftMouseDown = true;
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                this.RightMouseDown = true;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                this.ADown = true;

                if ((this.currentTime - this.ALastClickTime) / 1000f <= 0.5f)
                {
                    this.ADouble = true;
                }
                else
                {
                    this.ADouble = false;
                }

                this.ALastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                this.WDown = true;

                if ((this.currentTime - this.WLastClickTime) / 1000f <= 0.5f)
                {
                    this.WDouble = true;
                }
                else
                {
                    this.WDouble = false;
                }

                this.WLastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.QDown = true;

                if ((this.currentTime - this.QLastClickTime) / 1000f <= 0.5f)
                {
                    this.QDouble = true;
                }
                else
                {
                    this.QDouble = false;
                }

                this.QLastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                this.EDown = true;

                if ((this.currentTime - this.ELastClickTime) / 1000f <= 0.5f)
                {
                    this.EDouble = true;
                }
                else
                {
                    this.EDouble = false;
                }

                this.ELastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                this.RDown = true;

                if ((this.currentTime - this.RLastClickTime) / 1000f <= 0.5f)
                {
                    this.RDouble = true;
                }
                else
                {
                    this.RDouble = false;
                }

                this.RLastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                this.DDown = true;

                if ((this.currentTime - this.DLastClickTime) / 1000f <= 0.5f)
                {
                    this.DDouble = true;
                }
                else
                {
                    this.DDouble = false;
                }

                this.DLastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                this.SDown = true;

                if ((this.currentTime - this.SLastClickTime) / 1000f <= 0.5f)
                {
                    this.SDouble = true;
                }
                else
                {
                    this.SDouble = false;
                }

                this.SLastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                this.JDown = true;

                if ((this.currentTime - this.JLastClickTime) / 1000f <= 0.5f)
                {
                    this.JDouble = true;
                }
                else
                {
                    this.JDouble = false;
                }

                this.JLastClickTime = this.currentTime;
            }
        }

        /// <summary>
        /// 检查按键输入
        /// </summary>
        private void CheckKey()
        {
            if (Input.GetKey(KeyCode.A))
            {
                ADown_long = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                DDown_long = true;
            }

            if (Input.GetKey(KeyCode.J))
            {
                JDown_long = true;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                QDown_long = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                SDown_long = true;
            }


            if (Input.GetKey(KeyCode.Space))
            {
                SpaceDown_long = true;
            }

            if (Input.GetKey(KeyCode.W))
            {
                WDown_long = true;
            }

            if (Input.GetKey(KeyCode.E))
            {
                EDown_long = true;
            }

            if (Input.GetKey(KeyCode.R))
            {
                RDown_long = true;
            }
        }

        private void ResetAllState()
        {
            this.LeftMouseDown = false;
            this.LeftMouseUp = false;
            
            this.RightMouseDown = false;
            this.RightMouseUp = false;

            this.AUp = false;
            this.ADown = false;

            this.DDown = false;
            this.DUp = false;

            this.JUp = false;
            this.JDown = false;

            this.WUp = false;
            this.WDown = false;

            this.SpaceUp = false;
            this.SpaceDown = false;

            this.QUp = false;
            this.QDown = false;

            this.SUp = false;
            this.SDown = false;

            this.EUp = false;
            this.EDown = false;

            this.RUp = false;
            this.RDown = false;
        }
    }
}