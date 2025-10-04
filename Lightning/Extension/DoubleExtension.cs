namespace Lightning.Extension
{
    public static class DoubleExtension
    {
        /// <summary>
        /// 返回指定精度的double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="acc">acc需大于0</param>
        /// <returns></returns>
        public static double Accuracy(this double value, int acc)
        {
            return acc < 1 ? double.Parse(value.ToString("F0")) : double.Parse((value / acc).ToString("F0")) * acc;
        }
    }
}