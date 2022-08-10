using Microsoft.EntityFrameworkCore;
using Repository.Entities.Orders;
using Repository.Interfaces;
using Service.Interfaces.Payments;

namespace Service.Implments.Payments
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentMethodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 取得一筆付款方式
        /// </summary>
        /// <param name="id">付款方式Id</param>
        /// <returns></returns>
        public async Task<PaymentMethod> GetByIdAsync(int id)
        {
            PaymentMethod paymentMethod = await _unitOfWork.Repository<PaymentMethod>()
                .GetAsync(q => q.Id == id);

            return paymentMethod;
        }

        /// <summary>
        /// 取得所有付款方式
        /// </summary>
        /// <returns></returns>
        public async Task<List<PaymentMethod>> GetAllAsync()
        {
            IQueryable<PaymentMethod> query = _unitOfWork.Repository<PaymentMethod>().GetAllNoTracking();
            List<PaymentMethod> paymentMethods = await query.ToListAsync();

            return paymentMethods;
        }

        /// <summary>
        /// 付款方式是否為允許使用
        /// </summary>
        /// <param name="id">付款方式Id</param>
        /// <returns></returns>
        public async Task<bool> IsAllowedMethodAsync(int id)
        {
            PaymentMethod paymentMethod = await _unitOfWork.Repository<PaymentMethod>()
                .GetAsync(q => q.Id == id);

            return paymentMethod != null;
        }
    }
}