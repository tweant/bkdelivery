using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Data;

namespace BKDelivery.CallCenter.Model
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        public UnitOfWorkService()
        {
            IsActive = false;
        }

        public bool IsActive { get; private set; }

        private IUnitOfWork _unitOfWork;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                if(!IsActive) throw new Exception("UnitOfWork hasn't been initialised.");
                return _unitOfWork;
            }
            private set
            {
                if (IsActive) throw new Exception("UnitOfWork has already been assigned to.");
                _unitOfWork = value;
            }
        }

        public void InitializeTransaction()
        {
            if (IsActive) throw new Exception("UnitOfWork has already been initialised.");
            UnitOfWork = new GenericUnitOfWork();
            IsActive = true;
        }


        public void SaveChanges()
        {
            if (!IsActive) throw new Exception("UnitOfWork hasn't been initialised.");
            UnitOfWork.SaveChanges();
            IsActive = false;
            UnitOfWork = null;
        }
    }
}