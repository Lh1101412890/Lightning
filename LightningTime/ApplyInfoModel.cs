namespace LightningTime
{
    public class ApplyInfoModel
    {
        public string Note { get; set; }
        public string Product { get; set; }
        public string Guid { get; set; }
        public bool IsForever { get; set; }
        public string Date { get; set; }
        public long Days { get; set; }
        /// <summary>
        /// 申请码
        /// </summary>
        public string Code { get; set; }
    }
}