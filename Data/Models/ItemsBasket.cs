namespace Shop.Data.Models
{
    public class ItemsBasket
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public Items Item { get; set; }
    }
}