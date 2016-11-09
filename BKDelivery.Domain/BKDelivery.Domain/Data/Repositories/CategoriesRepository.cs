using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data.Repositories
{

    public interface ICategoriesRepository
    {
        IEnumerable<Category> GetAll();
        void Add(Category category);
    }

    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<Category> _set;

        public CategoriesRepository(BkDeliveryContext db)
        {
            _db = db;
            _set = _db.Categories;
        }

        public IEnumerable<Category> GetAll()
        {
            return _set.AsEnumerable();
        }

        public void Add(Category category)
        {
            _set.Add(category);
        }
    }
}
