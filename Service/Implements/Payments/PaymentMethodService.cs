using Microsoft.EntityFrameworkCore;
using Repository.Entities.Orders;
using Repository.Interfaces;
using Service.Interfaces.Payments;

namespace Service.Implments.Payments
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;

        public PaymentMethodService(IRepository<PaymentMethod> paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        /// <summary>
        /// 取得一筆付款方式
        /// </summary>
        /// <param name="id">付款方式Id</param>
        /// <returns></returns>
        public async Task<PaymentMethod> GetByIdAsync(int id)
        {
            PaymentMethod paymentMethod = await _paymentMethodRepository.GetAsync(q => q.Id == id);

            return paymentMethod;
        }

        /// <summary>
        /// 取得所有付款方式
        /// </summary>
        /// <returns></returns>
        public async Task<List<PaymentMethod>> GetAllAsync()
        {
            IQueryable<PaymentMethod> query = _paymentMethodRepository.GetAllNoTracking();
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
            PaymentMethod paymentMethod = await _paymentMethodRepository.GetAsync(q => q.Id == id);

            return paymentMethod != null;
        }
    }
}