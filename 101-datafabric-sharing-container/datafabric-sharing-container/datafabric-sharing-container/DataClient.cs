using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace datafabric_sharing_container
{
    /// <summary>
    /// A helper class that takes care of authenticated calls towards data fabric api
    /// </summary>
    public class DataClient : VeracityClient
    {

        public DataClient(IOptions<DataFabricApiOptions> options) : base(options)
        {
            
        }

        public async Task<DataFabricProfile> MyProfile()
        {
            return await SendAsync<DataFabricProfile>("users/me");
        }

        public async Task<List<DataFabricContainer>> GetContainersSharedWithMe()
        {
            return await SendAsync<List<DataFabricContainer>>("resources");
        }

        public async Task<Guid> ShareContainerWithUser(DataFabricContainer container, Guid user, KeyTemplate keyTemplate, string comment, bool autoRefreshable)
        {
            var response =  await SendAsync<dynamic>($"resources/{container.id}/accesses?autoRefreshed={autoRefreshable}", new
            {
                userId = user.ToString(),
                accessKeyTemplateId = keyTemplate.id,
                comment
            });
            return response.accessSharingId;
        }

        public async Task<KeyTemplate> GetKeyTemplate(bool readAccess, bool writeAccess, bool deleteAccess, bool listAccess, int durationHours)
        {

            var templates =  await SendAsync<List<KeyTemplate>>("keytemplates");
            return templates.FirstOrDefault(template => template.attribute1 == readAccess && template.attribute2 == writeAccess &&
                                      template.attribute3 == deleteAccess && template.attribute4 == listAccess &&
                                      template.totalHours == durationHours);
        }
    }
}