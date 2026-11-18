using System;
using System.Data;
using AdminBackOffice.Models;

namespace AdminBackOffice.Data
{
    public static class AgentRepository
    {
        public static DataTable Search(string keyword)
        {
            return Database.Query("SELECT AgentID,AgentName,Address,Phone FROM dbo.Agent WHERE AgentName LIKE @keyword OR Phone LIKE @keyword ORDER BY AgentID",
                Database.Param("@keyword", "%" + (keyword ?? "").Trim() + "%"));
        }

        public static DataTable Lookup()
        {
            return Database.Query("SELECT AgentID,AgentName FROM dbo.Agent ORDER BY AgentName");
        }

        public static void Save(AgentRecord agent)
        {
            if (string.IsNullOrWhiteSpace(agent.AgentName) || string.IsNullOrWhiteSpace(agent.Address))
                throw new ArgumentException("Agent name and address are required.");
            if (string.IsNullOrWhiteSpace(agent.Phone) || agent.Phone.Trim().Length < 9 || agent.Phone.Trim().Length > 20)
                throw new ArgumentException("Phone must be 9 to 20 characters.");
            if (agent.AgentID == 0)
            {
                Database.Execute("INSERT dbo.Agent(AgentName,Address,Phone) VALUES (@name,@address,@phone)",
                    Database.Param("@name", agent.AgentName.Trim()), Database.Param("@address", agent.Address.Trim()),
                    Database.Param("@phone", agent.Phone.Trim()));
            }
            else
            {
                var count = Database.Execute("UPDATE dbo.Agent SET AgentName=@name,Address=@address,Phone=@phone WHERE AgentID=@id",
                    Database.Param("@name", agent.AgentName.Trim()), Database.Param("@address", agent.Address.Trim()),
                    Database.Param("@phone", agent.Phone.Trim()), Database.Param("@id", agent.AgentID));
                if (count != 1) throw new DBConcurrencyException("Agent no longer exists. Refresh the list.");
            }
        }

        public static void Delete(int agentId)
        {
            var count = Database.Execute("DELETE dbo.Agent WHERE AgentID=@id", Database.Param("@id", agentId));
            if (count != 1) throw new DBConcurrencyException("Agent no longer exists. Refresh the list.");
        }
    }
}
