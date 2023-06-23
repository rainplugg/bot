namespace Course.Classes
{
   public class Refer
   {
      public long Id { get; set; }
      public int Count { get; set; }
      public Refer(long id, int count)
      {
         Id = id;
         Count = count;
      }
   }
}
