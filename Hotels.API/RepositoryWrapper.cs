using System;
using Hotels.API.Repositories;
using Hotels.API.UnitOfWorks;

namespace Hotels.API
{
    public class RepositoryWrapper
    {
        IUnitOfWork _unitOfWork;

        public RepositoryWrapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private RoomRepository _roomRepository = null;
        private UserRepository _userRepository = null;

        public RoomRepository RoomRepository
        {
            get
            {
                if (_roomRepository == null)
                    _roomRepository = new RoomRepository(_unitOfWork);
                return _roomRepository;
            }
        }

        public UserRepository UserRepository
        {
            get 
            {
                if(_userRepository == null)
                    _userRepository = new UserRepository(_unitOfWork);
                return _userRepository;
            }
        }

    }
}
