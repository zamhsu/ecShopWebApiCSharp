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
    /// 優惠券狀態參數
    /// </summary>
    public enum CouponStatusPara
    {
        OK = 1,
        Full = 2,
        Delete = 3
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
    /// 會員身份
    /// </summary>
    public enum MemberRolePara
    {
        AdminMember = 1,
        Customer = 2
    }

    /// <summary>
    /// 常用的雜湊演算法參數
    /// </summary>
    public enum GeneralHashAlgorithmPara
    {
        SHA256,
        SHA512
    }
}