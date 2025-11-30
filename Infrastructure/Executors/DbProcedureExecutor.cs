using Application.DTOs.Responses;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Executors
{
    public class DbProcedureExecutor : IDbProcedureExecutor
    {
        private readonly ApplicationDbContext _context;
        public DbProcedureExecutor(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> GetAllUsersAsync()
        {
            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "dbo.GetLeaderboardAll";
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = await cmd.ExecuteReaderAsync();
            var allUsers = new List<dynamic>();

            while (await reader.ReadAsync())
            {
                // Tạo object dynamic từ từng record
                var user = new ExpandoObject() as IDictionary<string, object>;
                user["UserId"] = reader.GetString(0);
                user["UserName"] = reader.GetString(1);
                user["FullName"] = reader.GetString(2);
                user["TotalPoints"] = reader.GetInt32(3);

                allUsers.Add(user);
            }

            return allUsers;
        }
    }
}
