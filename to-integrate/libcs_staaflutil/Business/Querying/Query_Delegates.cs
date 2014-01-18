

namespace Common.Queries
{
      public interface IEntity
      {
            Argument Instantiate(Argument arg);
      }

      //public delegate bool Query_Predicate(object obj);

      public delegate bool Query_Predicate(IEntity entity);






}