using System;
using AxGrid.Base;
using SmartFormat;
using TMPro;
using UnityEngine;

namespace AxGrid.Tools.Binders
{
    public class UIGameObjectActiveDataBind : Binder
    {
        /// <summary>
        /// ѕоле при изменении которого будет срабатывать событие
        /// </summary>
        public string activeField = "";

        /// <summary>
        /// јктивен по умолчанию
        /// </summary>
        public bool defaultActive = true;

        [OnAwake]
        public void awake()
        {
            activeField = activeField == "" ? $"{gameObject.name}Active" : activeField;
        }

        [OnStart]
        protected void StartThis()
        {
            try
            {
                Model.EventManager.AddAction($"On{activeField}Changed", Changed);
                Changed();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        [OnDestroy]
        protected void DestroyThis()
        {
            try
            {
                Model.EventManager.RemoveAction($"On{activeField}Changed", Changed);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        protected virtual void Changed()
        {
            gameObject.SetActive(Model.GetBool(activeField, defaultActive));
        }
    }
}