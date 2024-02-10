﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, 
            ILoggerManager logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        }

        public CompanyDto GetCompany(Guid id, bool trackChanges) 
        {
            var company = _repository.Company.GetCompany(id, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(id);

            var companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);

            _repository.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;
        }

        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var companiesEntities =
                _repository.Company.GetByIds(ids, trackChanges);

            if (ids.Count() != companiesEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var companiesToReturn = 
                _mapper.Map<IEnumerable<CompanyDto>>(companiesEntities);

            return companiesToReturn;


        }
    }
}
