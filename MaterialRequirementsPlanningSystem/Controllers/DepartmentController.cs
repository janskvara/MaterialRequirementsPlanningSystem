using BusinessLogic.DepartmentService;
using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiMaterialRequirementsPlanningSystem.Controllers
{
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _capacityPlanningRepository;
        private readonly IDepartmentQuery _departmentQuery;

        public DepartmentController(IDepartmentRepository capacityPlanningRepository,  IDepartmentQuery departmentQuery)
        {
            _capacityPlanningRepository = capacityPlanningRepository;
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
        [Route("/api/department")]
        public async Task CreateDeparture([FromBody] DepartmentModel request)
        {
            await _departmentQuery.AddNewDepartment(request);
        }

        [HttpGet]
        [Route("/api/department")]
        public async Task<SummaryDepartmentModel> GetSummaryDeparture()
        {
            return await _departmentQuery.GetSummaryDepartment();
        }
    }
}
