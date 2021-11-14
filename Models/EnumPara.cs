namespace WebApi.Models
{
    /// <summary>
    /// 產品狀態參數
    /// </summary>
    public enum ProductStatusPara
    {
        OK = 1,
        Delete = 2
    }

    /// <summary>
    /// 管理員狀態參數
    /// </summary>
    public enum AdminMemberStatusPara
    {
        OK = 1,
        Stop = 2,
        Delete = 3,
        Lock = 4
    }

    /// <summary>
    /// 雜湊演算法參數
    /// </summary>
    public enum HashAlgorithmPara
    {
        SHA256,
        SHA512
    }
}