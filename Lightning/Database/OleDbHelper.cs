namespace Lightning.Database
{
    public static class OleDbHelper
    {
        /// <summary>
        /// 获取连接Excel的Oledb连接字符串
        /// </summary>
        /// <param name="fileFullName">Excel文件路径</param>
        /// <param name="hasTitle">第一行是否为标题</param>
        /// <param name="isWrite">读写模式：true为读写，false为只读</param>
        /// <returns></returns>
        public static string GetConnectionString(string fileFullName, bool hasTitle, bool isWrite) => $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileFullName};Extended Properties='Excel 12.0;HDR={(hasTitle ? "YES" : "NO")};IMEX={(isWrite ? 0 : 1)}'";

        /// <summary>
        /// 获取连接Excel的Oledb连接字符串
        /// </summary>
        /// <param name="fileFullName">Excel文件路径</param>
        /// <param name="hasTitle">第一行是否为标题</param>
        /// <param name="isWrite">读写模式：true为读写，false为只读</param>
        /// <returns></returns>
        public static string GetConnection8String(string fileFullName, bool hasTitle, bool isWrite) => $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileFullName};Extended Properties='Excel 12.0;HDR={(hasTitle ? "YES" : "NO")};IMEX={(isWrite ? 0 : 1)}'";

    }
}