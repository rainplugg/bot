namespace Course.Classes
{
   public class Catalog
   {
      public int Id { get; set; }
      public string Category { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public string Seller { get; set; }
      public string Source { get; set; }
      public Catalog(int id, string category, string name, string description, string seller, string source)
      {
         Id = id;
         Name = name;
         Category = category;
         Description = description;
         Seller = seller;
         Source = source;
      }
   }
}
