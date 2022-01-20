﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Core.Routing
{
    public interface IRouteCommandExecutor
    {
        Response.Response ExecuteCommand(IRouteCommand command);
    }
}
