﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.Models.Response
{
    public class ErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ErrorResponse(string errorMessage) : this(new List<string>() { errorMessage }) { }

        public ErrorResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}
