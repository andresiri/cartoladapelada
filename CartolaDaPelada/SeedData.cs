﻿using Domain.Entities;
using Repositorio.Contexto;
using System.Linq;

namespace CartolaDaPelada
{
    public static class SeedData
    {
        public static void EnsureSeedData(this Context context)
        {
            if (context.AllMigrationsApplied())
            {
                if (!context.User.Any())
                {
                    var user1 = new User { Email = "andremirannda@gmail.com", FirstName = "Andre", LastName = "Miranda", Password = "andresiri" };
                    var user2 = new User { Email = "heliofeliciano@gmail.com", FirstName = "Helio", LastName = "Feliciano", Password = "andresiri" };

                    context.User.AddRange(user1, user2);

                    context.SaveChanges();
                }  
                
                if (!context.Pelada.Any())
                {                               
                    var user1 = context.User.First(w => w.FirstName.Equals("Andre"));
                    var pelada1 = new Pelada { Description = "Pelada Teste", CreatedByUserId = user1.Id };

                    context.Pelada.AddRange(pelada1);

                    context.SaveChanges();
                }
            }
        }
    }
}
