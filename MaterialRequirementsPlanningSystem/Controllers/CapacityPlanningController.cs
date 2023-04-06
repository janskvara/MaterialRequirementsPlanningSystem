using BusinessLogic.DepartmentService;
using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiMaterialRequirementsPlanningSystem.Controllers
{
    public class CapacityPlanningController : ControllerBase
    {
        private readonly IDepartmentRepository _capacityPlanningRepository;
        private readonly ICapacityPlanningQuery _capacityPlanningQuery;
        private readonly IDepartmentQuery _departmentQuery;

        public CapacityPlanningController(IDepartmentRepository capacityPlanningRepository, 
            ICapacityPlanningQuery capacityPlanningQuery, IDepartmentQuery departmentQuery)
        {
            _capacityPlanningRepository = capacityPlanningRepository;
            _capacityPlanningQuery = capacityPlanningQuery;
            _departmentQuery = departmentQuery;
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
            await _capacityPlanningRepository.CreateStationAsync(request);
        }

        [HttpPost]
        [Route("/api/departure")]
        public async Task CreateDeparture([FromBody] DepartmentModel request)
        {
            await _departmentQuery.AddNewDepartment(request);
        }

        [HttpGet]
        [Route("/api/route-sheet")]
        public async Task<RouteSheetResponseModel> GetRouteSheet([FromQuery] int routeSheetId)
        {
            return await _capacityPlanningQuery.GetRouteSheetAsync(routeSheetId);
        }
    }
}
