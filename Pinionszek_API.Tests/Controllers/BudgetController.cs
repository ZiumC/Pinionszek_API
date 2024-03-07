using AutoMapper;
using FakeItEasy;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Controller
{
    public class BudgetController
    {
        private readonly IBudgetApiService _budgetService;
        private readonly IMapper _mapper;

        public BudgetController()
        {
            _budgetService = A.Fake<IBudgetApiService>();
            _mapper = A.Fake<IMapper>();
        }
    }
}
