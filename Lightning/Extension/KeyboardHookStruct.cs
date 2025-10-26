using System.Runtime.InteropServices;

namespace Lightning.Extension
{
    /// <summary>
    /// 键盘钩子结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardHookStruct
    {
        /// <summary>
        /// 指定一个虚拟键码。该代码必须有一个价值的范围1至254
        /// </summary>
        public int vkCode;
        /// <summary>
        /// 指定的硬件扫描码的关键
        /// </summary>
        public int scanCode;
        /// <summary>
        /// 键标志
        /// </summary>
        public int flags;
        /// <summary>
        /// 指定的时间戳记的这个讯息
        /// </summary>
        public int time;
        /// <summary>
        /// 指定额外信息相关的信息
        /// </summary>
        public int dwExtraInfo;
    }
}