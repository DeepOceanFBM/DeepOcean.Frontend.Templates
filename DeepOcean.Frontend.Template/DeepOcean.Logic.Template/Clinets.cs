using CAT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicProject
{
    public class Clinets
    {
        //Get
        public async Task<ServiceResponseModel<List<LogicProject.Model.Clients>>> Get(string Index, string pagezie)
        {
            if (string.IsNullOrEmpty(Index) || string.IsNullOrEmpty(pagezie))
            {
                return new() { Success = false, Message = "Index and page size cannot be null or empty." };
            }

            var pageIndex = int.Parse(Index);
            var pageSize = int.Parse(pagezie);

            var api = await DeepOcean.SDK.ApiClinet.HttpClinet.Get<LogicProject.Model.Clients>("Clients", pageIndex: pageIndex, pageSize: pageSize, updatedAfter: null);

            return api;
        }
 

        //Post
        public async Task<ServiceResponseModel<bool>> Add(string ClinetJson)
        {
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            LogicProject.Model.Clients? clients = System.Text.Json.JsonSerializer.Deserialize<LogicProject.Model.Clients>(ClinetJson, options);

            if (clients == null)
            {
                return new() { Success = false, Message = "Invalid client data." };
            }

            var api = await DeepOcean.SDK.ApiClinet.HttpClinet.Save("Clients", clients);

            return api ?? new ServiceResponseModel<bool>() { Message = "Failed to save client.", Success = false, CodeStatus = 500 };
        }

        //Delete
        public async Task<ServiceResponseModel<object>?> Delete(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int clientId))
            {
                return new ServiceResponseModel<object>() { Success = false, Message = "Invalid client ID." };
            }

            // Assuming DeepOcean SDK provides a Delete method by table name and ID
            var api = await DeepOcean.SDK.ApiClinet.HttpClinet.Delete("Clients", clientId);

            return api ?? new ServiceResponseModel<object>() { Message = "Failed to delete client.", Success = false, CodeStatus = 500 };
        }
    }
}
