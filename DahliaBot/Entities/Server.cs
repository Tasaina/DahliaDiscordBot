using Discord.WebSocket;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DahliaBot.Entities
{
    public class Server
    {
        [Key] public ulong Id { get; set; }
        [NotMapped]public SocketRole ActiveUserRole { get; set; }
        public Server()
        {
        }

        public Server(ulong id)
        {
            Id = id;
         }
    }
}