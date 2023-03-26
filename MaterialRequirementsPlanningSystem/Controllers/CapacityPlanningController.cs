using BusinessLogic.CapacityPlanningService;
using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiMaterialRequirementsPlanningSystem.Controllers
{
    public class CapacityPlanningController : ControllerBase
    {
        private readonly ICapacityPlanningRepository _capacityPlanningRepository;
        private readonly ICapacityPlanningQuery _capacityPlanningQuery;

        public CapacityPlanningController(ICapacityPlanningRepository capacityPlanningRepository, 
            ICapacityPlanningQuery capacityPlanningQuery)
        {
            _capacityPlanningRepository = capacityPlanningRepository;
            _capacityPlanningQuery= capacityPlanningQuery;
        }

        [HttpPost]
        [Route("/api/route-sheet")]
        public async Task CreateRouteSheet([FromBody] RouteSheetEntity request)
        {
            await _capacityPlanningRepository.CreateRouteSheet(request);
        }

        [HttpPost]
        [Route("/api/station")]
        public async Task CreateStation([FromBody] StationEntity request)
        {
            await _capacityPlanningRepository.CreateStation(request);
        }

        [HttpPost]
        [Route("/api/departure")]
        public async Task CreateDeparture([FromBody] DepartmentEntity request)
        {
            await _capacityPlanningRepository.CreateDepartment(request);
        }

        [HttpGet]
        [Route("/api/route-sheet")]
        public async Task<RouteSheetResponseModel> GetRouteSheet([FromQuery] int routeSheetId)
        {
            return await _capacityPlanningQuery.GetRouteSheet(routeSheetId);
        }
    }
}
