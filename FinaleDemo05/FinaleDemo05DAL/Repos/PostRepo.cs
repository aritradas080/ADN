﻿using FinaleDemo05DAL.Interfaces;
using FinaleDemo05DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaleDemo05DAL.Repos
{
    internal class PostRepo : Repo, IRepo<Post, int, bool>
    {
        public bool Create(Post obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Post> Read()
        {
           return db.Posts.ToList();
        }

        public Post Read(int id)
        {
            return db.Posts.Find(id);
        }

        public bool Update(Post obj)
        {
            throw new NotImplementedException();
        }
    }
}
