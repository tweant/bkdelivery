using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Data;

namespace BKDelivery.CallCenter.Model
{
    public interface IUnitOfWorkService
    {
        bool IsActive { get; }
        void InitializeTransaction();
        IUnitOfWork UnitOfWork { get; }
        void SaveChanges();
    }
}
