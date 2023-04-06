using BusinessLogic.Models;
using BusinessLogic.MRPService;
using DataAcess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiMaterialRequirementsPlanningSystem.Controllers
{
    public class MRPController: ControllerBase
    {
        private readonly IProductionShedule _productionShedule;
        private readonly IMRPRepository _mrpRepository;
        public MRPController(IProductionShedule productionShedule, IMRPRepository mrpRepository)
        {
            _productionShedule= productionShedule;
            _mrpRepository= mrpRepository;
        }

        [HttpPost]
        [Route("/api/mrp-production-sheduler")]
        public async Task PlanProductionSchedule([FromBody]OrderPlanRequest request)
        {
            await _productionShedule.Plan(request);
        }

        [HttpGet]
        [Route("/api/mrp-production-sheduler")]
        public async Task<ProductionSheduleReportsModel> GetProductionScheduleReports()
        {
            return await _productionShedule.GetReports();
        }

        [HttpGet]
        [Route("/api/shift-summaries")]
        public async Task<ShiftSummaryResponseModel> GetShiftSummaries()
        {
            return new ShiftSummaryResponseModel
            {
                ShiftsSummary = await _mrpRepository.GetShiftSummaries()
            };
        }
    }
}
