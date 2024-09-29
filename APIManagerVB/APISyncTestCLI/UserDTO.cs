using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISyncTestCLI
{
    public class UserDTO
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("job")]
        public string Job { get; private set; }

        public UserDTO(string name, string job)
        {
            Name = name;
            Job = job;
        }
    }
}
