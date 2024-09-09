namespace Shopping_tutorial.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public List<OrderDetails> Items { get; set; }
        public OrderModel Order { get; set; }
    }
}
