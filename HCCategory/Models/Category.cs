namespace HCCategory.Models
{
    public class Category
    {
        public int Id { get; set; } 
        public string? Name { get; set; }  
    }

    public class Extra
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductId { get; set; }
    }

    public class Product 
    { 
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? CategoryId { get; set;}

    }
    
}
