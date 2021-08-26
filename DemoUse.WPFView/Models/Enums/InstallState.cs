using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoUse.WPFView.Models.Enums
{
    /// <summary>
    /// 執行時的狀態
    /// </summary>
    public enum InstallState
    {
        /// <summary>
        /// 開始
        /// </summary>
        Initializing,
        /// <summary>
        /// 已安裝
        /// </summary>
        Present,
        /// <summary>
        /// 未安裝
        /// </summary>
        NotPresent,
        /// <summary>
        /// 進行中
        /// </summary>
        Applying,
        /// <summary>
        /// 取消
        /// </summary>
        Cancelled,
        /// <summary>
        /// 進行結束
        /// </summary>
        Applied,
        Failed,
    }
}
