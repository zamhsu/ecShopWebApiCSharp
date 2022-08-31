using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Orders;
using Repository.Interfaces;
using Service.Dtos.Payments;
using Service.Interfaces.Payments;

namespace Service.Implments.Payments
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public PaymentMethodService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 取得一筆付款方式
        /// </summary>
        /// <param name="id">付款方式Id</param>
        /// <returns></returns>
        public async Task<PaymentMethodDto> GetByIdAsync(int id)
        {
            PaymentMethod paymentMethod = await _unitOfWork.Repository<PaymentMethod>()
                .GetAsync(q => q.Id == id);

            PaymentMethodDto dto = _mapper.Map<PaymentMethodDto>(paymentMethod);

            return dto;
        }

        /// <summary>
        /// 取得所有付款方式
        /// </summary>
        /// <returns></returns>
        public async Task<List<PaymentMethodDto>> GetAllAsync()
        {
            IQueryable<PaymentMethod> query = _unitOfWork.Repository<PaymentMethod>().GetAllNoTracking();
            List<PaymentMethod> paymentMethods = await query.ToListAsync();

            List<PaymentMethodDto> dtos = _mapper.Map<List<PaymentMethodDto>>(paymentMethods);

            return dtos;
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