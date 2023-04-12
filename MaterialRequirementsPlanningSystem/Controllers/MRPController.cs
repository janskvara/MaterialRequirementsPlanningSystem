using BusinessLogic.Models;
using BusinessLogic.MRPService;
using Microsoft.AspNetCore.Mvc;

namespace ApiMaterialRequirementsPlanningSystem.Controllers
{
    public class MRPController: ControllerBase
    {
        private readonly IProductionShedule _productionShedule;
        public MRPController(IProductionShedule productionShedule)
        {
            _productionShedule= productionShedule;
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
    }
}
