using DataAcess.Entities;
using DataAcess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiMaterialRequirementsPlanningSystem.Controllers
{
    public class ComponentInformationController: ControllerBase
    {
        private readonly IComponentInformationRepository _componentInformationRepository;

        public ComponentInformationController(IComponentInformationRepository componentInformationRepository)
        {
            _componentInformationRepository = componentInformationRepository;
        }

        [HttpPost]
        [Route("/api/component-information")]
        public async Task AddComponentInformation([FromBody] ComponentInformationEntity componentInformation)
        {
            await _componentInformationRepository.SaveComponentInformation(componentInformation);
        }

        [HttpPost]
        [Route("/api/product-information")]
        public async Task AddProductInformation([FromBody] ProductInformationEntity componentInformation)
        {
            await _componentInformationRepository.SaveProductInformation(componentInformation);
        }

    }
}
