﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class FavoriteModel
    {
        public int MovieId { get; set; }
        public bool Toggle { get; set; }
        public bool IsFavorite { get; set; }
    }
}
