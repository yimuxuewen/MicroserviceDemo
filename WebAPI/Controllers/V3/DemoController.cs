using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.V3
{
    [Route("api/v3/[controller]")]
    [ApiExplorerSettings(GroupName = "V3")]
    [ApiController]
    public class DemoController : ControllerBase
    {
       

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
