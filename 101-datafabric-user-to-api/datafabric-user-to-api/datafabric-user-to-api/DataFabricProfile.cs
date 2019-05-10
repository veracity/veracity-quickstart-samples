using System;

namespace datafabric_user_to_api
{
    

    public class DataFabricProfile
    {
        public Guid userId { get; set; }
        public Guid companyId { get; set; }
        public string role { get; set; }
    }

}
