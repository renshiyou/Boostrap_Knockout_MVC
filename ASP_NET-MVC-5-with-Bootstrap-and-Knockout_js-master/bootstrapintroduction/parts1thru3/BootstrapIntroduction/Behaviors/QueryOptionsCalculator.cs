﻿using BootstrapIntroduction.ViewModels;
using System;

namespace BootstrapIntroduction.Behaviors
{
    public class QueryOptionsCalculator
    {
        public static int CalculateStart(QueryOptions queryOptions)
        {
            return (queryOptions.CurrentPage - 1) * queryOptions.PageSize;
        }

        public static int CaclulateTotalPages(int count, int pageSize)
        {
            return (int)Math.Ceiling((double)count / pageSize);
        }
    }
}