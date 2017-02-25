﻿using ImgShareDemo.BO.Entities;
using ImgShareDemo.DAL.Repositories;
using ImgShareDemo.DAL.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShareDemo.DAL
{
    public class UnitOfWork
    {
        #region fields
        private bool _disposed = false;
        private ImgShareDemoContext _context;

        private Lazy<IUserRepository> _userRepository;
        #endregion

        #region Properties
        public IUserRepository UserRepository => _userRepository.Value;
        #endregion

        #region Constructors
        public UnitOfWork()
        {
            _context = new ImgShareDemoContext();
            SetRepositories();
        }
        #endregion

        #region Methods
        private void SetRepositories()
        {

            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}