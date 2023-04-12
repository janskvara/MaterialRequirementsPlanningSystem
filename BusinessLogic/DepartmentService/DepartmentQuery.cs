using BusinessLogic.ComponentInformationService;
using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;

namespace BusinessLogic.DepartmentService
{
    public interface IDepartmentQuery
    {
        public Task AddNewDepartment(DepartmentModel departmentModel);
        public Task<SummaryDepartmentModel> GetSummaryDepartment();
        public Task<DepartmentEntity> GetDepartment(int departmentId);
    }

    public class DepartmentQuery: IDepartmentQuery
    {
        private readonly IDepartmentRepository _repository;
        private readonly IProductInformationQuery _productInformationQuery;

        public DepartmentQuery(IDepartmentRepository departmentRepository, IProductInformationQuery productInformationQuery) 
        {
            _repository= departmentRepository;
            _productInformationQuery= productInformationQuery;
        }

        public async Task AddNewDepartment(DepartmentModel departmentModel)
        {
            var departmentEntity = new DepartmentEntity
            {
                Id = departmentModel.Id,
                Name = departmentModel.Name,
                TypeOfProduction = departmentModel.TypeOfProduction,
                WorkDaysOfWeek = departmentModel.WorkDaysOfWeek,
                ShiftsPerDay = departmentModel.ShiftsPerDay.
                Select(x => new Shift(x.Item1, x.Item2)).ToList()           
            };
           await _repository.AddNewDepartmentAsync(departmentEntity);
        }

        public async Task<DepartmentEntity> GetDepartment(int departmentId)
        {
            return await _repository.GetDepartmentAsync(departmentId);
        }

        public async Task<SummaryDepartmentModel> GetSummaryDepartment()
        {
            var department = await _repository.GetDepartmentAsync(1);
            var products = await _productInformationQuery.GetProductsInformation(department.Id);

            return new SummaryDepartmentModel
            {
                Id = department.Id,
                Name = department.Name,
                ShiftsPerDay = department.ShiftsPerDay,
                WorkDaysOfWeek = department.WorkDaysOfWeek.Select(x => x.ToString()).ToList(),
                TypeOfProduction = department.TypeOfProduction.ToString(),
                MaximumCapacityPerDay = ProductionCapacityCalculator.CalculateProductionCapacityPerWorkDay(department, products.First().RouteSheet.StationList),
                MaximumCapacityPerWeek = ProductionCapacityCalculator.CalculateProductionCapacityPerWeek(department, products.First().RouteSheet.StationList), 
                Products = products.ToDictionary(keySelector: m => m.PartNumber, elementSelector: m => m)
            };
        }
    }
}
