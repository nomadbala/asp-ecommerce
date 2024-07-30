using PaymentService.Contracts;
using PaymentService.Models;
using PaymentService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentService.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _repository;
        private readonly IEpayService _epayService;

        public PaymentsService(IPaymentsRepository repository, IEpayService epayService)
        {
            _repository = repository;
            _epayService = epayService;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Payment> CreateAsync(CreatePaymentContract contract)
        {
            var paymentResponse = await _epayService.MakePaymentAsync();

            contract = contract with
            {
                Status = paymentResponse.Status == "Success" ? PaymentStatus.Successful : PaymentStatus.Unsuccessful
            };

            return await _repository.CreateAsync(contract);
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");

            return await _repository.GetByIdAsync(id);
        }

        public async Task<Payment> UpdateAsync(Guid id, UpdatePaymentContract contract)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");

            return await _repository.UpdateAsync(id, contract);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");

            await _repository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new BadHttpRequestException($"Invalid user id. {nameof(userId)}");

            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new BadHttpRequestException($"Invalid order id. {nameof(orderId)}");

            return await _repository.GetByOrderIdAsync(orderId);
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
        {
            if (!Enum.IsDefined(typeof(PaymentStatus), status))
                throw new BadHttpRequestException($"Invalid payment status. {nameof(status)}");

            return await _repository.GetByStatusAsync(status);
        }
    }
}
