﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib.Jwt;
public interface IJwtService
{
    public string GenerateToken(string UserId);
}
