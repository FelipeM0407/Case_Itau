using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseItau.API.src.Domain.DTOs;

namespace CaseItau.API.src.Application.UseCases
{
    public class CreateFundoResult
    {
        public bool Success { get; set; }
        public bool FundoJaExiste { get; set; }
        public FundoDTO Fundo { get; set; }
        public string ErrorMessage { get; set; }
    }
}