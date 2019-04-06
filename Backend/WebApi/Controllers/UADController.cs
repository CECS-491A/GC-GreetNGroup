using System;
using System.Web.Http;
using ManagerLayer.UADManagement;
namespace WebApi.Controllers
{
    public class UADController : ApiController
    {
        [HttpGet]
        [Route("api/UAD")]
        public string DisplayUserAnalysisData()
        {
            string b;
            UADManager uadManager = new UADManager();
            b = uadManager.LoginVSRegistered("March");

           
            return b;
        }


    }
}