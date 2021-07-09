using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;
using TriangleLogoDrawer.ApplicationCore.Services.Exceptions;

namespace TriangleLogoDrawer.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository OrderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            OrderRepository = orderRepository;
        }

        public async Task Create(Order order)
        {
            Task createTask = OrderRepository.Create(order);
            Task increaseNumberTask = IncreaseNumberIfNessisary(order.Shape, order.OrderNumber);
            await Task.WhenAll(createTask, increaseNumberTask);
        }

        private async Task IncreaseNumberIfNessisary(Shape shape, int orderNumberThatMustIncrease)
        {
            Order orderThatAlsoHasNumber = await OrderRepository.GetOrderWithNumber(shape, orderNumberThatMustIncrease);
            if (orderThatAlsoHasNumber != null)
            {
                orderThatAlsoHasNumber.OrderNumber++;
                Task editTask = OrderRepository.Edit(orderThatAlsoHasNumber);
                Task increaseNumberTask = IncreaseNumberIfNessisary(shape, orderThatAlsoHasNumber.OrderNumber);
                await Task.WhenAll(editTask, increaseNumberTask);
            }
        }
        private async Task DecreaseNumberIfNessisary(Shape shape, int orderNumberThatMustDecrease)
        {
            Task<Order> orderThatMustAlsoDecreaseNumberTask = OrderRepository.GetOrderWithNumber(shape, orderNumberThatMustDecrease);
            
            Order orderThatMustAlsoDecreaseNumber = await orderThatMustAlsoDecreaseNumberTask;
            if (orderThatMustAlsoDecreaseNumber != null)
            {
                Task decreaseNumberTask = DecreaseNumberIfNessisary(shape, orderThatMustAlsoDecreaseNumber.OrderNumber+1);

                orderThatMustAlsoDecreaseNumber.OrderNumber--;
                Task editTask = OrderRepository.Edit(orderThatMustAlsoDecreaseNumber);
                
                await Task.WhenAll(editTask, decreaseNumberTask);
            }
        }

        public async Task Remove(Order order)
        {
            Task removeTask = OrderRepository.Remove(order);
            Task decreaseNumberTask = DecreaseNumberIfNessisary(order.Shape, order.OrderNumber+1);
            await Task.WhenAll(removeTask, decreaseNumberTask);
        }

        public async Task Remove(int orderId)
        {
            await Remove(await OrderRepository.Get(orderId));
        }

        public async Task<bool> TriangleNotUsed(Triangle triangle)
        {
            return await OrderRepository.TriangleNotUsed(triangle);
        }

        public int GetOrderNumber(Triangle workingOnTriangle)
        {
            Order orderWithTriangle = OrderRepository.GetAll((order) => { return order.TriangleId == workingOnTriangle.Id; }).Result.FirstOrDefault();
            if (orderWithTriangle == null)
            {
                throw ExceptionProvider.GetNullPointerException(nameof(Order), nameof(Order.TriangleId), workingOnTriangle.Id.ToString());
            }
            return orderWithTriangle.OrderNumber;
        }
    }
}
