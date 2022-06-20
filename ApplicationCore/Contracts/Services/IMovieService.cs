﻿using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IMovieService
    {
        List<MovieCardModel> GetTopGrossingMovies();
        MovieDetailsModel GetMovieDetails(int id);
    }
}