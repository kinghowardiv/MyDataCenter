

namespace MyDataCenter.Models.POCOS
{
    public class Expense
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
        public string MonthId { get; set; }
        public bool IsSelected { get; set; }
    }
}