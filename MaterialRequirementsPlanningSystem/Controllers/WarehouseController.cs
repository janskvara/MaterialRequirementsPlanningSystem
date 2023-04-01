using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiMaterialRequirementsPlanningSystem.Controllers
{
    public class WarehouseController: ControllerBase
    {
        private readonly IWarehouseRepository _warehouseRepository;
        public WarehouseController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository= warehouseRepository;
        }

        [HttpPost]
        [Route("/api/warehouse-goods")]
        public async Task AddGoods(GoodsEntity goods)
        {
            await _warehouseRepository.AddGoodsToProductionWarehouse(goods);
        }

        [HttpGet]
        [Route("/api/warehouse-goods")]
        public async Task<List<GoodsModel>> GetAllGoods()
        {
            var goods =  await _warehouseRepository.GetAllGoodsInProductionWarehouse();
            return goods.Select(x => new GoodsModel(x.PartNumber, x.Description, x.Unit,
                x.MaxCapacityOfProduct, x.ActualCapacity, x.MinimumThresholdCapacity)).ToList();
        }
    }
}
