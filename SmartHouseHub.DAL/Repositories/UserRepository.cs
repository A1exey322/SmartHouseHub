﻿using SmartHouseHub.DAL.EF;
using SmartHouseHub.DAL.Interfaces;
using SmartHouseHub.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseHub.DAL.Repositories
{
    public class UserRepository : IBaseRepository<User>
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) 
        {
            _context = context;
        }


        public async Task Create(User entity)
        {
            _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }

        public async Task<User> Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
