﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaleDemo03DAL.Interface
{
    public interface IRepo<Type, id, Ret>
    {
        List<Type> Get();

        Type Get(id id);

        Ret Create(Type type);

        Ret Update(Type type);  

        Ret Delete(id id);

     
    }
}
