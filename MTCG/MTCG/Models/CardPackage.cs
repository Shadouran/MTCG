﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class CardPackage
    {
        public Guid Uid { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
