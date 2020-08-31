using System.Collections.Generic;

namespace Taxi.Web.Data.Entities
{
    public class UserGroupEntity //Group of people that has access to my trips!! So they know Where I'm
    {
        public int Id { get; set; }
        public UserEntity User { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}
