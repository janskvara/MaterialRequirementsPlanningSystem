using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;

namespace BusinessLogic.DepartmentService
{
    public interface IDepartmentQuery
    {
        public Task AddNewDepartment(DepartmentModel departmentModel);
    }

    public class DepartmentQuery: IDepartmentQuery
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentQuery(IDepartmentRepository departmentRepository) 
        {
            _repository= departmentRepository;
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

    }
}
