using System;
using UnityEngine;

namespace Snowy.CustomAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class SnReadOnly : PropertyAttribute
    {
        // Token: 0x060001C8 RID: 456 RVA: 0x00012160 File Offset: 0x00010360
        public SnReadOnly(string conditionPropertyName = "", bool inverse = false, bool disableonfalse = true)
        {
            this.ConditionPropertyName = conditionPropertyName;
            this.Inverse = inverse;
            this.DisableOnFalse = disableonfalse;
        }

        // Token: 0x040002DA RID: 730
        public string ConditionPropertyName;

        // Token: 0x040002DB RID: 731
        public bool Inverse;

        // Token: 0x040002DC RID: 732
        public bool DisableOnFalse;
    }
}