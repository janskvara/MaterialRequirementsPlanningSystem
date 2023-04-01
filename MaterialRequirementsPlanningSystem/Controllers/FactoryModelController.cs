using BusinessLogic.FactoryModelsService;
using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiMaterialRequirementsPlanningSystem.Controllers
{
    public class FactoryModelController : ControllerBase
    {
        private readonly IFactoryModelRepository _factoryModelRepository;
        private readonly IFactoryModelQuery _factoryModelQuery;
        public FactoryModelController(IFactoryModelRepository factoryModelRepository, IFactoryModelQuery factoryModelQuery) 
        {
            _factoryModelRepository= factoryModelRepository;
            _factoryModelQuery= factoryModelQuery;
        }

        [HttpPost]
        [Route("/api/bill-of-materials")]
        public async Task CreateRouteSheet([FromBody] BillOfMaterialsEntity request)
        {
            await _factoryModelRepository.CreateBillOfMaterials(request);
        }

        [HttpPost]
        [Route("/api/factory-model")]
        public async Task CreateFactoryModel([FromBody] FactoryModelEntity request)
        {
            await _factoryModelRepository.CreateFactoryModel(request);
        }

        [HttpGet]
        [Route("/api/factory-model")]
        public async Task<FactoryModelsResponseModel> GetFactoryModels()
        {
            return await _factoryModelQuery.GetFactoryModelsAsync();
        }
    }
}
