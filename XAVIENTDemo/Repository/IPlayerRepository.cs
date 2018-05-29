using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XAVIENTDemo.Model;

namespace XAVIENTDemo.Repository
{
   public interface IPlayersRepository
    {
        Task<bool> CreateTableAsync(bool RecreateIfexists);
        Task<bool> SavePlayersAsync(Player player);
        Task<Player> GetPlayerByIDAsync(Int32 playerId);
        Task<List<Player>> GetPlayersAsync();
        Task<bool> DeletePlayersAsync(Player player);
        Task<bool> UpdatePlayersAsync(Player player);



    }
}
