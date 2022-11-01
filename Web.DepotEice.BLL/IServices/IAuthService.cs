﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DepotEice.BLL.Models;

namespace Web.DepotEice.BLL.IServices
{
    public interface IAuthService
    {
        Task<string> SignInAsync(SignInModel signInModel);
        Task<string> SignUpAsync(SignUpModel signUpModel);
    }
}
